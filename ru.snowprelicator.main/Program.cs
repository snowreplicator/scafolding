using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using ru.snowprelicator.code_generation;
using ru.snowprelicator.loading;
using ru.snowprelicator.populate_database;
using ru.snowprelicator.scafolding;
using System;
using System.Collections.Generic;
using System.IO;

namespace ru.snowprelicator.main
{
    public class Program
    {
        // connect to database
        private static readonly string DB_CONNECTION_STRING = "Server=127.0.0.1;Port=5432;Database=visual_studio;Username=portal;Password=troP4444";

        // list of database schemas for scaffolding
        private static List<string> DB_SCHEMAS = new List<string>
        {
            "public"
        };

        // LazyLoading
        private static bool ENABLE_LAZY_LOADING = false;

        static void Main(string[] args)
        {
            Console.WriteLine("run exe in ru.snowprelicator.main namespace");

            // наполнение db данными
            PopulateDataBase.PopulateDatabase(DB_CONNECTION_STRING);


            // создание виртуального контеста бд
            ScaffoldedModel scaffoldedModel = Scafolding.CreateScaffolder(DB_CONNECTION_STRING, DB_SCHEMAS);

            MemoryStream memoryStream = CodeGeneration.GenerateCode(scaffoldedModel, ENABLE_LAZY_LOADING);

            DbContext applicationContext = Loading.LoadCompiledDll(memoryStream, ENABLE_LAZY_LOADING);
            memoryStream.Close();

            // действия с данными
            Utils.PrintAllApplicationContextTables(applicationContext.Model.GetEntityTypes());
        }

    }
}