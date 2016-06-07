CREATE TABLE [Sales].[SaleDetail] (
    [SalesOrderDetailID] INT              CONSTRAINT [DF_SaleDetail_SalesOrderDetailID] DEFAULT ([dbo].[Link_GetNewID]('Sales.SaleDetail')) NOT NULL,
    [SaleID]             INT              NOT NULL,
    [Name]               VARCHAR (50)     NOT NULL,
    [Amount]             DECIMAL (18)     NOT NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_SalesOrderDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_SalesOrderDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SalesOrderDetail] PRIMARY KEY CLUSTERED ([SalesOrderDetailID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_SaleDetail_UpdateID]
 ON [Sales].[SaleDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.SaleDetail')
    END
   END
END




GO
GRANT DELETE
    ON OBJECT::[Sales].[SaleDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SaleDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SaleDetail] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SaleDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SaleDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[SaleDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SaleDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SaleDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SaleDetail] TO [Principal]
    AS [dbo];

