SELECT SUM([UnitPrice]*[Quantity]*(1-[Discount])) as [Totals]
  FROM [Northwind].[dbo].[Order Details]