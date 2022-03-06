using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;

namespace ru.snowprelicator.scafolding
{
    internal class ScaffolderService : IScaffolderService
    {
        public IReverseEngineerScaffolder CreateScaffolder()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            IReverseEngineerScaffolder iReverseEngineerScaffolder = serviceCollection.BuildServiceProvider().GetRequiredService<IReverseEngineerScaffolder>();
            return iReverseEngineerScaffolder;
        }
    }
}

