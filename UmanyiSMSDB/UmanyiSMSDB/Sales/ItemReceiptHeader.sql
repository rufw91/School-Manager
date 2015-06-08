CREATE TABLE [Sales].[ItemReceiptHeader] (
    [ItemReceiptID] INT              NOT NULL,
    [SupplierID]    INT              NOT NULL,
    [RefNo]         VARCHAR (50)     NOT NULL,
    [OrderDate]     DATETIME         NOT NULL,
    [TotalAmt]      AS               ([dbo].[GetPurchaseTotal]([ItemReceiptID])),
    [IsCancelled]   BIT              CONSTRAINT [DF_ItemReceiptHeader_IsCancelled] DEFAULT ((0)) NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_PurchaseOrderHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_PurchaseOrderHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_PurchaseOrderHeader] PRIMARY KEY CLUSTERED ([ItemReceiptID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_ItemReceiptHeader_UpdateID]
 ON [Sales].[ItemReceiptHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemReceiptHeader')
    END
   END
END




GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemReceiptHeader] TO [Principal]
    AS [dbo];

