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

            Category cat1 = new Category { CategoryName = "Beverages" };
            Category cat2 = new Category { CategoryName = "SomeCategory" };
            Category cat3 = new Category { CategoryName = "Beverages" };

            Supplier sup1 = new Supplier { CompanyName = "Nord-Ost-Fisch Handelsgesellschaft mbH" };
            Supplier sup2 = new Supplier { CompanyName = "SomeSupplier" };
            Supplier sup3 = new Supplier { CompanyName = "TestSupplier" };

            var products = new List<Product>
                {
                    new Product
                    {
                        ProductName = "FirstPr",
                        Category = InsertOrReturnExistValue(cat1),
                        Supplier = InsertOrReturnExistValue(sup2)
                    },
                    new Product
                    {
                        ProductName = "SecondPr",
                        Category = InsertOrReturnExistValue(cat2),
                        Supplier = InsertOrReturnExistValue(sup2)
                    },
                    new Product
                    {
                        ProductName = "ThirdPr",
                        Category = InsertOrReturnExistValue(cat3),
                        Supplier = InsertOrReturnExistValue(sup3)
                    }
            };
            foreach(var pr in products)
            {
                pr.CategoryID = pr.Category.CategoryID;
                pr.SupplierID = pr.Supplier.SupplierID;
                db.InsertWithIdentity(pr);
            }
        }

        //•	Замена продукта на аналогичный: во всех еще неисполненных заказах 
        //(считать таковыми заказы, у которых ShippedDate = NULL) заменить один продукт на другой
        [Test]
        public void ReplacementOfProducts()
        {
            var notShippedOrders = db.OrderDetails
                                    .LoadWith(x => x.Order)
                                    .Where(x => x.Order.ShippedDate == null).ToList();

            HashSet<int> productsToReplace = new HashSet<int>(notShippedOrders.Select(x => x.ProductID));
            Dictionary<int, int> productAnalogs = new Dictionary<int, int>();

            var rand = new Random();
            foreach (var pr in productsToReplace)
            {
                productAnalogs.Add(pr, rand.Next(db.Products.Count()));
            }

            foreach (var notShippedOrder in notShippedOrders)
            {
                var analog = productAnalogs[notShippedOrder.ProductID];
                db.OrderDetails.Where(
                    od => od.OrderID == notShippedOrder.OrderID && od.ProductID == notShippedOrder.ProductID).
                        Update(od => new OrderDetail
                        {
                            ProductID = analog,
                            UnitPrice = db.Products.Find(analog).UnitPrice.Value
                        });
            }
        }

        public T InsertOrReturnExistValue<T>(T entity) where T : class, IEquatable<T>
        {
            ITable<T> table = db.GetTable<T>();
            foreach(var obj in table)
            {
                if (entity.Equals(obj)) {
                    return obj;
                }                   
            }
            db.InsertWithIdentity(entity);
            return entity;
            
        }
    }
}
