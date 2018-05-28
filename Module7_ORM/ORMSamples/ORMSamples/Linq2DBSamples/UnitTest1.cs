using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToDB.Mapping;
using LinqToDB.Data;
using LinqToDB;

namespace Linq2DBSamples
{
    [Table("Categories")]
    public class Category
    {
        [Column("CategoryID")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }
        [Column("CategoryName")]
        public string Name { get; set; }
        [Column]
        public string Description { get; set; }
    }

    public class Northwind : DataConnection
    {
        public Northwind() : base("Northwind")
        { }

        public ITable<Category> Categories { get { return GetTable<Category>(); } }
    }
    
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var connection = new DataConnection("Northwind"))
            {
                foreach (var cat in connection.GetTable<Category>())
                    Console.WriteLine("{0} {1} | {2}", cat.Id, cat.Name, cat.Description);
            }
        }
    }
}
