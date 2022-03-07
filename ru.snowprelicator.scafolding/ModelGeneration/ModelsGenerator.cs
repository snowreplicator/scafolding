using Microsoft.EntityFrameworkCore.Scaffolding;

namespace ru.snowprelicator.scafolding.ModelGeneration
{
    public class ModelsGenerator : IModelsGenerator
    {
        private readonly GeneratorOptions generatorOptions;

        public ModelsGenerator(GeneratorOptions generatorOptions)
        {
            this.generatorOptions = generatorOptions;
        }

        public ScaffoldedModel GenerateScaffoldedModel()
        {
            var scaffolder = generatorOptions.Scaffolder.CreateScaffolder();

            DatabaseModelFactoryOptions databaseModelFactoryOptions = new DatabaseModelFactoryOptions(schemas: generatorOptions.SchemasList?.Count > 0 ? generatorOptions.SchemasList : null);

            ModelReverseEngineerOptions modelReverseEngineerOptions = new ModelReverseEngineerOptions()
            {
                NoPluralize = false,
            };

            ModelCodeGenerationOptions modelCodeGenerationOptions = new ModelCodeGenerationOptions()
            {
                RootNamespace = generatorOptions.RootNamespace,
                ContextName = generatorOptions.ContextName,
                ContextNamespace = generatorOptions.ContextNamespace,
                ModelNamespace = generatorOptions.ModelNamespace,
                UseDataAnnotations = generatorOptions.GenerateWithAnnotations,

                SuppressConnectionStringWarning = true
            };

            ScaffoldedModel scaffoldedModel = scaffolder.ScaffoldModel(generatorOptions.ConnectionString, databaseModelFactoryOptions, modelReverseEngineerOptions, modelCodeGenerationOptions);

            string contextFile = scaffoldedModel.ContextFile.Code.Replace("Microsoft.EntityFrameworkCore.Metadata", "Npgsql.EntityFrameworkCore.PostgreSQL.Metadata");
            scaffoldedModel.ContextFile.Code = contextFile;

            return scaffoldedModel;
        }

    }
}
