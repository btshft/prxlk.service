using System;
using Prxlk.Application.Shared.Messages;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyParseCommand : Command
    {
        public ProxySource Source { get; }

        public ProxyParseCommand(Guid correlationId, ProxySource source) 
            : base(correlationId)
        {
            Source = source;
        }
    }
}