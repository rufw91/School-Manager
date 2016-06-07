CREATE TABLE [Institution].[StudentTranscriptDetail] (
    [StudentTranscriptDetailID] INT              CONSTRAINT [DF_StudentTranscriptDetail_StudentTranscriptDetailID] DEFAULT ([dbo].[GetNewID]('Institution.StudentTranscriptDetail')) NOT NULL,
    [StudentTranscriptID]       INT              NOT NULL,
    [SubjectID]                 INT              NOT NULL,
    [Remarks]                   VARCHAR (50)     NULL,
    [ModifiedDate]              DATETIME         CONSTRAINT [DF_StudentTranscriptDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]                   UNIQUEIDENTIFIER CONSTRAINT [DF_StudentTranscriptDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StudentTranscriptDetail] PRIMARY KEY CLUSTERED ([StudentTranscriptDetailID] ASC)
);




GO
CREATE TRIGGER [Institution].[TR_StudentTranscriptDetail_UpdateID]
 ON [Institution].[StudentTranscriptDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentTranscriptDetail')
    END
   END
END



GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Teacher]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTranscriptDetail] TO [Deputy]
    AS [dbo];

