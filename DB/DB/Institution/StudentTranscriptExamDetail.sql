CREATE TABLE [Institution].[StudentTranscriptExamDetail] (
    [StudentTranscriptExamDetailID] INT              CONSTRAINT [DF_StudentTranscriptExamDetail_StudentTranscriptExamDetailID] DEFAULT ([dbo].[GetNewId]('Institution.StudentTranscriptExamDetail')) NOT NULL,
    [StudentTranscriptID]           INT              NOT NULL,
    [Exam1ID]                       INT              NULL,
    [Exam2ID]                       INT              NULL,
    [Exam3ID]                       INT              NULL,
    [Exam1Weight]                   DECIMAL (18)     NULL,
    [Exam2Weight]                   DECIMAL (18)     NULL,
    [Exam3Weight]                   DECIMAL (18)     NULL,
    [ModifiedDate]                  DATETIME         CONSTRAINT [DF_StudentTranscriptExamDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]                       UNIQUEIDENTIFIER CONSTRAINT [DF_StudentTranscriptExamDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StudentTranscriptExamDetail] PRIMARY KEY CLUSTERED ([StudentTranscriptExamDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_StudentTranscriptExamDetail_UpdateID]
 ON [Institution].[StudentTranscriptExamDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentTranscriptExamDetail')
    END
   END
END