SELECT DISTINCT emplTer.EmployeeID, empl.FirstName, empl.LastName
  FROM EmployeeTerritories AS emplTer
  join [Territories] AS ter
  ON emplter.TerritoryID = ter.TerritoryID
  join [Region] AS reg
  ON ter.RegionID = reg.RegionID AND reg.RegionDescription = 'Western'
  join Employees AS empl
  ON emplTer.EmployeeID = empl.EmployeeID
  