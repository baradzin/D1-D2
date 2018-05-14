SELECT [OrderID], 
		CASE
			WHEN [ShippedDate] IS NULL THEN 'Not Shipped' ELSE CONVERT(VARCHAR(30), [ShippedDate], 121) END   
  FROM [Northwind].[dbo].[Orders]
  WHERE [ShippedDate] IS NULL