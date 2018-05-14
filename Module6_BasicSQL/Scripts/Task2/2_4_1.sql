SELECT [SupplierID]
      ,[CompanyName]
  FROM [Suppliers]
  WHERE SupplierID IN 
	(SELECT DISTINCT SupplierID 
		FROM Products as prod
		WHERE prod.UnitsInStock = 0)