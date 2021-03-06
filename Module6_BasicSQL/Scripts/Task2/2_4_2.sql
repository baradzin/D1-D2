SELECT empl.EmployeeID, empl.FirstName, empl.LastName
  FROM Employees as empl
  WHERE EmployeeID IN 
	(SELECT ord.EmployeeID 
		FROM Orders as ord
		GROUP BY ord.EmployeeID
		HAVING COUNT(ord.OrderID) >= 150)