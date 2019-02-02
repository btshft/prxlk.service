namespace Prxlk.Application.Shared.Validation
{
    public interface IValidator<in TEntity>
    {
        ValidationResult Validate(TEntity entity);
    }
}