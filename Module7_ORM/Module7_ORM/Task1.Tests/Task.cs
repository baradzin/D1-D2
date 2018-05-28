using System;
using NUnit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Task1;
using DataModels;

namespace Task1.Tests
{
    [TestFixture]
    public class Task
    {
        [Test]
        public void ProductListWithCategoryAndSuppliers()
        {
            using (var db = new MyDatabaseDb())
            {
                foreach (var product in db.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier))
                {
                    Console.WriteLine("Product Name - " + product.Name + ", Category - " + product.Category.Name + ", Supplier - " + product.Supplier.CompanyName);
                }
            }
        }
    }
}
