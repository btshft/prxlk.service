using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prxlk.Application.Features.ProxyParse;
using Prxlk.Application.Features.ProxyParse.Strategies;
using Prxlk.Application.Shared.Options;
using Prxlk.Contracts;
using Prxlk.Gateway.DependencyInjection;

namespace Prxlk.Gateway.BackgroundServices
{
    public class ProxyBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly List<Timer> _refreshProxyTimers;
        private readonly ServiceOptions _options;
        private readonly IProxyParseStrategyProvider _parseStrategyProvider;

        private readonly IScopedServiceFactory<IMediator> _mediatorFactory;

        public ProxyBackgroundService(
            IOptions<ServiceOptions> options, 
            ILogger<ProxyBackgroundService> logger, 
            IProxyParseStrategyProvider parseStrategyProvider, 
            IScopedServiceFactory<IMediator> mediatorFactory)
        {
            _options = options.Value;
            _logger = logger;
            _parseStrategyProvider = parseStrategyProvider;
            _mediatorFactory = mediatorFactory;
            _refreshProxyTimers = new List<Timer>();
        }
        
        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellation)
        {
            _logger.LogInformation("Refresh service is starting");
            
            StartTimers(cancellation);
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellation)
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

        private void StartTimers(CancellationToken cancellation)
        {
            foreach (var proxySource in Enum.GetValues(typeof(ProxySource)).Cast<ProxySource>())
            {
                if (proxySource == ProxySource.Undefined)
                    continue;

                var sourceOptions = _options.GetSource(proxySource);
                var currentProxySource = proxySource;
                
                _refreshProxyTimers.Add(CreateTimer(async _ =>
                {
                    try
                    {
                        // TODO
                        var strategy = _parseStrategyProvider.GetStrategy(currentProxySource);
                        var proxies = await strategy.ParseAsync(new ProxyParseRequest(), cancellation);

                        using (var scope = _mediatorFactory.CreateScope())
                        {
                            var mediator = scope.GetService();
                            foreach (var proxy in proxies)
                            {
                                await mediator.Send(new ProxyInsertCommand(
                                    proxy.Ip, proxy.Port, proxy.Protocol, proxy.Country), cancellation);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // Log
                    }

                }, sourceOptions.Refresh, TimeSpan.FromSeconds(1)));
            }
        }

        private void StopTimers()
        {
            foreach (var timer in _refreshProxyTimers)
            {
                timer.Change(Timeout.Infinite, 0);
            }
        }
        
        private static Timer CreateTimer(TimerCallback callback, TimeSpan period, TimeSpan delay)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            // Don't capture the current ExecutionContext and its AsyncLocals onto the timer
            var restoreFlow = false;
            try
            {
                if (ExecutionContext.IsFlowSuppressed()) 
                    return new Timer(callback, null, delay, period);
                
                ExecutionContext.SuppressFlow();
                restoreFlow = true;

                return new Timer(callback, null, delay, period);
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