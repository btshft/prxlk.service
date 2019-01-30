using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prxlk.Application.Shared.Options;
using Prxlk.Contracts;

namespace Prxlk.Gateway.BackgroundServices
{
    public class ProxyBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly List<Timer> _refreshProxyTimers;
        private readonly ProxyCoreOptions _options;

        public ProxyBackgroundService(
            IOptions<ProxyCoreOptions> options, 
            ILogger<ProxyBackgroundService> logger)
        {
            _options = options.Value;
            _logger = logger;
            _refreshProxyTimers = new List<Timer>();
        }
        
        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh service is starting");
            
            StartTimers();
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh service is stopping");
            
            StopTimers();
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var timer in _refreshProxyTimers)
            {
                try
                {
                    timer.Dispose();
                } catch(Exception e){ }
            }
        }

        private void StartTimers()
        {
            foreach (var proxySource in Enum.GetValues(typeof(ProxySource)).Cast<ProxySource>())
            {
                if (proxySource == ProxySource.Undefined)
                    continue;

                var interval = _options.Intervals.FirstOrDefault(i =>
                    string.Equals(i.Name, proxySource.ToString(), StringComparison.InvariantCultureIgnoreCase));

                if (interval == null) 
                    continue;
                
                var currentProxySource = proxySource;
                
                _refreshProxyTimers.Add(CreateTimer(_ =>
                {
                    // TODO

                }, interval.Interval));
            }
        }

        private void StopTimers()
        {
            foreach (var timer in _refreshProxyTimers)
            {
                timer.Change(Timeout.Infinite, 0);
            }
        }
        
        private static Timer CreateTimer(TimerCallback callback, TimeSpan period)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            // Don't capture the current ExecutionContext and its AsyncLocals onto the timer
            var restoreFlow = false;
            try
            {
                if (ExecutionContext.IsFlowSuppressed()) 
                    return new Timer(callback, null, TimeSpan.Zero, period);
                
                ExecutionContext.SuppressFlow();
                restoreFlow = true;

                return new Timer(callback, null, TimeSpan.Zero, period);
            }
            finally
            {
                // Restore the current ExecutionContext
                if (restoreFlow)
                {
                    ExecutionContext.RestoreFlow();
                }
            }
        } 
    }
}