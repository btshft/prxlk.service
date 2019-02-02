using System;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyParseFailed : Event
    {
        public ProxyParseFailed(Guid correlationId, string errorMessage)
            : base(correlationId)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}