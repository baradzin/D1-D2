SELECT
	[CompanyName]
	,[Country]
FROM Customers
WHERE  Country IN ('USA', 'Canada')
ORDER BY [CompanyName], [Country]