using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prxlk.Application.Features.ProxyReturn;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Application.Shared.Options;
using Prxlk.ComponentRegistrar;
using Prxlk.Gateway.Features;
using Serilog;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Prxlk.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        
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
 
            // Mediatr
            services.AddMediatR(typeof(GetProxiesQueryHandler));

            // Core components
            services.AddSingleton(typeof(IScopedServiceFactory<>), typeof(ScopedServiceFactory<>));     
            ApplicationRegistrar.ConfigureServices(services);
            
            // Features
            services.AddFeatures(Configuration);
            
            // Api options
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Type = "about:blank",
                        Detail = "Please refer to the errors property for additional details."
                    };
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            // Features
            app.UseFeatures(Configuration);
        }
    }
}