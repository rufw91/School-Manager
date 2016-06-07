CREATE TABLE [Institution].[ClassSetupHeader] (
    [ClassSetupID] INT              NOT NULL,
    [IsActive]     BIT              CONSTRAINT [DF_ClassSetupHeader_IsActive] DEFAULT ((1)) NOT NULL,
    [StartDate]    DATETIME         NOT NULL,
    [EndDate]      DATETIME         NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_SubjectSetup_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_SubjectSetup_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SubjectSetup_1] PRIMARY KEY CLUSTERED ([ClassSetupID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ClassSetupHeader_UpdateID]
 ON [Institution].[ClassSetupHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ClassSetupHeader')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_ClassSetupHeader_SetActive_EndDate]
 ON [Institution].[ClassSetupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
  UPDATE [Institution].[ClassSetupHeader] SET EndDate = SYSDATETIME() WHERE IsActive=1 
  AND ClassSetupID NOT IN (SELECT ClassSetupID FROM inserted)
     UPDATE [Institution].[ClassSetupHeader] SET IsActive = 0 WHERE ClassSetupID NOT IN 
  (SELECT ClassSetupID FROM inserted)
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupHeader] TO [User]
    AS [dbo];

