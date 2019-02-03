using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Prxlk.Gateway.Features.Throttling.Store;
using Prxlk.Gateway.Models;

namespace Prxlk.Gateway.Features.Throttling
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited =  false)]
    public class ThrottleAttribute : Attribute, IFilterFactory
    {
        public string PolicyName { get; }
                      
        /// <inheritdoc />
        public bool IsReusable => false;
        
        public ThrottleAttribute(string policyName)
        {
            if (policyName == null)
                throw new ArgumentNullException(nameof(policyName));
            
            PolicyName = policyName;
        }

        public ThrottleAttribute()
        {
            PolicyName = string.Empty;
        }

        /// <inheritdoc />
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var policyEvaluator = serviceProvider.GetRequiredService<ThrottlePolicyEvaluator>();
            var policy = GetPolicy(serviceProvider);
            
            return new ThrottleAsyncFilter(policy, policyEvaluator);
        }

        private ThrottlePolicy GetPolicy(IServiceProvider serviceProvider)
        {
            var policyStore = serviceProvider.GetRequiredService<IThrottlePolicyStore>();
            var storedPolicy = policyStore.GetOrAdd(PolicyName, _ =>
            {
                var factoryProvider = serviceProvider.GetRequiredService<ThrottlePolicyFactoryProvider>();
                var factory = factoryProvider.GetFactory(PolicyName);

                return factory();
            });

            return storedPolicy;
        }

        private class ThrottleAsyncFilter : IAsyncActionFilter
        {
            private readonly ThrottlePolicy _policy;
            private readonly ThrottlePolicyEvaluator _evaluator;

            public ThrottleAsyncFilter(ThrottlePolicy policy, ThrottlePolicyEvaluator evaluator)
            {
                _policy = policy;
                _evaluator = evaluator;
            }

            /// <inheritdoc />
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var result = _evaluator.ForContext(context)
                    .Evaluate(_policy);

                if (result.ShouldBypass || result.Reset <= DateTime.UtcNow)
                {
                    await next();
                    return;
                }

                if (result.Remaining < 1)
                {
                    var message = new ApiError("API call quota exceed",
                        $"Only {_policy.Limit} request(s) allowed per {_policy.Period} period",
                        context.HttpContext.TraceIdentifier);

                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
                    await context.HttpContext.Response.WriteAsync(
                        JsonConvert.SerializeObject(message, Formatting.Indented));
                    
                    return;
                }

                context.HttpContext.Response.OnStarting(SetResponseHeaders);

                await next();
                
                Task SetResponseHeaders()
                {
                    context.HttpContext.Response.Headers["X-Rate-Limit-Limit"] = result.Limit.ToString("c");
                    context.HttpContext.Response.Headers["X-Rate-Limit-Remaining"] = result.Remaining.ToString("D");
                    context.HttpContext.Response.Headers["X-Rate-Limit-Reset"] =
                        result.Reset.ToString("o", DateTimeFormatInfo.InvariantInfo);
                    
                    return Task.CompletedTask;
                }
            }
        }
    }
}