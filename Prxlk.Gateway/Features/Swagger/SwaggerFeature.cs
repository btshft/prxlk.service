using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Prxlk.Gateway.Features.Swagger
{
    [GatewayFeature("Swagger")]
    public class SwaggerFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services)
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
        public override void UseFeature(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}