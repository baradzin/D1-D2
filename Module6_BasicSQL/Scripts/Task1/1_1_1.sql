SELECT [OrderID]      
      ,[ShippedDate]
      ,[ShipVia]      
  FROM [Northwind].[dbo].[Orders]
  WHERE [ShippedDate]>= '1998-05-06' AND [ShipVia] >=2