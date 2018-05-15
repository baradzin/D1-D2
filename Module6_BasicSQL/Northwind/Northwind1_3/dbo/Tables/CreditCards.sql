CREATE TABLE [dbo].[CreditCards] (
    [CreditCardID]   INT           IDENTITY (1, 1) NOT NULL,
    [CardType]       NVARCHAR (50) NOT NULL,
    [CardNumber]     NVARCHAR (25) NOT NULL,
    [ExpMonth]       TINYINT       NOT NULL,
    [ExpYear]        SMALLINT      NOT NULL,
    [CVV]            SMALLINT      NOT NULL,
    [CardHolderName] NVARCHAR (90) NOT NULL,
    [EmployeeID]     INT           NOT NULL,
    CONSTRAINT [PK_CreditCard] PRIMARY KEY CLUSTERED ([CreditCardID] ASC),
    CONSTRAINT [FK_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

