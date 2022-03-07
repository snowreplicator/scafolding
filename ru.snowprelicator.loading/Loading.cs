using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ru.snowprelicator.loading
{
    public class Loading
    {
        public static DbContext LoadCompiledDll(MemoryStream memoryStream, bool enableLazyLoading)
        {
            AssemblyLoadContext assemblyLoadContext = new AssemblyLoadContext("InstanceOfLoaded", isCollectible: !enableLazyLoading);

            memoryStream.Seek(0, SeekOrigin.Begin);
            Assembly assembly = assemblyLoadContext.LoadFromStream(memoryStream);

            if (!enableLazyLoading)
            {
                assemblyLoadContext.Unload();
            }

            // ContextNamespace + ContextName из GeneratorOptions
            Type typeApplicationContext = assembly.GetType("ru.snowreplicator.scafolder.contextnamespace.ApplicationContext");
            _ = typeApplicationContext ?? throw new Exception("Type of ApplicationContext not found");

            // constructor of ApplicationContext
            ConstructorInfo constructorInfo = typeApplicationContext.GetConstructor(Type.EmptyTypes);
            _ = constructorInfo ?? throw new Exception("DataContext ctor not found");
           
            // constructor call
            DbContext applicationContext = (DbContext) constructorInfo.Invoke(null);

            return applicationContext;
        }
    }
}