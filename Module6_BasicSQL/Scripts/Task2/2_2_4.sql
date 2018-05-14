SELECT cust.City, cust.CustomerID, (empl.FirstName + ' ' + empl.LastName) AS Employee
  FROM Employees as empl, Customers as cust
  WHERE cust.City = empl.City
  ORDER BY cust.City