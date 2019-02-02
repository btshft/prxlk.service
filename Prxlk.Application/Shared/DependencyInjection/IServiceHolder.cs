namespace Prxlk.Application.Shared.DependencyInjection
{
    public interface IServiceHolder<out TService>
    {
        TService GetService();
        TService GetRequiredService();
    }
}