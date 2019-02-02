using System;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Logging;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Gateway.Features.Diagnostics
{
    public class EventEmitterDiagnosticObserver : DiagnosticObserver
    {
        private readonly ILogger<EventEmitterDiagnosticObserver> _logger;

        public EventEmitterDiagnosticObserver(ILogger<EventEmitterDiagnosticObserver> logger)
        {
            _logger = logger;
        }

        protected override bool IsMatch(string listenerName)
        {
            return listenerName == EventEmitterDiagnostic.ListenerName;
        }

        [DiagnosticName(EventEmitterDiagnostic.EmitExceptionEventName)]
        public void OnEmitException(Event @event, Exception exception)
        {
            _logger.LogError(EventEmitterDiagnostic.EmitExceptionEventId, exception,
                $"Event '{@event.GetType().Name}' emit failed. Correlation Id: {@event.CorrelationId}");
        }

        [DiagnosticName(EventEmitterDiagnostic.FatalException)]
        public void OnFatalException(Exception exception)
        {
            _logger.LogError(EventEmitterDiagnostic.FatalExceptionEventId, exception,
                "Event emitter fatal exception");
        }
    }
}