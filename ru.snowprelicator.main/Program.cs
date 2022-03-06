using Microsoft.EntityFrameworkCore.Scaffolding;
using ru.snowprelicator.populate_database;
using ru.snowprelicator.scafolding;

namespace ru.snowprelicator.main
{
    internal class Program
    {
        public static readonly string DB_CONNECTION_STRING = "Server=127.0.0.1;Port=5432;Database=visual_studio;Username=portal;Password=troP4444";

        static void Main(string[] args)
        {
            PopulateDataBase.PopulateDatabase(DB_CONNECTION_STRING);
            IReverseEngineerScaffolder iReverseEngineerScaffolder = Scafolding.CreateScaffolder();
        }
    }
}