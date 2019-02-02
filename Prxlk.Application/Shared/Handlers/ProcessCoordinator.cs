using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Shared.Handlers
{
    public abstract class ProcessCoordinator : INotificationHandler<Event>
    { 
        private readonly IMediator _mediator;
        private readonly Lazy<Type[]> _supportedEventTypes;
        
        protected ProcessCoordinator(IMediator mediator)
        {
            _mediator = mediator;
            _supportedEventTypes = new Lazy<Type[]>(GetSupportedEventTypes);
        }

        /// <inheritdoc />
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            var method = typeof(ProcessCoordinator).GetMethod(
                nameof(ProcessEvent), BindingFlags.Instance | BindingFlags.NonPublic);
            
            var supportedEvents = _supportedEventTypes.Value;
            if (!supportedEvents.Contains(notification.GetType()))
                return;

            // Dynamic dispatch - optimize later (maybe)
            var processMethod = method.MakeGenericMethod(notification.GetType());
            var outputsTask = (Task<Message[]>)processMethod
                .Invoke(this, new object[] {notification, cancellationToken});
            
            var outputs = await outputsTask;
            if (outputs != null)
            {
                foreach (var output in outputs)
                {
                    switch (output)
                    {
                        // Queries are ignored
                        
                        case Command command: 
                            await DispatchCommand(command, cancellationToken);
                            break;
                        
                        case Event @event:
                            await DispatchEvent(@event, cancellationToken);
                            break;
                    }
                }
            }

            Task DispatchCommand(Command command, CancellationToken cancellation)
            {
                var dispatchMethodRef = typeof(IMediator).GetMethod(nameof(IMediator.Send));
                var innerType = command.GetType().GetInterfaces()
                    .First(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IRequest<>))
                    .GetGenericArguments()[0];
                
                var dispatchMethod = dispatchMethodRef.MakeGenericMethod(innerType);
                return (Task) dispatchMethod.Invoke(_mediator, new object[] {command, cancellation});
            }

            Task DispatchEvent(Event @event, CancellationToken cancellation)
            {
                var dispatchMethodRef = typeof(IMediator).GetMethods()
                    .First(m => m.IsGenericMethodDefinition && m.Name == nameof(IMediator.Publish));
                
                var innerType = @event.GetType();
                var dispatchMethod = dispatchMethodRef.MakeGenericMethod(innerType);               
                return (Task) dispatchMethod.Invoke(_mediator, new object[] {@event, cancellation});
            }
        }

        protected Task<Message[]> ProcessEvent<TEvent>(TEvent @event, CancellationToken cancellation)
            where TEvent : Event
        {
            var coordinator = this as IProcessCoordinator<TEvent>;
            if (coordinator == null)
                throw new InvalidCastException($"Unable to cast coordinator to {typeof(IProcessCoordinator<TEvent>).Name}");

            return coordinator.ProcessAsync(@event, cancellation);
        }

        protected Type[] GetSupportedEventTypes()
        {
            var types = GetType().GetInterfaces()
                .Where(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IProcessCoordinator<>))
                .Select(i => i.GetGenericArguments()[0])
                .ToArray();

            return types;
        }
    }
}