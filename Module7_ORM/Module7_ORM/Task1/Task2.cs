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
    public class Task2
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

        [Test]
        public void ProductListWithCategoryAndSuppliers()
        {
            foreach (var product in db.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier))
            {
                Console.WriteLine("Product Name - " + product.ProductName + ", Category - " + product.Category.CategoryName + ", Supplier - " + product.Supplier.CompanyName);
            }
        }

        [Test]
        public void EmployeeListWithRegions()
        {
            var targetList = db.EmployeeTerritories.Select(x => new { x.Employee.FirstName, x.Employee.LastName, x.Territory.Region.RegionDescription }).Distinct();
            foreach(var empl in targetList)
            {
                Console.WriteLine(empl.FirstName + " " + empl.LastName + " " + empl.RegionDescription);
            }
        }

        [Test]
        public void EmployeeRegionStatistic()
        {
            var targetList = db.EmployeeTerritories.Select(x => new { x.EmployeeID, x.Territory.Region })
                .Distinct()
                .GroupBy(x => x.Region.RegionDescription);
            foreach(var set in targetList)
            {
                Console.WriteLine(set.Key + " Count: " + set.Count());
            }
        }

        [Test]
        public void EmployeeWithSuppliersList()
        {
            var targetList = db.Orders
                .Select(x => new { EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName, x.Shipper.CompanyName })
                .Distinct()
                .GroupBy(x => x.EmployeeName);
            foreach(var set in targetList)
            {
                foreach(var ship in set)
                Console.WriteLine(set.Key + " " + ship.CompanyName);
            }
        }
    }
}
