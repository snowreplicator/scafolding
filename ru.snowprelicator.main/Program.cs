﻿using Microsoft.EntityFrameworkCore.Scaffolding;
using ru.snowprelicator.populate_database;
using ru.snowprelicator.scafolding;
using System;
using System.Collections.Generic;

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

        static void Main(string[] args)
        {
            Console.WriteLine("run exe in ru.snowprelicator.main namespace");
            PopulateDataBase.PopulateDatabase(DB_CONNECTION_STRING);

            ScaffoldedModel scaffoldedModel = Scafolding.CreateScaffolder(DB_CONNECTION_STRING, DB_SCHEMAS);
        }
    }
}