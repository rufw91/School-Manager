CREATE TABLE [Sales].[StockTakingDetail] (
    [StockTakingDetailID] INT              CONSTRAINT [DF_StockTakingDetail_StockTakingDetailID] DEFAULT ([dbo].[Link_GetNewID]('Sales.StockTakingDetail')) NOT NULL,
    [StockTakingID]       INT              NOT NULL,
    [ItemID]              BIGINT           NOT NULL,
    [AvailableQuantity]   FLOAT (53)       NOT NULL,
    [Expected]            AS               ([dbo].[GetCurrentQuantity]([ItemID])),
    [VarianceQty]         AS               ([dbo].[GetCurrentQuantity]([ItemID])-[AvailableQuantity]),
    [VariancePc]          AS               ((([dbo].[GetCurrentQuantity]([ItemID])-[AvailableQuantity])*(100))/[dbo].[GetCurrentQuantity]([ItemID])),
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_StockTakingDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_StockTakingDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StockTakingDetail] PRIMARY KEY CLUSTERED ([StockTakingDetailID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_StockTakingDetail_UpdateID]
 ON [Sales].[StockTakingDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.StockTakingDetail')
    END
   END
END

GO
GRANT DELETE
    ON OBJECT::[Sales].[StockTakingDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[StockTakingDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[StockTakingDetail] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[StockTakingDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[StockTakingDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[StockTakingDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[StockTakingDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[StockTakingDetail] TO [Principal]
    AS [dbo];

