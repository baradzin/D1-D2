using DataModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.SqlProvider;

namespace Task1
{
    [TestFixture]
    public class Task3
    {
        private NorthwindDB db;

        [OneTimeSetUp]
        public void Init()
        {
            db = new NorthwindDB();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            db.Close();
        }

        //	Добавить нового сотрудника, и указать ему список территорий,
        //    за которые он несет ответственность.
        [Test]
        public void InsertEmployeeWithTerritories()
        {
            var emplId = Convert.ToInt32(db.InsertWithIdentity(new Employee { FirstName = "Aliaksei", LastName = "Baradzin" }));
            var territoriesSet = db.Territories.Take(2).ToArray();
            db.Insert(new EmployeeTerritory { EmployeeID = emplId, TerritoryID = territoriesSet[0].TerritoryID });
            db.Insert(new EmployeeTerritory { EmployeeID = emplId, TerritoryID = territoriesSet[1].TerritoryID });
        }

        //	Перенести продукты из одной категории в другую
        [Test]
        public void ChangeCategoryForProducts()
        {
            var rand = new Random();
            var categoryId = db.Categories.ToArray()[rand.Next(db.Categories.Count())].CategoryID;
            var product = db.Products.First(x => x.CategoryID != categoryId);
            product.CategoryID = categoryId;
            db.Update(product);
            Assert.AreEqual(db.Products.First(x => x.ProductID == product.ProductID).CategoryID, categoryId);
        }

        //•	Добавить список продуктов со своими поставщиками и категориями (массовое занесение),
        //при этом если поставщик или категория с таким названием есть, то использовать их – иначе создать новые. 
        [Test]
        public void FullSaveGraph()
        {

        }
    }
}
