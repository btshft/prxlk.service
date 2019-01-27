using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prxlk.Application.Options;
using Prxlk.ComponentRegistrar;
using Prxlk.Gateway.BackgroundServices;
using Swashbuckle.AspNetCore.Swagger;

namespace Prxlk.Gateway
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment environment)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true);

            if (environment.IsProduction())
                configurationBuilder.AddUserSecrets<Startup>();

            configurationBuilder.AddEnvironmentVariables();
            
            Configuration = configurationBuilder.Build();
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<ProxyCoreOptions>(o =>
                {
                    Configuration.GetSection("Core").Bind(o);
                    o.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                });
            
            services.AddMvcCore(o =>
                {
                    o.OutputFormatters.RemoveType<XmlDataContractSerializerOutputFormatter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonFormatters()
                .AddApiExplorer();

            if (Environment.IsDevelopment())
            {
                services.AddLogging(o => o.SetMinimumLevel(LogLevel.Debug)
                    .AddDebug());
            }
            else
            {
                services.AddLogging(o => o.SetMinimumLevel(LogLevel.Warning));
            }

            services.AddHealthChecks();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
            });
            
            services.AddSwaggerGen(o => 
            {
                o.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Proxy Lake"
                });
            });

            // Bg services
            services.AddHostedService<ProxyBackgroundService>();
            
            ApplicationRegistrar.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseMvc();
            app.UseHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}