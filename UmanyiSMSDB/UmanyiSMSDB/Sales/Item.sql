CREATE TABLE [Sales].[Item] (
    [ItemID]         BIGINT           NOT NULL,
    [Description]    VARCHAR (50)     NOT NULL,
    [DateAdded]      DATETIME         NOT NULL,
    [ItemCategoryID] INT              NOT NULL,
    [Price]          DECIMAL (18)     NOT NULL,
    [Cost]           DECIMAL (18)     NOT NULL,
    [StartQuantity]  DECIMAL (18)     NOT NULL,
    [VatID]          INT              NOT NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_Product_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_Product_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ItemID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_Item_UpdateID]
 ON [Sales].[Item] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.Item')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[Item] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[Item] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[Item] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[Item] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[Item] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[Item] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[Item] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[Item] TO [Principal]
    AS [dbo];

