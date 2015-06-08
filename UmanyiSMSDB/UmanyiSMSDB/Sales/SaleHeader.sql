CREATE TABLE [Sales].[SaleHeader] (
    [SaleID]       INT              NOT NULL,
    [CustomerID]   VARCHAR (50)     NOT NULL,
    [EmployeeID]   INT              NOT NULL,
    [PaymentID]    INT              CONSTRAINT [DF_SaleHeader_PaymentID] DEFAULT ((0)) NULL,
    [IsCancelled]  VARCHAR (50)     NULL,
    [OrderDate]    DATETIME         NOT NULL,
    [TotalAmt]     AS               ([dbo].[GetSaleTotal]([SaleID])),
    [IsDiscount]   VARCHAR (50)     NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_SalesOrderHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_SalesOrderHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SalesOrderHeader] PRIMARY KEY CLUSTERED ([SaleID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_SaleHeader_UpdateID]
 ON [Sales].[SaleHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.SaleHeader')
    END
   END
END





GO
GRANT DELETE
    ON OBJECT::[Sales].[SaleHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SaleHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SaleHeader] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SaleHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[SaleHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SaleHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SaleHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SaleHeader] TO [Principal]
    AS [dbo];

