using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Application.Shared.Options;
using Prxlk.ComponentRegistrar;
using Prxlk.Gateway.BackgroundServices;
using Prxlk.Gateway.Features;
using Serilog;

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
 
            // Features
            services.AddFeatures(Configuration);
            
            // Bg services
            services.AddHostedService<ProxyParseEventEmitter>();
            
            //
            services.AddSingleton(typeof(IScopedServiceFactory<>), typeof(ScopedServiceFactory<>));
            
            ApplicationRegistrar.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Features
            app.UseFeatures();
        }
    }
}