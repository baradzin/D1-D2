/****** Script for SelectTopNRows command from SSMS  ******/
SELECT cust.CustomerID, cust.CompanyName, COUNT(ord.OrderID) AS Amount
  FROM [Customers] AS cust
  LEFT JOIN [Orders] AS ord
  ON cust.CustomerID = ord.CustomerID
  GROUP BY cust.CustomerID, cust.CompanyName
  ORDER BY Amount 