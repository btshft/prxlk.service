using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prxlk.Application.Features.ProxyParse;
using Prxlk.Application.Features.ProxyParse.Strategies;
using Prxlk.Application.Features.ProxyReturn;
using Prxlk.Application.Shared.Behaviors;
using Prxlk.Application.Shared.Options;
using Prxlk.Data.MongoDb;
using Prxlk.Domain.DataAccess;

namespace Prxlk.ComponentRegistrar
{
    public static class ApplicationRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMongo();
            
            services.AddAutoMapper(o =>
            {
                o.AddProfiles(typeof(ProxyParseMappingProfile).Assembly);
            });

            services.AddMediatR(typeof(GetProxiesQueryHandler));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        }

        public static void AddMongo(this IServiceCollection services)
        {
            services.AddSingleton(p =>
            {
                var options = p.GetRequiredService<IOptions<ServiceOptions>>();
                return new MongoInternalsProvider(options.Value.MongoDbConnectionString,
                    options.Value.MongoDbDatabaseName);
            });

            services.AddSingleton<IMongoClientProvider>(p => p.GetRequiredService<MongoInternalsProvider>());
            services.AddSingleton<IMongoDatabaseProvider>(p => p.GetRequiredService<MongoInternalsProvider>());
            services.AddScoped<IDataSessionFactory, MongoDataSessionFactory>();
            
            services.AddScoped(typeof(IQueryRepository<>), typeof(MongoRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

            services.AddSingleton<IProxyParseStrategyProvider, ProxyParseStrategyProvider>();
            services.AddSingleton<IProxyParseStrategy, SslProxyParseStrategy>();
        }
    }
}