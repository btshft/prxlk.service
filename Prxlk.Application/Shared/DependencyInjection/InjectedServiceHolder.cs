using System;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Application.Shared.DependencyInjection
{
    public sealed class InjectedServiceHolder<TService> : IServiceHolder<TService>
    {
        private readonly IServiceProvider _provider;

        public InjectedServiceHolder(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        public TService GetService()
        {
            return _provider.GetService<TService>();
        }

        /// <inheritdoc />
        public TService GetRequiredService()
        {
            return _provider.GetRequiredService<TService>();
        }
    }
}