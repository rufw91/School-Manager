CREATE TABLE [Sales].[ItemReceiptDetail] (
    [ItemReceiptDetailID] INT              CONSTRAINT [DF_ItemReceiptDetail_ItemReceiptDetailID] DEFAULT ([dbo].[Link_GetNewID]('Sales.ItemreceiptDetail')) NOT NULL,
    [ItemReceiptID]       INT              NOT NULL,
    [ItemID]              BIGINT           NOT NULL,
    [Quantity]            DECIMAL (18)     NOT NULL,
    [UnitCost]            DECIMAL (18)     NOT NULL,
    [LineTotal]           DECIMAL (18)     NOT NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_PurchaseOrderDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_PurchaseOrderDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_PurchaseOrderDetail] PRIMARY KEY CLUSTERED ([ItemReceiptDetailID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_ItemReceiptDetail_UpdateID]
 ON [Sales].[ItemReceiptDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemReceiptDetail')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemReceiptDetail] TO [Principal]
    AS [dbo];

