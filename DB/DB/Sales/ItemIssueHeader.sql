CREATE TABLE [Sales].[ItemIssueHeader] (
    [ItemIssueID]  INT              NOT NULL,
    [Description]  VARCHAR (50)     NOT NULL,
    [DateIssued]   DATETIME         NOT NULL,
    [IsCancelled]  BIT              NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_ItemIssueHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_ItemIssueHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ItemIssueHeader] PRIMARY KEY CLUSTERED ([ItemIssueID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_ItemIssueHeader_UpdateID]
 ON [Sales].[ItemIssueHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemIssueHeader')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemIssueHeader] TO [Principal]
    AS [dbo];

