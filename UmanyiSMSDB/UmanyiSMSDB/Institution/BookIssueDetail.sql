CREATE TABLE [Institution].[BookIssueDetail] (
    [BookIssueDetailID] INT              CONSTRAINT [DF_BookIssueDetail_BookIssueDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.BookIssueDetail')) NOT NULL,
    [BookIssueID]       INT              NOT NULL,
    [BookID]            INT              NOT NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_BookIssueDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_BookIssueDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BookIssueDetail] PRIMARY KEY CLUSTERED ([BookIssueDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_BookIssueDetail_UpdateID]
 ON [Institution].[BookIssueDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookIssueDetail')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookIssueDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookIssueDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookIssueDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookIssueDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookIssueDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookIssueDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookIssueDetail] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookIssueDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueDetail] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookIssueDetail] TO [Teacher]
    AS [dbo];

