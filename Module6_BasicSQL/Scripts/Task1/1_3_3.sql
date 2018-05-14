SELECT
	[CustomerID]
	,[Country]
FROM Customers
WHERE  LEFT([Country], 1) >= 'b' AND LEFT([Country], 1) <= 'g'