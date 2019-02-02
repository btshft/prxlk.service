using System;
using Prxlk.Application.Shared.Messages;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyParsed : Event
    {
        public ProxyTransportModel ParsedProxy { get; }
        
        public ProxyParsed(Guid correlationId, ProxyTransportModel parsedProxy) 
            : base(correlationId)
        {
            if (parsedProxy == null)
                throw new ArgumentNullException(nameof(parsedProxy));
            
            ParsedProxy = parsedProxy;
        }
    }
}