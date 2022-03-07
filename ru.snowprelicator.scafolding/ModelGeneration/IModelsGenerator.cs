using Microsoft.EntityFrameworkCore.Scaffolding;

namespace ru.snowprelicator.scafolding.ModelGeneration
{
    public interface IModelsGenerator
    {
        ScaffoldedModel GenerateScaffoldedModel();
    }
}
