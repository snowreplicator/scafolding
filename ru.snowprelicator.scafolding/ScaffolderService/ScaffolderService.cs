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
    public class ScaffolderService : IScaffolderService
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


    // при переходе на последнюю версию
    //           <PackageReference Include="Bricelam.EntityFrameworkCore.Pluralizer" Version="1.0.0" />
    //           <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="6.0.2" />
    //           <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="6.0.3" />
    //           <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
    // будет идти ошибка
    //           Unhandled exception. System.InvalidOperationException: Unable to resolve service for type 'Microsoft.EntityFrameworkCore.Scaffolding.ProviderCodeGeneratorDependencies' while attempting to activate 'Npgsql.EntityFrameworkCore.PostgreSQL.Scaffolding.Internal.NpgsqlCodeGenerator'.
    // ее придется как то решать 
    // ниже предварительные данные
    // https://github.com/npgsql/efcore.pg/issues/2113
    // var services = new ServiceCollection()
    //     .AddEntityFrameworkDesignTimeServices()
    //     .AddDbContextDesignTimeServices(ctx);
    // new NpgsqlDesignTimeServices().ConfigureDesignTimeServices(services);
    // 
    // var serviceProvider = services.BuildServiceProvider();
    // 
    // var scaffolder = serviceProvider.GetRequiredService<IReverseEngineerScaffolder>();
    
}
