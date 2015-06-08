CREATE TABLE [Institution].[StudentSubjectSelectionDetail] (
    [StudentSubjectSelectionDetailID] INT              CONSTRAINT [DF_StudentSubjectSelectionDetail_StudentSubjectSelectionDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.StudentSubjectSelectionDetail')) NOT NULL,
    [StudentSubjectSelectionID]       INT              NOT NULL,
    [SubjectID]                       INT              NOT NULL,
    [ModifiedDate]                    DATETIME         CONSTRAINT [DF_StudentSubjectSelectionDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]                         UNIQUEIDENTIFIER CONSTRAINT [DF_StudentSubjectSelectionDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StudentSubjectSelectionDetail] PRIMARY KEY CLUSTERED ([StudentSubjectSelectionDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_StudentSubjectSelectionDetail_UpdateID]
 ON [Institution].[StudentSubjectSelectionDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentSubjectSelectionDetail')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentSubjectSelectionDetail] TO [Teacher]
    AS [dbo];

