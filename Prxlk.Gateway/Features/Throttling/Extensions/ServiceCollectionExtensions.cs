using System;
using Microsoft.Extensions.DependencyInjection;
using Prxlk.Gateway.Features.Throttling.Store;

namespace Prxlk.Gateway.Features.Throttling.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestThrottling(
            this IServiceCollection services, Action<ThrottlePolicyBuilder> configure)
        {
            services.AddSingleton<ThrottlePolicyFactoryProvider>();
            services.AddSingleton<ThrottlePolicyEvaluator>();
            services.AddSingleton<IThrottlePolicyStore, InMemoryThrottlePolicyStore>();
            services.AddSingleton<IThrottleCounterStore, InMemoryThrottleCounterStore>();
            
            var builder = new ThrottlePolicyBuilder(services);
            configure(builder);
            
            return services;
        }
    }
}