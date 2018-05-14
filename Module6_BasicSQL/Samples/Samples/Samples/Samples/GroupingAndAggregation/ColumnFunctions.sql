select AVG(UnitPrice), SUM(UnitsInStock) + SUM(UnitsOnOrder)
from Northwind.Products


select COUNT(UnitPrice), COUNT(UnitsOnOrder)
from Northwind.Products