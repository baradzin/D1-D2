select CategoryName, AVG(UnitPrice) Price, SUM(UnitsInStock) + SUM(UnitsOnOrder) Summ
from Northwind.Products as p
join Northwind.Categories as c
on p.CategoryID = c.CategoryID
group by p.CategoryID, CategoryName
having AVG(UnitPrice) > 30