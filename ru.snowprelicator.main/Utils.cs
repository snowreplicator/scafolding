using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ru.snowprelicator.main
{
    public class Utils
    {
        public static void PrintAllApplicationContextTables(IEnumerable<IEntityType> types)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("all tables (models) names in db:");
            List<string> typeNames = new List<string>();
            typeNames.AddRange(types.Select(e => e.Name));
            foreach (var name in typeNames)
            {
                Console.WriteLine("type name: {0}:", name);
            }
            Console.WriteLine("\n\n");
        }
    }
}
