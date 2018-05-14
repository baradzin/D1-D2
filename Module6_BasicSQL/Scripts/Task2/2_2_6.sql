SELECT empl.EmployeeID
	  ,(empl.FirstName + ' ' + empl.LastName) as Employee
      ,empl.ReportsTo
	  ,(director.FirstName + ' ' + director.LastName) as Director
  FROM Employees as empl
  join Employees as director
  on empl.ReportsTo = director.EmployeeID