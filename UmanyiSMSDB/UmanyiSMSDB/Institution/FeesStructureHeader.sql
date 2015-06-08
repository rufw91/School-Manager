CREATE TABLE [Institution].[FeesStructureHeader] (
    [FeesStructureID] INT              NOT NULL,
    [ClassID]         INT              NOT NULL,
    [IsActive]        BIT              CONSTRAINT [DF_FeesStructureHeader_IsActive] DEFAULT ((1)) NULL,
    [StartDate]       DATETIME         NOT NULL,
    [EndDate]         DATETIME         NULL,
    [ModifiedDate]    DATETIME         CONSTRAINT [DF_FeesStructureHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_FeesStructureHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_FeesStructureHeader] PRIMARY KEY CLUSTERED ([FeesStructureID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_FeesStructureHeader_UpdateID]
 ON [Institution].[FeesStructureHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.FeesStructureHeader')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_FeesStructureHeader_SetActive_EndDate]
 ON [Institution].[FeesStructureHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
  UPDATE [Institution].[FeesStructureHeader] SET EndDate = SYSDATETIME() WHERE IsActive=1 
  AND FeesStructureID NOT IN (SELECT FeesStructureID FROM inserted) 
  AND ClassID IN (SELECT ClassID FROM inserted) 
     UPDATE [Institution].[FeesStructureHeader] SET IsActive = 0 WHERE FeesStructureID NOT IN 
  (SELECT FeesStructureID FROM inserted) AND ClassID IN (SELECT ClassID FROM inserted) 
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureHeader] TO [User]
    AS [dbo];

