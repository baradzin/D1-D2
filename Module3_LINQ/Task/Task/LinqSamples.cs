// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Task 1")]
		[Description("1.	Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) " +
            "превосходит некоторую величину X. Продемонстрируйте выполнение запроса с различными X " +
            "(подумайте, можно ли обойтись без копирования запроса несколько раз)")]
		public void Linq1()
		{
            var X = 23000;
            var customers = dataSource.Customers;
            var targetCustomers = customers.Where(x => x.Orders.Sum(order => order.Total) > X);
			foreach (var x in targetCustomers)
			{
                ObjectDumper.Write(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 2")]
		[Description("2. Для каждого клиента составьте список поставщиков, находящихся в той " +
            "же стране и том же городе. Сделайте задания с использованием группировки и без.")]
		public void Linq2()
		{
            var customers = dataSource.Customers;
            var suppliers = dataSource.Suppliers;

            var targetSet = customers.Select(x => new
            {
                Customer = x,
                Suppliers = suppliers.Where(supplier => supplier.Country.Equals(x.Country) && supplier.City.Equals(x.City))
            });

            var supplierCusts = from cust in customers
                                join sup in suppliers on new { cust.City, cust.Country } equals new { sup.City, sup.Country } into cs
                                from c in cs//.DefaultIfEmpty() //Remove DefaultIfEmpty method call to make this an inner join   
                                orderby cust.CompanyName//cust.Country, cust.City
                                select new
                                {
                                    Country = cust.Country,
                                    City = cust.City,
                                    CompanyName = cust.CompanyName,
                                    Supplier = c == null ? "(No suppliers)" : c.SupplierName
                                };

            ObjectDumper.Write(supplierCusts);

            /*foreach (var item in targetSet)
            {
                ObjectDumper.Write(item.Customer);
                ObjectDumper.Write(item.Suppliers);
            }*/
        }

        [Category("Quantifiers Operators")]
        [Title("Task 3")]
        [Description("3. Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq3()
        {
            var X = 5000;
            var customers = dataSource.Customers;
            var targetCustomers = customers.Where(x => x.Orders.Any(order => order.Total > X));
            foreach (var x in targetCustomers)
            {
                ObjectDumper.Write(x);
            }
        }

        [Category("Aggregate Operators")]
        [Title("Task 4")]
        [Description("4. Выдайте список клиентов с указанием, начиная с какого месяца какого года " +
            "они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
        public void Linq4()
        {
            var customers = dataSource.Customers;

            var targetCustomers = customers.Select(x => new { Customer = x, Order = 
                x.Orders.DefaultIfEmpty().Aggregate((minOrd, order) => (minOrd == null ? DateTime.MaxValue : minOrd.OrderDate) < minOrd.OrderDate ? order : minOrd)});

            foreach (var x in targetCustomers)
            {
                Console.WriteLine($"{x.Customer.CustomerID} FirstOrder = {(x.Order == null ? "NONE" : x.Order.OrderDate.ToString())}");
            }
        }

        [Category("Ordering Operators")]
        [Title("Task 5")]
        [Description("5.	Сделайте предыдущее задание, но выдайте список отсортированным по году," +
            " месяцу, оборотам клиента (от максимального к минимальному) и имени клиента")]
        public void Linq5()
        {
            var customers = dataSource.Customers;
            var targetCustomers = customers.Select(x => new {
                Customer = x,
                Order =
                x.Orders.DefaultIfEmpty().Aggregate((minOrd, order) => (minOrd == null ? DateTime.MaxValue : minOrd.OrderDate) < minOrd.OrderDate ? order : minOrd)
            }).
            OrderBy(x => x.Order!= null ? x.Order.OrderDate.Year : DateTime.MinValue.Year).
            ThenBy(x => x.Order != null ? x.Order.OrderDate.Month : DateTime.MinValue.Month).
            ThenByDescending(x => x.Order != null ? x.Customer.Orders.Sum(ord => ord.Total) : 0).
            ThenBy(x => x.Customer.CompanyName).ToList();

            foreach (var x in targetCustomers)
            {
                ObjectDumper.Write(x.Customer);
                ObjectDumper.Write(x.Order);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 6")]
        [Description("6.	Укажите всех клиентов, у которых указан нецифровой почтовый код или не " +
            "заполнен регион или в телефоне не указан код оператора " +
            "(для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).")]
        public void Linq6()
        {
            var customers = dataSource.Customers;
            var targetCustomers = customers.Where(x => x.PostalCode == null 
                                                    || !x.PostalCode.All(char.IsDigit)
                                                    || x.Region == null 
                                                    || x.Phone == null 
                                                    || x.Phone.First().Equals("("));
            ObjectDumper.Write(targetCustomers);
        }

        [Category("Grouping Operators")]
        [Title("Task 7")]
        [Description("7.	Сгруппируйте все продукты по категориям," +
            " внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
        public void Linq7()
        {
            var products = dataSource.Products;
            var productGroups = from prod in products
                                group prod by prod.Category into сategoryGroup
                                select new
                                {
                                    Category = сategoryGroup.Key,
                                    UnitsInStockGroup =
                                        from prod in сategoryGroup
                                        orderby prod.UnitPrice
                                        group prod by prod.UnitsInStock into stockGroup
                                        select new
                                        {
                                            UnitsInStock = stockGroup.Key,
                                            Products = stockGroup
                                        }
                                };
            
            foreach(var product in productGroups)
            {
                Console.WriteLine(product.Category);
                foreach(var unitInStock in product.UnitsInStockGroup)
                {
                    Console.WriteLine($"   {unitInStock.UnitsInStock}");
                    foreach(var prod in unitInStock.Products)
                    {
                        Console.WriteLine($"      {prod.UnitPrice} {prod.ProductName}");
                    }
                }
            }
        }

        [Category("Grouping Operators")]
        [Title("Task 8")]
        [Description("8. Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». " +
            "Границы каждой группы задайте сами")]
        public void Linq8()
        {
            decimal[] ranges = new[] { 15, 100, decimal.MaxValue };
            var products = dataSource.Products;

            var groupings = products.GroupBy(item => ranges.First(range => range >= item.UnitPrice)).OrderBy(gr => gr.Key);

            foreach(var group in groupings)
            {
                Console.WriteLine($"Group {group.Key}");
                foreach(var prod in group)
                {
                    Console.WriteLine("  " + prod.UnitPrice);
                }
            }
           
        }

        [Category("Aggregate Operators")]
        [Title("Task 9")]
        [Description("9.	Рассчитайте среднюю прибыльность каждого города " +
            "(среднюю сумму заказа по всем клиентам из данного города) и среднюю интенсивность" +
            " (среднее количество заказов, приходящееся на клиента из каждого города)")]
        public void Linq9()
        {
            var customers = dataSource.Customers;
            var targetSet = customers.GroupBy(customer => customer.City)
                                     .Select(gr => new
                                     {
                                         City = gr.Key,
                                         AverageProfit = gr.Average(cst => cst.Orders.Count() != 0 ? cst.Orders.Average(ord => ord.Total) : 0),
                                         AverageIntensity = gr.Average(cst => cst.Orders.Count())
                                     });

            foreach (var x in targetSet)
            {
                Console.WriteLine($"{x.City} {x.AverageProfit}$ Intensity = {x.AverageIntensity}");
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 10")]
        [Description("10.	Сделайте среднегодовую статистику активности клиентов по месяцам" +
            " (без учета года), статистику по годам," +
            " по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение).")]
        public void Linq10()
        {
            //var customers = dataSource.Customers;
            //var monthStatExceptYear = customers.SelectMany(customer => customer.Orders).
           //                          .OrderBy(order => order.OrderDate.Month)

            //                         .Select(gr => new
            //                         {
            //                             City = gr.Key,
            //                             AverageProfit = gr.Average(cst => cst.Orders.Count() != 0 ? cst.Orders.Average(ord => ord.Total) : 0),
            //                             AverageIntensity = gr.Average(cst => cst.Orders.Count())
            //                         });
            //Console.WriteLine(targetSet.Count());
            //foreach (var x in targetSet)
            //{
            //    Console.WriteLine($"{x.City} {x.AverageProfit}$ Intensity = {x.AverageIntensity}");
            //}
        }
    }
}
