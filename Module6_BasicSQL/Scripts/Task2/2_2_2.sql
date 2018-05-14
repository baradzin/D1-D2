SELECT (empl.FirstName + ' ' + empl.LastName) AS [Employee], ord.Amount
FROM 
	(SELECT [EmployeeID] AS [Seller], COUNT([OrderID]) AS [Amount]
	FROM [Northwind].[dbo].[Orders]
	GROUP BY [EmployeeID]) AS ord
JOIN [Northwind].[dbo].[Employees] AS empl
ON ord.Seller = empl.EmployeeID
ORDER BY Amount DESC