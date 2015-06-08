CREATE TABLE [Institution].[ExamHeader] (
    [ExamID]       INT              NOT NULL,
    [NameOfExam]   VARCHAR (50)     NOT NULL,
    [OutOf]        DECIMAL (18)     NULL,
    [ExamDatetime] DATETIME         CONSTRAINT [DF_ExamHeader_ExamDatetime] DEFAULT (sysdatetime()) NOT NULL,
    [Modifieddate] DATETIME         CONSTRAINT [DF_ExamHeader_Modifieddate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_ExamHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ExamHeader_1] PRIMARY KEY CLUSTERED ([ExamID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ExamHeader_UpdateID]
 ON [Institution].[ExamHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamHeader')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ExamHeader] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ExamHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamHeader] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ExamHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ExamHeader] TO [User]
    AS [dbo];

