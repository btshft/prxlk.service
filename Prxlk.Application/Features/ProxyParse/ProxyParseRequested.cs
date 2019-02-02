using System;
using Prxlk.Application.Shared.Messages;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyParseRequested : Event
    {
        public ProxySource Source { get; }
        
        public ProxyParseRequested(ProxySource source) 
            : base(Guid.NewGuid())
        {
            if (source == ProxySource.Undefined)
                throw new ArgumentException("Undefined proxy source");
            
            Source = source;
        }
    }
}