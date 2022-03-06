using Microsoft.EntityFrameworkCore.Scaffolding;

namespace ru.snowprelicator.scafolding
{
    public interface IScaffolderService
    {
        IReverseEngineerScaffolder CreateScaffolder();
    }
}
