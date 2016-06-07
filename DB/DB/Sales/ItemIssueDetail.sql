CREATE TABLE [Sales].[ItemIssueDetail] (
    [ItemIssueDetailID] INT              CONSTRAINT [DF_ItemIssueDetail_ItemIssueDetailID] DEFAULT ([dbo].[Link_GetNewID]('Sales.ItemIssueDetail')) NOT NULL,
    [ItemIssueID]       INT              NOT NULL,
    [ItemID]            BIGINT           NOT NULL,
    [Quantity]          DECIMAL (18)     NOT NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_ItemIssueDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_ItemIssueDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ItemIssueDetail] PRIMARY KEY CLUSTERED ([ItemIssueDetailID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_ItemIssueDetail_UpdateID]
 ON [Sales].[ItemIssueDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemIssueDetail')
    END
   END
END




GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[ItemIssueDetail] TO [Principal]
    AS [dbo];

