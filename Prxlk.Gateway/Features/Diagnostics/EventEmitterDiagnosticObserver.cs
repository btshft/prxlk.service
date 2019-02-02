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

        [DiagnosticName(EventEmitterDiagnostic.ExceptionEventName)]
        public void OnException(Event @event, Exception exception)
        {
            _logger.LogError(exception, $"Event '{@event.GetType().Name}' emit failed. Correlation Id: {@event.CorrelationId}");
        }
    }
}