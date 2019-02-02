using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prxlk.Gateway.Features.Diagnostics
{
    public abstract class DiagnosticObserver : IObserver<DiagnosticListener>
    {
        private readonly Dictionary<string, IDisposable> _subscriptions;

        protected DiagnosticObserver()
        {
            _subscriptions = new Dictionary<string, IDisposable>(
                StringComparer.InvariantCulture);
        }
        
        /// <inheritdoc />
        public void OnCompleted()
        {
            foreach (var subscription in _subscriptions)
                subscription.Value.Dispose();

            _subscriptions.Clear();
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        { }

        /// <inheritdoc />
        public void OnNext(DiagnosticListener listener)
        {
            if (IsMatch(listener.Name) && !_subscriptions.ContainsKey(listener.Name))
            {
                var subscription = listener.SubscribeWithAdapter(this);
                _subscriptions[listener.Name] = subscription;
            }
        }

        protected abstract bool IsMatch(string listenerName);
    }
}