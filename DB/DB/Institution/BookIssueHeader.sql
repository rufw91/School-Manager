CREATE TABLE [Institution].[BookIssueHeader] (
    [BookIssueID]  INT              CONSTRAINT [DF_BookIssueHeader_BookIssueID] DEFAULT ([dbo].[Link_GetNewID]('BookIssueHeader')) NOT NULL,
    [StudentID]    INT              NOT NULL,
    [DateIssued]   DATETIME         NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_BookIssue_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_BookIssue_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BookIssue] PRIMARY KEY CLUSTERED ([BookIssueID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_BookIssueHeader_UpdateID]
 ON [Institution].[BookIssueHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookIssueHeader')
    END
   END
END




GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookIssueHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookIssueHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookIssueHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookIssueHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookIssueHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookIssueHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookIssueHeader] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookIssueHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookIssueHeader] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookIssueHeader] TO [Teacher]
    AS [dbo];

