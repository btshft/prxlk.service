using Microsoft.Extensions.Logging;

namespace Prxlk.Gateway.Features.Diagnostics
{
    public static class EventEmitterDiagnostic
    {
        public const string ListenerName = "Prxlk.Gateway.EventEmitter";
        public const string EmitExceptionEventName = "Emit.Exception";
        public const string FatalException = "FatalException";
        
        public static readonly EventId EmitExceptionEvent = new EventId(130, $"{ListenerName}.{EmitExceptionEventName}");
        public static readonly EventId FatalExceptionEvent = new EventId(131, $"{ListenerName}.{FatalException}");
    }
}