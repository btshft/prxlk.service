namespace Prxlk.Gateway.DependencyInjection
{
    public interface IScopedServiceFactory<out TService>
    {
        IScopedService<TService> CreateScope();
    }
}