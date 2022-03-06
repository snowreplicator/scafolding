using Microsoft.EntityFrameworkCore.Scaffolding;

namespace ru.snowprelicator.scafolding
{
    public class Scafolding
    {
        public static IReverseEngineerScaffolder CreateScaffolder()
        {
            ScaffolderService scaffolderService = new ScaffolderService();
            return scaffolderService.CreateScaffolder();
        }
    }
}