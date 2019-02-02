using System.Threading;
using System.Threading.Tasks;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Shared.Handlers
{
    public interface IProcessCoordinator<in TEvent> where TEvent : Event
    {
        Task<Message[]> ProcessAsync(TEvent @event, CancellationToken cancellation);
    }
}