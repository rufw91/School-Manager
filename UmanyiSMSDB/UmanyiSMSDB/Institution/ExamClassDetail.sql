CREATE TABLE [Institution].[ExamClassDetail] (
    [ExamClassDetailID] INT              CONSTRAINT [DF_ExamClassDetail_ExamClassDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.ExamClassDetail')) NOT NULL,
    [ExamID]            INT              NOT NULL,
    [ClassID]           INT              NOT NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_ExamClassDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_ExamClassDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ExamClassDetail] PRIMARY KEY CLUSTERED ([ExamClassDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ExamClassDetail_UpdateID]
 ON [Institution].[ExamClassDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamClassDetail')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamClassDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamClassDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamClassDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamClassDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamClassDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamClassDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamClassDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamClassDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamClassDetail] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamClassDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamClassDetail] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamClassDetail] TO [Teacher]
    AS [dbo];

