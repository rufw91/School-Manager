CREATE TABLE [Institution].[ExamDetail] (
    [ExamDetailID] INT              CONSTRAINT [DF_ExamDetail_ExamDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.ExamDetail')) NOT NULL,
    [ExamID]       INT              NOT NULL,
    [SubjectID]    INT              NOT NULL,
    [ExamDateTime] DATETIME         NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_ExamDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_ExamDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ExamDetail] PRIMARY KEY CLUSTERED ([ExamDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ExamDetail_UpdateID]
 ON [Institution].[ExamDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamDetail')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamDetail] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamDetail] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamDetail] TO [User]
    AS [dbo];

