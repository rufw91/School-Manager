CREATE TABLE [Institution].[SubjectSetupHeader] (
    [SubjectSetupID] INT              NOT NULL,
    [ClassID]        INT              NOT NULL,
    [IsActive]       BIT              CONSTRAINT [DF_SubjectSetupHeader_IsActive] DEFAULT ((1)) NOT NULL,
    [StartDate]      DATETIME         NOT NULL,
    [EndDate]        DATETIME         NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_SubjectSetupHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_SubjectSetupHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SubjectSetup] PRIMARY KEY CLUSTERED ([SubjectSetupID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_SubjectSetupHeader_UpdateID]
 ON [Institution].[SubjectSetupHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.SubjectSetupHeader')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_SubjectSetupHeader_SetActive_EndDate]
 ON [Institution].[SubjectSetupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
  UPDATE [Institution].[SubjectSetupHeader] SET EndDate = SYSDATETIME() WHERE IsActive=1 
  AND SubjectSetupID NOT IN (SELECT SubjectSetupID FROM inserted) 
  AND ClassID IN (SELECT ClassID FROM inserted) 
     UPDATE [Institution].[SubjectSetupHeader] SET IsActive = 0 WHERE SubjectSetupID NOT IN 
  (SELECT SubjectSetupID FROM inserted) AND ClassID IN (SELECT ClassID FROM inserted) 
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupHeader] TO [User]
    AS [dbo];

