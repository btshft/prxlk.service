using System;

namespace Prxlk.Gateway.DependencyInjection
{
    public interface IScopedService<out TService> : IDisposable
    {
        TService GetService();
    }
}