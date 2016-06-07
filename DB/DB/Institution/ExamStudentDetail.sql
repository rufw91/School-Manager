CREATE TABLE [Institution].[ExamStudentDetail] (
    [ExamStudentDetailID] INT              CONSTRAINT [DF_ExamStudentDetail_ExamStudentDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.ExamStudentDetail')) NOT NULL,
    [ExamID]              INT              NOT NULL,
    [StudentID]           INT              NOT NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_ExamStudentDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_ExamStudentDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ExamStudentDetail] PRIMARY KEY CLUSTERED ([ExamStudentDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ExamStudentDetail_UpdateID]
 ON [Institution].[ExamStudentDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamStudentDetail')
    END
   END
END