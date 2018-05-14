SELECT
	[CompanyName]
	,[Country]
FROM Customers
WHERE  Country NOT IN ('USA', 'Canada')
ORDER BY [CompanyName]