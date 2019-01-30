using System;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.DependencyInjection
{
    public class ScopedServiceFactory<TService> : IScopedServiceFactory<TService>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public ScopedServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IScopedService<TService> CreateScope()
        {
            return new ScopedService(_serviceProvider.CreateScope());
        }
        
        private class ScopedService : IScopedService<TService>
        {
            private readonly IServiceScope _scope;
                       
            public ScopedService(IServiceScope scope)
            {
                _scope = scope;
            }

            /// <inheritdoc />
            public TService GetService()
            {
                return _scope.ServiceProvider.GetService<TService>();
            }

            /// <inheritdoc />
            public void Dispose()
            {
                _scope?.Dispose();
            }
        }
    }
}