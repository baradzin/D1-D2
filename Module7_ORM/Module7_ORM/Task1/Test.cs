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
    public class Task
    {
        [Test]
        public void ProductListWithCategoryAndSuppliers()
        {
            using (var db = new NorthwindDB())
            {
                foreach (var product in db.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier))
                {
                    Console.WriteLine("Product Name - " + product.ProductName + ", Category - " + product.Category.CategoryName + ", Supplier - " + product.Supplier.CompanyName);
                }
            }
        }
    }
}
