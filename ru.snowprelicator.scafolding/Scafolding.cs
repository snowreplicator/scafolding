using Microsoft.EntityFrameworkCore.Scaffolding;
using ru.snowprelicator.scafolding.ModelGeneration;
using System;
using System.Collections.Generic;

namespace ru.snowprelicator.scafolding
{
    public class Scafolding
    {
        public static ScaffoldedModel CreateScaffolder(string dbConnectionString, List<string> dbSchemas)
        {
            Console.WriteLine("\n--- screating scaffolding:\n");
            GeneratorOptions generatorOptions = GeneratorOptions.GetDefaultOptions(dbConnectionString, dbSchemas);

            ScaffolderService scaffolderService = new ScaffolderService();
            generatorOptions.Scaffolder = scaffolderService;

            IModelsGenerator modelsGenerator = new ModelsGenerator(generatorOptions);
            ScaffoldedModel scaffoldedModel = modelsGenerator.GenerateScaffoldedModel();

            // print scaffolding result
            Console.WriteLine("\nscaffoldedModel models context:\n\n{0}", scaffoldedModel.ContextFile.Code);
            Console.WriteLine("\nscaffoldedModel models (count: {0}):\n", scaffoldedModel.AdditionalFiles.Count);
            foreach(var model in scaffoldedModel.AdditionalFiles)
            {
                Console.WriteLine(model.Code);
            }
            Console.WriteLine("\n---\n");
            return scaffoldedModel;
        }
    }
}