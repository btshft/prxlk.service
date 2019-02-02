using System;

namespace Prxlk.Application.Shared.DependencyInjection
{
    public interface IScopedServiceHolder<out TService> : IServiceHolder<TService>, IDisposable
    {
    }
}