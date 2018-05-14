DELETE FROM Northwind.Products 
WHERE UnitPrice > 50



UPDATE Northwind.Products
SET UnitPrice = UnitPrice + 3
WHERE UnitsInStock > 10


delete from Northwind.Products
where ProductID not in 
	(select od.ProductID
	from Northwind.[Order Details] od)
	

update Northwind.Products
SET UnitsOnOrder = 
	(select SUM(od.Quantity)
	from Northwind.[Order Details] od
	where od.ProductID = Products.ProductID)