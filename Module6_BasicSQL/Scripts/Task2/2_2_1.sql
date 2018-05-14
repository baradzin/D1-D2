SELECT YEAR([OrderDate]) AS [Year]
	   ,COUNT([OrderID]) AS [Total]
  FROM [Northwind].[dbo].[Orders]
  GROUP BY YEAR([OrderDate])