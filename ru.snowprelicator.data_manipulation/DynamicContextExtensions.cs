using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ru.snowprelicator.data_manipulation
{
    public static class DynamicContextExtensions
    {
        public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        public static IQueryable QueryAsFunc(this DbContext context, string entityName)
        {
            Type tableType = context.Model.FindEntityType(entityName).ClrType;
            IQueryable<object> modelItems = (IQueryable<object>)DynamicContextExtensions.QueryAsFunc(context, tableType);
            return modelItems;
        }

        

        static readonly MethodInfo SetMethod =
            typeof(DbContext).GetMethod(nameof(DbContext.Set), 1, Array.Empty<Type>()) ??
            throw new Exception($"Type not found: DbContext.Set");

        public static MethodInfo SetMethodAsFunc(Type entityType)
        {
            MethodInfo methodInfo = typeof(DbContext).GetMethod(nameof(DbContext.Set), 1, Array.Empty<Type>()) ?? throw new Exception($"Type not found: DbContext.Set");
            methodInfo = methodInfo.MakeGenericMethod(entityType);
            return methodInfo;
        }

        

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType)?.Invoke(context, null) ??
            throw new Exception($"Type not found: {entityType.FullName}");
        
        public static IQueryable QueryAsFunc(this DbContext context, Type entityType)
        {
            MethodInfo methodInfo = DynamicContextExtensions.SetMethodAsFunc(entityType);
            if (methodInfo == null) throw new Exception($"Type not found: {entityType.FullName}");
            IQueryable<object> modelItems = (IQueryable<object>)methodInfo.Invoke(context, null);
            return modelItems;
        }



        public static string NameOfDbSet(this DbContext context, Type entityType)
        {
            Type generic = typeof(DbSet<>);
            Type constructed = generic.MakeGenericType(entityType);
            var prop = context.GetType().GetProperties().Where(_ => _.PropertyType.Name == constructed.Name && _.Name.StartsWith(entityType.Name))?.FirstOrDefault();

            return prop.Name;
        }



        public static Object GetDbSet(this DbContext context, Type entityType)
        {
            Type generic = typeof(DbSet<>);
            Type constructed = generic.MakeGenericType(entityType);
            var prop = context.GetType().GetProperties().Where(_ => _.PropertyType.Name == constructed.Name && _.Name.StartsWith(entityType.Name))?.FirstOrDefault();
            var dbSet = prop?.GetValue(context);
            return dbSet;
        }

        // удаление записи из бд
        public static bool DeleteEntity(this DbContext context, Object entity)
        {
            Type entityType = entity.GetType();
            var dbSet = GetDbSet(context, entityType);

            MethodInfo methodRemove = dbSet.GetType().GetMethod("Remove");
            var res = methodRemove.Invoke(dbSet, new object[] { entity });
            return res != null;
        }

        // поиск записи в бд
        public static Object FindEntity(this DbContext context, Type entityType, params object[] keyValues)
        {
            var dbSet = GetDbSet(context, entityType);

            MethodInfo methodAdd = dbSet.GetType().GetMethod("Find");
            return methodAdd.Invoke(dbSet, new object[] { keyValues });
        }

        // вставка новой записи
        public static bool InsertEntity(this DbContext context, Object entity)
        {
            Type entityType = entity.GetType();
            var dbSet = GetDbSet(context, entityType);
            MethodInfo methodAdd = dbSet.GetType().GetMethod("Add");
            var res = methodAdd.Invoke(dbSet, new object[] { entity });
            return res != null;
        }

        // вывод всех параметром объекта
        public static void PrintAllMethods(Object entity)
        {
            Type entityType = entity.GetType();

            // Получаем информацию об именах всех методов:
            Console.WriteLine("\n всех методы:");
            MethodInfo[] mi = entityType.GetMethods(BindingFlags.Instance
                   | BindingFlags.Static
                   | BindingFlags.Public
                   | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in mi) Console.WriteLine(item.Name);




            // Получаем информацию об именах полей класса.
            Console.WriteLine("\n всех поля:");
            FieldInfo[] fi = entityType.GetFields(BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.Public
                    | BindingFlags.NonPublic);
            foreach (FieldInfo item in fi) Console.WriteLine(item.Name);

            // Получаем список всех свойств класса:
            Console.WriteLine("\n всех свойства:");
            PropertyInfo[] pi = entityType.GetProperties();
            foreach (PropertyInfo item in pi) Console.WriteLine(item.Name);

            //Получаем список всех интерфейсов классов:
            Console.WriteLine("\n все интерфейсы:");
            Type[] it = entityType.GetInterfaces();
            foreach (Type item in it) Console.WriteLine(item.Name);

            // Получаем информацию обо всех конструкторах:
            Console.WriteLine("\n все конструкторы:");
            ConstructorInfo[] ci = entityType.GetConstructors();
            foreach (ConstructorInfo item in ci) Console.WriteLine(item.Name);

            // Получаем информацию обо всех конструкторах:
            Console.WriteLine("\n runtime:");
            IEnumerable<MethodInfo> rm = entityType.GetRuntimeMethods();
            foreach (MethodInfo item in rm) Console.WriteLine(item.Name);



        }
    }
}
