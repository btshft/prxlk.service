using System;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Application.Shared.DependencyInjection
{
    public class ScopedServiceFactory<TService> : IScopedServiceFactory<TService>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public ScopedServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IScopedServiceHolder<TService> CreateScope()
        {
            return new ScopedServiceHolder(_serviceProvider.CreateScope());
        }
        
        private class ScopedServiceHolder : IScopedServiceHolder<TService>
        {
            private readonly IServiceScope _scope;
                       
            public ScopedServiceHolder(IServiceScope scope)
            {
                _scope = scope;
            }

            /// <inheritdoc />
            public TService GetService()
            {
                return _scope.ServiceProvider.GetService<TService>();
            }

            /// <inheritdoc />
            public TService GetRequiredService()
            {
                return _scope.ServiceProvider.GetRequiredService<TService>();
            }

            /// <inheritdoc />
            public void Dispose()
            {
                _scope?.Dispose();
            }
        }
    }
}