using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prxlk.Application.Shared.Options;

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
                .Select(t => new { FeatureType = t, FeatureName = t.GetCustomAttribute<GatewayFeatureAttribute>()?.Name })
                .Where(f => f.FeatureName != null)
                .Where(f => enabledFeatures.Contains(f.FeatureName, StringComparer.InvariantCultureIgnoreCase))
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
            var features = builder.ApplicationServices.GetServices<GatewayFeature>();
            foreach (var feature in features)
                feature.UseFeature(builder, configuration);

            return builder;
        }
    }
}