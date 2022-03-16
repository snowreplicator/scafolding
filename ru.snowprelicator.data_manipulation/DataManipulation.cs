using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ru.snowprelicator.data_manipulation
{
    public class DataManipulation
    {
        private static readonly string HORSE_TABLE = "Horse";
        private static readonly string LESSON_SOURCE = "LessonSource";

        public static void ManipulateWithData(DbContext applicationContext)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("different operations on data\n");


            Type classifierSetType = applicationContext.Model.FindEntityType("ru.snowreplicator.scafolder.modelnamespace.Classifierset").ClrType;


            var dbSet = applicationContext.GetDbSet(classifierSetType);
            //DbSet<object> dbSet = (DbSet<object>) applicationContext.GetDbSet(classifierSetType);
            //MethodInfo methodRemove = dbSet.GetType().GetMethod("Select");

            //DynamicContextExtensions.PrintAllMethods(dbSet);

            //Enumerable.Select(dbSet, x => x.Name == "");

            var sss = typeof(Enumerable).GetMethods();
            MethodInfo methodInfo20 = null;
            foreach (var item in sss)
            {
                Console.WriteLine("  name " + item.Name);
                if (item.Name.IndexOf("Select") != -1)
                {

                    methodInfo20 = item;
                    break;

                    ParameterInfo[] parameters = item.GetParameters();
                    // Выводим некоторые характеристики каждого из параметров. 
                    foreach (ParameterInfo parameter in parameters)
                    {

                        //Console.WriteLine("Имя параметра: {0}", parameter.Name);
                        //Console.WriteLine("Позиция в методе: {0}", parameter.Position);
                        //Console.WriteLine("Тип параметра: {0}", parameter.ParameterType);
                    }
                }

            }

            // { System.Collections.Generic.IEnumerable`1[TResult] Select[TSource, TResult]
            // (System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource, TResult])}
            //MethodInfo methodRemove = typeof(Enumerable).GetMethod("Select", new Type[] { typeof(IEnumerable<>), typeof(Func<,>) });
            //MethodInfo methodRemove = typeof(Enumerable).GetMethod("Select", new Type[] { typeof( IEnumerable<>), typeof(Func<,>) });
            
            MethodInfo methodSelect = typeof(Enumerable).GetMethod("Select", new Type[] { typeof(IEnumerable<>), typeof(Func<,>) });

            MethodInfo methodAverage = typeof(Enumerable).GetMethod("Average", new Type[] { typeof(IEnumerable<int>) });
            var res1 = methodAverage.Invoke(null, new object[] { new int[] { 11, 22, 33 } });

            //this IEnumerable<TSource> source, Func< TSource, TResult > selector
            //var res2 = methodInfo20.Invoke(null, new object[] { dbSet, "x => x" });

            //Func<bool, object> square = x => x.GetPropertyValue("Name") == "";
            //var res2 = methodInfo20.Invoke(null, new object[] { dbSet, square });

            /*
            var sssss = from c in (DbSet<object>) dbSet
                        where c.GetPropertyValue("Name") == ""
                        select c;
            */

            /*
            var query = from e in db.Employments
                        join rf in db.RoleFuncs on e.rolefuncid equals rf.RoleFuncId
                        where rf.RoleFuncName.ToUpper().Contains(data.name.ToUpper())
                        select e;
            result.employments = query.ToList();
            */

            /*
            Type t = Type.GetType("ru.snowreplicator.scafolder.modelnamespace.Classifierset");
            var type = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(t => t.Name == tableName);*/

            /*
            IEntityType horseIEntityType = GetEntityTypeByTableName(applicationContext, HORSE_TABLE);
            Type horseEntityType = applicationContext.Model.FindEntityType(horseIEntityType.Name).ClrType;

            DbSet<object> dbset2 = applicationContext.Set(horseIEntityType);*/

            //DbSet<Horse> sss;

            //applicationContext.Model.GetEntityTypes.Select(e => e.Name);


            IEntityType horseIEntityType = GetEntityTypeByTableName(applicationContext, LESSON_SOURCE);
            Type TypeOfHorseEntity = applicationContext.Model.FindEntityType(horseIEntityType.Name).ClrType;

            /*
            FieldInfo[] fields = TypeOfHorseEntity.GetFields();
            foreach (var field in fields)
            {
                string name = field.Name;
                object temp = field.GetValue(null);
                // See if it is an integer or string.
                if (temp is int)
                {
                    int value = (int)temp;
                    Console.Write(name);
                    Console.Write(" (int) = ");
                    Console.WriteLine(value);
                }
                else if (temp is string)
                {
                    string value = temp as string;
                    Console.Write(name);
                    Console.Write(" (string) = ");
                    Console.WriteLine(value);
                }
            }*/



            Console.WriteLine("\n\n");






            /*
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
            */
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
            IQueryable<object> items = (IQueryable<object>)applicationContext.QueryAsFunc(iEntityType.Name);
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