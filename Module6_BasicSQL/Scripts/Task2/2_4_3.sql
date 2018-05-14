/*3.	Выдать всех заказчиков (таблица Customers), которые не имеют 
ни одного заказа (подзапрос по таблице Orders). Использовать оператор EXISTS.*/
SELECT cust.CustomerID
FROM Customers as cust
WHERE NOT EXISTS 
	(SELECT ord.OrderID 
		FROM Orders as ord 
		WHERE ord.CustomerID = cust.CustomerID)