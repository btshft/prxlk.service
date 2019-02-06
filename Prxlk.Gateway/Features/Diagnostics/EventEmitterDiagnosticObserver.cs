using System;
using Microsoft.Extensions.DiagnosticAdapter;
using Prxlk.Application.Shared.Messages;
using Serilog;

namespace Prxlk.Gateway.Features.Diagnostics
{
    public class EventEmitterDiagnosticObserver : DiagnosticObserver
    {
        private readonly ILogger _logger;

        public EventEmitterDiagnosticObserver()
        {
            _logger = Log.ForContext<EventEmitterDiagnosticObserver>();
        }

        protected override bool IsMatch(string listenerName)
        {
            return listenerName == EventEmitterDiagnostic.ListenerName;
        }

        [DiagnosticName(EventEmitterDiagnostic.EmitExceptionEventName)]
        public void OnEmitException(Exception exception)
        {
            _logger
                .ForContext("EventId",  EventEmitterDiagnostic.EmitExceptionEvent.Id)
                .ForContext("EventName", EventEmitterDiagnostic.EmitExceptionEvent.Name)
                .Error(exception, "Event emit failed {CorrelationId}");
        }

        [DiagnosticName(EventEmitterDiagnostic.FatalException)]
        public void OnFatalException(Exception exception)
        {
            _logger
                .ForContext("EventId",  EventEmitterDiagnostic.FatalExceptionEvent.Id)
                .ForContext("EventName", EventEmitterDiagnostic.FatalExceptionEvent.Name)
                .Fatal(exception, "Fatal exception occured");
        }
    }
}