CREATE TABLE [Sales].[ItemCategory] (
    [ItemCategoryID] INT              CONSTRAINT [DF_ItemCategory_ItemCategoryID] DEFAULT ([dbo].[Link_GetNewID]('Sales.ItemCategory')) NOT NULL,
    [Description]    VARCHAR (50)     NOT NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_ProductCategory_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_ProductCategory_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED ([ItemCategoryID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_ItemCategory_UpdateID]
 ON [Sales].[ItemCategory] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemCategory')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemCategory] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemCategory] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemCategory] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemCategory] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemCategory] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemCategory] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemCategory] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemCategory] TO [Principal]
    AS [dbo];

