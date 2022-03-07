using System.Collections.Generic;

namespace ru.snowprelicator.scafolding.ModelGeneration
{
    public class GeneratorOptions
    {
        public string RootNamespace { get; set; } = "InteractiveStore";

        public string ContextName { get; set; } = "InteractiveStoreContext";

        public string ContextNamespace { get; set; } = "InteractiveStore.Context";

        public string ModelNamespace { get; set; } = "InteractiveStore.Models";

        public bool GenerateWithAnnotations { get; set; } = true;

        public string ConnectionString { get; set; }

        public List<string> TablesList { get; private set; }

        public List<string> SchemasList { get; private set; }

        public IScaffolderService Scaffolder { get; set; }

        private GeneratorOptions()
        {
            TablesList = new List<string>();
            SchemasList = new List<string>();
        }

        public static GeneratorOptions GetDefaultOptions(string connectionString, List<string> schemas)
        {
            var options = new GeneratorOptions()
            {
                ConnectionString = connectionString,
                SchemasList = schemas
            };
            return options;
        }

        public string GetContextFullName()
        {
            return $"{ContextNamespace}.{ContextName}";
        }
        
    }
}
