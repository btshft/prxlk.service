using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Prxlk.Gateway.Features.Swagger
{
    [GatewayFeature("Swagger", order: 2)]
    public class SwaggerFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(o => 
            {
                o.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Proxy Lake"
                });
            });
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}