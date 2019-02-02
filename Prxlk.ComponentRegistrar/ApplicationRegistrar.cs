using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prxlk.Application.Features.ProxyParse;
using Prxlk.Application.Features.ProxyParse.Strategies;
using Prxlk.Application.Features.ProxyReturn;
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

            // Covariant registration not supported in MC DI :(
            services.AddTransient(typeof(INotificationHandler<ProxyParseRequested>), typeof(ProxyParseCoordinator));
            services.AddTransient(typeof(INotificationHandler<ProxyParsed>), typeof(ProxyParseCoordinator));
            services.AddTransient(typeof(INotificationHandler<ProxyParseFailed>), typeof(ProxyParseCoordinator));
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