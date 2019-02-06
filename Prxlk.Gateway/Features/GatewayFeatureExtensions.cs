using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features
{
    public static class GatewayFeatureExtensions
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            var enabledFeatures = configuration
                .GetSection("Settings:Features")
                .Get<string[]>();

            var featureTypes = typeof(GatewayFeature).Assembly.GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(GatewayFeature).IsAssignableFrom(t))
                .Select(t => new
                {
                    FeatureType = t, 
                    FeatureName = t.GetCustomAttribute<GatewayFeatureAttribute>()?.Name,
                    FeatureOrder = t.GetCustomAttribute<GatewayFeatureAttribute>()?.Order
                })
                .Where(f => f.FeatureName != null)
                .Where(f => enabledFeatures.Contains(f.FeatureName, StringComparer.InvariantCultureIgnoreCase))
                .OrderBy(f => f.FeatureOrder)
                .Select(f => f.FeatureType)
                .ToArray();              
            
            foreach (var featureType in featureTypes)
            {
                var feature = (GatewayFeature)Activator.CreateInstance(featureType);     
                feature.RegisterFeature(services, configuration);                  
                services.AddSingleton(feature);      
            }
            
            return services;
        }

        public static IApplicationBuilder UseFeatures(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var features = builder.ApplicationServices.GetServices<GatewayFeature>()
                .Select(f => new
                {
                    Feature = f,
                    Order = f.GetType()
                        .GetCustomAttribute<GatewayFeatureAttribute>().Order
                });
            
            foreach (var feature in features.OrderBy(f => f.Order).Select(f => f.Feature))
                feature.UseFeature(builder, configuration);

            return builder;
        }

        public static bool IsFeatureEnabled<TFeature>(this IConfiguration configuration) where TFeature : GatewayFeature
        {
            var featureAttribute = typeof(TFeature).GetCustomAttribute<GatewayFeatureAttribute>();
            if (featureAttribute != null)
            {
                var enabledFeatures = configuration.GetSection("Settings:Features")
                    .Get<string[]>();

                if (enabledFeatures.Contains(featureAttribute.Name))
                    return true;
            }

            return false;
        }
    }
}