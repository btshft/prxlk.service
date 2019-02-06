using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Prxlk.Gateway.Features.Diagnostics;
using Prxlk.Shared.Expressions;

namespace Prxlk.Gateway.Features.ScheduledEventEmit.Emitters
{
    public abstract class ScheduledEmitter : IDisposable
    {
        protected Timer Timer { get; }
        protected ScheduledEmitterContext Context { get; }
        protected DiagnosticSource DiagnosticSource { get; }
        
        protected ScheduledEmitter(TimeSpan initialDelay, TimeSpan interval, CancellationToken cancellation, DiagnosticSource diagnosticSource)
        {
            DiagnosticSource = diagnosticSource;
            Context = new ScheduledEmitterContext(StringComparer.InvariantCultureIgnoreCase)
            {
                ["interval"] = initialDelay,
                ["success_emits"] = 0,
                ["failed_emits"] = 0,
                ["last_emit"] = DateTime.UtcNow
            };
            
            Timer = TimerFactory.CreateTimer(async __ =>
            {
                try
                {
                    await ExecuteAsync(cancellation);
                    Context.Update<int>("success_emits", i => i + 1);
                }
                catch (Exception e)
                {
                    if (DiagnosticSource.IsEnabled(EventEmitterDiagnostic.EmitExceptionEventName))
                        DiagnosticSource.Write(EventEmitterDiagnostic.EmitExceptionEventName, new
                        {
                            exception = e
                        });
                    
                    Context.Update<int>("failed_emits", i => i + 1);
                }
                finally
                {
                    Context.Update<DateTime>("last_emit", _ => DateTime.UtcNow);
                }
                
            }, interval, initialDelay);
        }

        public IReadOnlyDictionary<string, object> GetContext() => Context;

        public abstract string GetName();
        
        protected abstract Task ExecuteAsync(CancellationToken cancellation);

        /// <inheritdoc />
        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}