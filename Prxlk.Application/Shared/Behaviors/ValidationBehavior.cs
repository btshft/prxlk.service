using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Application.Shared.Validation;

namespace Prxlk.Application.Shared.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IServiceHolder<IValidator<TRequest>> _validatorHolder;

        public ValidationBehavior(IServiceHolder<IValidator<TRequest>> validatorHolder)
        {
            _validatorHolder = validatorHolder;
        }

        /// <inheritdoc />
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IValidatable)
            {
                var validator = _validatorHolder.GetRequiredService();
                var validation = validator.Validate(request);
                
                if (!validation.IsValid)
                    throw new ValidationException(validation);
            }

            return next();
        }
    }
}