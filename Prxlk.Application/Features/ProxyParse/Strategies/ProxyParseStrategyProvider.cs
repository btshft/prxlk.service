using System;
using System.Collections.Generic;
using System.Linq;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse.Strategies
{
    public class ProxyParseStrategyProvider : IProxyParseStrategyProvider
    {
        private readonly IEnumerable<IProxyParseStrategy> _parseStrategies;

        public ProxyParseStrategyProvider(IEnumerable<IProxyParseStrategy> parseStrategies)
        {
            _parseStrategies = parseStrategies;
        }

        /// <inheritdoc />
        public IProxyParseStrategy GetStrategy(ProxySource source)
        {
            if (source == ProxySource.Undefined)
                throw new ArgumentException("Undefined source is not supported");
            
            var strategy = _parseStrategies.FirstOrDefault(s => ProxyParseStrategyExtensions.GetSource(s) == source);
            if (strategy == null)
                throw new Exception($"Strategy for source '{source}' not registered");

            return strategy;
        }
    }
}