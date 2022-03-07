using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ru.snowprelicator.data_manipulation
{
    public class DataManipulation
    {
        private static readonly string HORSE_TABLE = "Horse";

        public static void ManipulateWithData(DbContext applicationContext)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("different operations on data\n");

            // список всех таблиц в бд
            PrintAllApplicationContextTables(applicationContext.Model.GetEntityTypes());

            // получение типа таблицы horse
            IEntityType horseIEntityType = GetEntityTypeByTableName(applicationContext, HORSE_TABLE);

            // вывод содержимого таблицы horse
            PrintTableContent(applicationContext, horseIEntityType);

            // вставка новой записи в таблицу (если нет)

            // поиск записи
            int horseId = 1111;
            IEntityType horseWithId_1111_IEntityType = FindEntityById(applicationContext, horseIEntityType, horseId);
            Console.WriteLine("\nfind horse with id: {0} = {1}", horseId, horseWithId_1111_IEntityType == null ? "not finded" : JsonConvert.SerializeObject(horseWithId_1111_IEntityType));

            // если нет лошади с индексом 1111 - то делаем вставку в таблицу
            if (horseWithId_1111_IEntityType == null)
            {
                if (InsertHorse(applicationContext, horseIEntityType))
                {
                    applicationContext.SaveChanges();
                }
            }

            // поиск первой записи из таблицы и вывод ее параметров
            Console.WriteLine("\n\nall params of horse object: ");
            object firstHorse = FindFirstTableEntity(applicationContext, horseIEntityType);
            DynamicContextExtensions.PrintAllMethods(firstHorse);

            Console.WriteLine("\n\nall params of horse item: ");
            DynamicContextExtensions.PrintAllMethods(horseIEntityType);
        }

        // список таблиц в бд
        private static void PrintAllApplicationContextTables(IEnumerable<IEntityType> types)
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

        // получение типа таблицы по ее известному имени
        private static IEntityType GetEntityTypeByTableName(DbContext applicationContext, string name)
        {
            IEnumerable<IEntityType> entityTypes = applicationContext.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                if (entityType.Name.IndexOf(name) != -1)
                {
                    return entityType;
                }
            }
            throw new Exception("Entity type " + name + " is not found");
        }

        // вывод содержимого указанной таблицы
        private static void PrintTableContent(DbContext applicationContext, IEntityType iEntityType)
        {
            Console.WriteLine("\n\n content of talbe: {0}", iEntityType.Name);
            IQueryable<object> items = (IQueryable<object>)applicationContext.QueryAsFunc(iEntityType.Name);
            if (items.Count() <= 0)
            {
                Console.WriteLine("table is empty");
                return;
            }
            foreach (var item in items)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }
        }

        // поиск записи по horseid
        private static IEntityType FindEntityById(DbContext applicationContext, IEntityType iEntityType, int horseId)
        {
            Type TypeOfHorseEntity = applicationContext.Model.FindEntityType(iEntityType.Name).ClrType;

            var finedEntity = (IEntityType)DynamicContextExtensions.FindEntity(applicationContext, TypeOfHorseEntity, horseId);
            return finedEntity;
        }

        // получение первой записи из таблицы
        private static object FindFirstTableEntity(DbContext applicationContext, IEntityType iEntityType)
        {
            IQueryable<object> items = (IQueryable<object>) applicationContext.QueryAsFunc(iEntityType.Name);
            if (items.Count() > 0)
            {
                return items.First();
            }
            return null;
        }

        // вставка в таблицу horse записи с индексом 1111
        private static bool InsertHorse(DbContext applicationContext, IEntityType horseIEntityType)
        {
            Type horseEntityType = applicationContext.Model.FindEntityType(horseIEntityType.Name).ClrType;
            object newHorse = Activator.CreateInstance(horseEntityType);
            newHorse.SetPropertyValue("Name", "insert animal number xxxx");
            newHorse.SetPropertyValue("Weight", 1111.1111);
            newHorse.SetPropertyValue("BirthDay", DateTime.Now);

            bool res = DynamicContextExtensions.InsertEntity(applicationContext, newHorse);
            return res;
        }

    }
}