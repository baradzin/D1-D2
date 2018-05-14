select c.CategoryName, p.Price, p.SummUnits
from 
	(select CategoryID, avg(UnitPrice) Price, SUM(UnitsInStock) + SUM(UnitsOnOrder) SummUnits
	from Northwind.Products 
	group by CategoryID) as p
join Northwind.Categories as c
on p.CategoryID = c.CategoryID