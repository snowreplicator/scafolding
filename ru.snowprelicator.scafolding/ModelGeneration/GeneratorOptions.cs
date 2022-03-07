using System.Collections.Generic;

namespace ru.snowprelicator.scafolding.ModelGeneration
{
    public class GeneratorOptions
    {
        // пока не найдено где используется
        public string RootNamespace { get; set; } = "ru.snowreplicator.scafolder.rootnamespace";

        // уходит в генерацию кода по контексту (класс расширяющий DbContext): public partial class ApplicationContext : DbContext
        public string ContextName { get; set; } = "ApplicationContext";

        // уходит в генерацию кода (namespace) для dbConext: namespace ru.snowreplicator.scafolder.contextnamespace
        public string ContextNamespace { get; set; } = "ru.snowreplicator.scafolder.contextnamespace";

        // уходит в генерацию кода (namespace) по моделям: namespace ru.snowreplicator.scafolder.modelnamespace
        public string ModelNamespace { get; set; } = "ru.snowreplicator.scafolder.modelnamespace";

        // влияет на генерацию аннотаций в моделях
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

        // видимо эта функция дает полное имя созданного ApplicationContext
        public string GetContextFullName()
        {
            return $"{ContextNamespace}.{ContextName}";
        }
        
    }
}
