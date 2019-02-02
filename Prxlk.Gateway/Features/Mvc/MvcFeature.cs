using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.Mvc
{
    [GatewayFeature("Mvc")]
    public class MvcFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services)
        {
            services.AddMvcCore(o =>
                {
                    o.OutputFormatters.RemoveType<XmlDataContractSerializerOutputFormatter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonFormatters()
                .AddApiExplorer();
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}