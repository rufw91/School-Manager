﻿CREATE TABLE [Institution].[ExamResultHeader] (
    [ExamResultID] INT              NOT NULL,
    [ExamID]       INT              NOT NULL,
    [StudentID]    INT              NOT NULL,
    [IsActive]     BIT              CONSTRAINT [DF_ExamResultHeader_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_ExamResultHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_ExamResultHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ExamResultHeader] PRIMARY KEY CLUSTERED ([ExamResultID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ExamResultHeader_UpdateID]
 ON [Institution].[ExamResultHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamResultHeader')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_ExamResultHeader_SetActive_EndDate]
 ON [Institution].[ExamResultHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
   UPDATE [Institution].[ExamResultHeader] SET IsActive=0 WHERE ExamResultID IN(
     SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamResultID NOT IN 
  (SELECT ExamResultID FROM inserted) AND StudentID IN 
  (SELECT StudentID FROM inserted) AND ExamID IN
  (SELECT ExamID FROM inserted))
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamResultHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamResultHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamResultHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamResultHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamResultHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamResultHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamResultHeader] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamResultHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultHeader] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamResultHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamResultHeader] TO [User]
    AS [dbo];
