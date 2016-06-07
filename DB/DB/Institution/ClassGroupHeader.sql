CREATE TABLE [Institution].[ClassGroupHeader] (
    [ClassGroupID] INT              NOT NULL,
    [IsActive]     BIT              NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_ClassGroup_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_ClassGroup_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ClassGroup] PRIMARY KEY CLUSTERED ([ClassGroupID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ClassGroupHeader_UpdateID]
 ON [Institution].[ClassGroupHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ClassGroupHeader')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_ClassGroupHeader_SetActive_EndDate]
 ON [Institution].[ClassGroupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     UPDATE [Institution].[ClassGroupHeader] SET IsActive = 0 WHERE ClassGroupID NOT IN 
  (SELECT ClassGroupID FROM inserted)
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupHeader] TO [Teacher]
    AS [dbo];

