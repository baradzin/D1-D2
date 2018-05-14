begin try
	begin transaction

	delete from Northwind.[Order Details]
	delete from Northwind.Orders;
	delete from Northwind.Products;
	delete from Northwind.Categories;

	commit transaction
end try
begin catch 
	rollback transaction
end catch
