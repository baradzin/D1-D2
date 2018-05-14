select CategoryID, AVG(UnitPrice), SUM(UnitsInStock) + SUM(UnitsOnOrder)
from Northwind.Products
group by CategoryID


select CategoryName, AVG(UnitPrice), SUM(UnitsInStock) + SUM(UnitsOnOrder)
from Northwind.Products as p
join Northwind.Categories as c
on p.CategoryID = c.CategoryID
group by p.CategoryID, CategoryName
