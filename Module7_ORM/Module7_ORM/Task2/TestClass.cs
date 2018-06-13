using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Task2.Entities;

namespace Task2
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void InitDb()
        {
            using (var db = new NorthwindDB())
            {
                db.Database.Create();
            }
        }

        [Test]
        public void Test()
        {
            using (var db = new NorthwindDB())
            {
                foreach (var category in db.Categories)
                {
                    foreach (var product in category.Products)
                    {
                        var orderDetails = db.Order_Details.Where(o => o.ProductID == product.ProductID);
                        foreach (var od in orderDetails)
                        {
                            var productOrders = db.Orders.Where(o => o.OrderID == od.OrderID);

                            foreach (var categoryOrder in productOrders)
                            {
                                Console.WriteLine(od.Product.Category.CategoryName);
                                Console.WriteLine("Product Name - " + od.Product.ProductName);
                                Console.WriteLine("Company Name - " + categoryOrder.Customer.CompanyName);
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }
    }
}
