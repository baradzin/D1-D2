SELECT [OrderID] AS [Order Number]     
      ,ISNULL(CONVERT(VARCHAR(30), [ShippedDate], 121), 'Not Shipped') AS [Shipped Date]  
  FROM [Northwind].[dbo].[Orders]
  WHERE [ShippedDate] >= '1998-05-06' OR [ShippedDate] IS NULL 
  --NO RESULTS IF > 1998-05-06, Decided to use >=