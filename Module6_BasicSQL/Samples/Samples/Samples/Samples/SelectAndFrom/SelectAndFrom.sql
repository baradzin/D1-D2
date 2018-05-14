-- From samples
select * 
from Northwind.Categories

select cat.*
from Northwind.Categories cat

select Categories.*
from Northwind.Categories 


-- Select samples
select CategoryID, CategoryName
from Northwind.Categories


select cat.CategoryID, cat.CategoryName
from Northwind.Categories as cat 


-- Select expressions
select 
	p.ProductName, 
	p.UnitPrice * (p.UnitsInStock + p.UnitsOnOrder) as SummaryCosts
from Northwind.Products p


select GETDATE() as Now, 2 + 5 * 6 as Expr
