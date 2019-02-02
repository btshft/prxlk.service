namespace Prxlk.Application.Shared.DependencyInjection
{
    public interface IScopedServiceFactory<out TService>
    {
        IScopedServiceHolder<TService> CreateScope();
    }
}