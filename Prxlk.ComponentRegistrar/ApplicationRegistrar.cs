﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prxlk.Application.MappingProfiles;
using Prxlk.Application.Options;
using Prxlk.Application.ParseStrategies;
using Prxlk.Application.Services;
using Prxlk.Data.EntityFramework;
using Prxlk.Domain.Behaviors;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.QueryHandlers;

namespace Prxlk.ComponentRegistrar
{
    public static class ApplicationRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ProxyDbContext>(p =>
            {
                var options = p.GetRequiredService<IOptions<ProxyCoreOptions>>();
                return new ProxyDbContext(options.Value.ConnectionString);
            });
            
            services.AddScoped(typeof(IQueryRepository<>), typeof(EntityFrameworkRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
            services.AddScoped<IDataSessionFactory, EntityFrameworkDataSessionFactory>();
            
            services.AddAutoMapper(o =>
            {
                o.AddProfile<DomainContractsProfile>();
            });

            services.AddMediatR(typeof(ProxyQueriesHandler));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            
            services.AddScoped<IProxyService, ProxyService>();
            services.AddSingleton<IExternalProxyProviderFactory, ExternalProxyProviderFactory>();
        }
    }
}