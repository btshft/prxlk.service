using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Prxlk.Application.Features.ProxyReturn;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Application.Shared.Options;
using Prxlk.ComponentRegistrar;
using Prxlk.Gateway.BackgroundServices;
using Prxlk.Gateway.Features;
using Serilog;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Prxlk.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment Environment { get; set; }
        
        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            
            services
                .AddOptions()
                .Configure<ServiceOptions>(o =>
                {
                    Configuration.GetSection("Settings").Bind(o);
                    o.MongoDbConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                    o.MongoDbDatabaseName = Configuration.GetSection("MongoDb:Database").Value;    
                });
 
            // Bg services
            services.AddSingleton<EventEmitService>();
            services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<EventEmitService>());
 
            // Mediatr
            services.AddMediatR(typeof(GetProxiesQueryHandler));
            services.AddScoped<IRequestHandler<EventEmitterStatisticsRequest, EventEmitterStatistics>>(
                sp => sp.GetRequiredService<EventEmitService>());
            
            // Core components
            services.AddSingleton(typeof(IScopedServiceFactory<>), typeof(ScopedServiceFactory<>));     
            ApplicationRegistrar.ConfigureServices(services);
            
            // Features
            services.AddFeatures(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Features
            app.UseFeatures(Configuration);
        }
    }
}