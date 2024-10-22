﻿using System;

using DBST_Test.Models;

using Logging.Net;

using MB.DBST;

namespace DBST_Test
{
    internal class Program
    {
        private static DBST<TestModel> Table { get; set; }

        static void Main(string[] args)
        {
            Logger.Info("Testing");


            MySQLActions.ConnectionString = $"server=<serverip>;" +
                                                        $"port=<serverport>;" +
                                                        $"database=<databasename>;" +
                                                        $"uid=<username>;" +
                                                        $"pwd=<password>";

            Table = new DBST<TestModel>("test");

            Table.Insert(new TestModel()
            {
                Test1 = "foxy"
            });

            TestModel model = null;

            var m = Table.AsList().Find(x => x.Test1 == "foxy");

            m.Test1 = "fox";

            Table.Modify(m);

            Table.Delete(model);

            Console.ReadLine();
        }
    }
}
