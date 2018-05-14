select * 
from Northwind.Products
where UnitPrice > (select avg(UnitPrice) from Northwind.Products)


/* Comparison Test */ 
select ProductName, UnitsInStock
from Northwind.Products
where UnitsInStock >= 
	(select max(UnitsInStock) - 10 
	from Northwind.Products)

select ProductName, CategoryID 
from Northwind.Products
where CategoryID = (
	select CategoryID 
	from Northwind.Categories 
	Where CategoryName = 'Seafood')


/* Set Membership Test */

select Top 3 *
from Northwind.Orders
where OrderID in (
	select OrderID
	from Northwind.[Order Details]
	group by OrderID
	having sum(Quantity * UnitPrice * (1 - Discount)) < 200)
order by ShippedDate desc


/* Existence Test */
select *
from Northwind.Products p
where exists 
		(select p.ProductID
		from Northwind.[Order Details] o
		where o.ProductID = p.ProductID 
			AND o.UnitPrice * o.Quantity > 5000)
			

/* Quantified Tests */

select ProductName, UnitPrice
from Northwind.Products 
where UnitPrice >= ANY 
	(select 4 * SUM(UnitPrice * Quantity * (1 - Discount))
	from Northwind.[Order Details]
	group by OrderID)


select distinct p.ProductName
from Northwind.Products p
join Northwind.[Order Details] od 
on p.ProductID = od.ProductID
where od.UnitPrice * od.Quantity * (1 - od.Discount) < ALL 
	(select SUM(UnitPrice * Quantity * (1 - Discount))
	from Northwind.[Order Details]
	group by OrderID)