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