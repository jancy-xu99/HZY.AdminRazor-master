using HZY.DapperCore.Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace HZY.Admin.Dapper
{
    public static class DapperFactoryCollectionExtensions
    {
        public static IServiceCollection AddDapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddLogging();
            services.AddOptions();

            services.AddSingleton<DefaultDapperFactory>();
            services.TryAddSingleton<IDapperFactory>(serviceProvider => serviceProvider.GetRequiredService<DefaultDapperFactory>());

            return services;
        }

        public static IDapperFactoryBuilder AddDapper(this IServiceCollection services, string name, Action<ConnectionConfig> configureClient)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (configureClient == null)
                throw new ArgumentNullException(nameof(configureClient));

            AddDapper(services);

            var builder = new DefaultDapperFactoryBuilder(services, name);
            builder.ConfigureDapper(configureClient);
            return builder;
        }

        public static IDapperFactoryBuilder ConfigureDapper(this IDapperFactoryBuilder builder, Action<ConnectionConfig> configureClient)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configureClient == null)
                throw new ArgumentNullException(nameof(configureClient));

            builder.Services.Configure<DapperFactoryOptions>(builder.Name, options => options.DapperActions.Add(configureClient));

            return builder;
        }
    }

    public interface IDapperFactoryBuilder
    {
        string Name { get; }

        IServiceCollection Services { get; }
    }

    internal class DefaultDapperFactoryBuilder : IDapperFactoryBuilder
    {
        public DefaultDapperFactoryBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}