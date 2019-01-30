using System;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse.Strategies
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ProxyParseStrategyAttribute : Attribute
    {
        public ProxyParseStrategyAttribute(ProxySource source)
        {
            Source = source;
        }

        public ProxySource Source { get; }
    }
}