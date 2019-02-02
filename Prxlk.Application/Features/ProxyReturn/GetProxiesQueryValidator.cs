using Prxlk.Application.Shared.Validation;

namespace Prxlk.Application.Features.ProxyReturn
{
    public class GetProxiesQueryValidator : IValidator<GetProxiesQuery>
    {
        /// <inheritdoc />
        public ValidationResult Validate(GetProxiesQuery entity)
        {
            var result = new ValidationResult();
            
            if (entity.Limit <= 0)
                result.Add(new ValidationFailure("Limit should be > 0", nameof(entity.Limit), entity.Limit));
            
            if (entity.Offset.HasValue && entity.Offset < 0)
                result.Add(new ValidationFailure("Offset cannot be < 0", nameof(entity.Offset), entity.Offset));

            return result;
        }
    }
}