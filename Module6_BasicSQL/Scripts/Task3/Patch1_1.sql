/*Patch 1.1*/
/*Добавляет таблицу данных кредитных карт сотрудников:
 номер карты, дата истечения, имя card holder, ссылку на сотрудника, …*/
GO
use "Northwind"
GO
if exists (select * from sysobjects where id = object_id('dbo.CreditCards') and sysstat & 0xf = 3)
	drop table "dbo"."CreditCards"
GO

CREATE TABLE "CreditCards" (
	"CreditCardID" "int" IDENTITY (1, 1) NOT NULL ,
	"CardType" nvarchar (50) NOT NULL ,
	"CardNumber" nvarchar (25) NOT NULL ,
	"ExpMonth" "tinyint" NOT NULL ,
	"ExpYear" "smallint" NOT NULL ,
	"CVV" "smallint" NOT NULL,
	"CardHolderName" nvarchar (90) NOT NULL,
	"EmployeeID" "int" NOT NULL ,
	
	CONSTRAINT "PK_CreditCard" PRIMARY KEY  CLUSTERED 
	(
		"CreditCardID"
	),
	CONSTRAINT "FK_Employee" FOREIGN KEY 
	(
		"EmployeeID"
	) REFERENCES "dbo"."Employees" (
		"EmployeeID"
	),
)

/*GO
set quoted_identifier on
go
set identity_insert [Northwind].[dbo].[CreditCards] on
go
ALTER TABLE [Northwind].[dbo].[CreditCards] NOCHECK CONSTRAINT ALL
GO
INSERT INTO [Northwind].[dbo].[CreditCards] (CreditCardID, CardType, CardNumber, ExpMonth, ExpYear, CVV, CardHolderName, EmployeeID)
VALUES ('1', 'VISA', '2112 5456 4231', '5', '2021', '150', 'Anton Novikau', 2);
*/