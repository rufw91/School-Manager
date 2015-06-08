CREATE TABLE [Sales].[StockTakingHeader] (
    [StockTakingID] INT              NOT NULL,
    [DateTaken]     DATETIME         NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_StockTakingHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_StockTakingHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StockTakingHeader] PRIMARY KEY CLUSTERED ([StockTakingID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_StockTakingHeader_UpdateID]
 ON [Sales].[StockTakingHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.StockTakingHeader')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[StockTakingHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[StockTakingHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[StockTakingHeader] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[StockTakingHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[StockTakingHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[StockTakingHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[StockTakingHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[StockTakingHeader] TO [Principal]
    AS [dbo];

