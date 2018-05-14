SELECT EmployeeID, CustomerID, COUNT(OrderID) as Orders
  FROM [Northwind].[dbo].[Orders]
  WHERE YEAR(OrderDate) = '1998'
  GROUP BY EmployeeID, CustomerID
  ORDER BY EmployeeID