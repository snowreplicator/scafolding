using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ru.snowprelicator.populate_database
{
    public class PopulateDataBase
    {
        public static void PopulateDatabase(string dbConnectionString)
        {
            Console.WriteLine("\n--- populating database: {0}", dbConnectionString);
            ApplicationContext applicationContext = new ApplicationContext(dbConnectionString);
            PopulateDatabase(applicationContext);
            PrintHorsesTableContent(applicationContext);
            applicationContext.Dispose();
            Console.WriteLine("\n---\n");
        }

        private static List<Horse> GetAllHorses(ApplicationContext applicationContext)
        {
            return applicationContext.HorseTable.ToList();
        }

        private static void PopulateDatabase(ApplicationContext applicationContext)
        {
            List<Horse> horses = GetAllHorses(applicationContext);
            if (horses.Count < 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Horse horse = new Horse()
                    {
                        Name = "horse number " + (i + 1),
                        Weight = (i + 1) + ((i + 1) / 10.0),
                        BirthDay = DateTime.Now
                    };
                    applicationContext.HorseTable.Add(horse);
                }
                applicationContext.SaveChanges();
            }
        }

        private static void PrintHorsesTableContent(ApplicationContext applicationContext)
        {
            List<Horse> horses = GetAllHorses(applicationContext);

            Console.WriteLine("horse table content:");
            if (horses.Count < 1)
            {
                Console.WriteLine("no items");
            }

            foreach (Horse horse in horses)
            {
                Console.WriteLine(JsonConvert.SerializeObject(horse));
            }
        }
    }
}