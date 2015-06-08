CREATE TABLE [Institution].[ExamResultDetail] (
    [ExamResultDetail] INT              CONSTRAINT [DF_ExamResultDetail_ExamResultDetail] DEFAULT ([dbo].[Link_GetNewID]('Institution.ExamResultDetail')) NOT NULL,
    [ExamResultID]     INT              NOT NULL,
    [SubjectID]        INT              NOT NULL,
    [Score]            DECIMAL (18)     NOT NULL,
    [Grade]            AS               ([dbo].[GetGrade](CONVERT([decimal],[Score],(0)))),
    [Remarks]          VARCHAR (50)     NULL,
    [Tutor]            VARCHAR (50)     NULL,
    [ModifiedDate]     DATETIME         CONSTRAINT [DF_ExamResultDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]          UNIQUEIDENTIFIER CONSTRAINT [DF_ExamResultDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ExamResultDetail] PRIMARY KEY CLUSTERED ([ExamResultDetail] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ExamResultDetail_UpdateID]
 ON [Institution].[ExamResultDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamResultDetail')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamResultDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamResultDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamResultDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamResultDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamResultDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamResultDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamResultDetail] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamResultDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultDetail] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamResultDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultDetail] TO [User]
    AS [dbo];

