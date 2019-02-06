using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Prxlk.Gateway.Features.ScheduledEventEmit.Emitters;

namespace Prxlk.Gateway.Features.ScheduledEventEmit
{
    public static class ScheduledEventEmitFeatureExtensions
    {
        public static bool IsEmitterEnabled<TEmitter>(this IConfiguration configuration)
            where TEmitter : ScheduledEmitter
        {
            var emitterCategoryAttr = typeof(TEmitter).GetCustomAttribute<EventEmitterCategoryAttribute>();
            if (emitterCategoryAttr != null)
            {
                var enabledEmitters = configuration.GetSection("ScheduledEventEmit:Emitters")
                    .Get<string[]>();
                
                if (enabledEmitters.Contains(emitterCategoryAttr.Name))
                    return true;
            }

            return false;
        }
    }
}