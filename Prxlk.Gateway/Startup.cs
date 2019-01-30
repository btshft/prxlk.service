using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prxlk.Application.Shared.Options;
using Prxlk.ComponentRegistrar;
using Prxlk.Gateway.BackgroundServices;
using Swashbuckle.AspNetCore.Swagger;

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
            services
                .AddOptions()
                .Configure<ProxyCoreOptions>(o =>
                {
                    Configuration.GetSection("Core").Bind(o);
                    o.MssqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
                    o.MongoDbConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                    o.MongoDbDatabaseName = Configuration.GetSection("MongoDb:Database").Value;    
                });
            
            services.AddMvcCore(o =>
                {
                    o.OutputFormatters.RemoveType<XmlDataContractSerializerOutputFormatter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonFormatters()
                .AddApiExplorer();

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
            {
                app.UseDeveloperExceptionPage();
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