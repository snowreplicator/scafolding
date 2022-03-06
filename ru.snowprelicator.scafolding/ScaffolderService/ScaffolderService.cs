using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Diagnostics.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Scaffolding.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal;

namespace ru.snowprelicator.scafolding
{
    internal class ScaffolderService : IScaffolderService
    {
        public IReverseEngineerScaffolder CreateScaffolder()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddEntityFrameworkNpgsql();
            serviceCollection.AddLogging();
            serviceCollection.AddEntityFrameworkDesignTimeServices();
            serviceCollection.AddSingleton<IAnnotationCodeGenerator, AnnotationCodeGenerator>();
            serviceCollection.AddSingleton<IPluralizer, Bricelam.EntityFrameworkCore.Design.Pluralizer>();
#pragma warning disable EF1001 // Internal EF Core API usage.
            serviceCollection.AddSingleton<LoggingDefinitions, NpgsqlLoggingDefinitions>();
            serviceCollection.AddSingleton<IRelationalTypeMappingSource, NpgsqlTypeMappingSource>();
            serviceCollection.AddSingleton<IDatabaseModelFactory, NpgsqlDatabaseModelFactory>();
            serviceCollection.AddSingleton<IProviderConfigurationCodeGenerator, NpgsqlCodeGenerator>();
            serviceCollection.AddSingleton<IScaffoldingModelFactory, RelationalScaffoldingModelFactory>();
#pragma warning restore EF1001 // Internal EF Core API usage.

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            IReverseEngineerScaffolder iReverseEngineerScaffolder = serviceProvider.GetRequiredService<IReverseEngineerScaffolder>();
            return iReverseEngineerScaffolder;
        }
    }
}

