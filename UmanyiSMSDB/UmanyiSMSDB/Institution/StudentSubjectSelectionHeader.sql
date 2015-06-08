CREATE TABLE [Institution].[StudentSubjectSelectionHeader] (
    [StudentSubjectSelectionID] INT              NOT NULL,
    [StudentID]                 INT              NOT NULL,
    [NoOfSubjects]              AS               ([dbo].[GetSubjectsTakenByStudent]([StudentSubjectSelectionID])),
    [IsActive]                  BIT              NOT NULL,
    [ModifiedDate]              DATETIME         CONSTRAINT [DF_StudentSubjectSelectionHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]                   UNIQUEIDENTIFIER CONSTRAINT [DF_StudentSubjectSelectionHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StudentSubjectSelectionHeader] PRIMARY KEY CLUSTERED ([StudentSubjectSelectionID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_StudentSubjectSelectionHeader_UpdateID]
 ON [Institution].[StudentSubjectSelectionHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentSubjectSelectionHeader')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_StudentSubjectSelectionHeader_SetActive_EndDate]
 ON [Institution].[StudentSubjectSelectionHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     UPDATE [Institution].[StudentSubjectSelectionHeader] SET IsActive = 0 WHERE StudentSubjectSelectionID NOT IN 
  (SELECT StudentSubjectSelectionID FROM inserted) AND StudentID IN (SELECT StudentID FROM inserted) 
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentSubjectSelectionHeader] TO [Teacher]
    AS [dbo];

