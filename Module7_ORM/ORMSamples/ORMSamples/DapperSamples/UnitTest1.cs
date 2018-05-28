using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Dapper;

namespace DapperSamples
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        public string connectionString =
            "Server=(local);Database=Northwind;Trusted_Connection=true";

        [TestMethod]
        public void TestMethod1()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var categories = SqlMapper.Query<Category>(
                    connection, "select * from Categories");

                foreach (var cat in categories)
                    Console.WriteLine("{0} {1} | {2}", cat.CategoryID,
                        cat.CategoryName, cat.Description);
            }
        }
    }
}
