CREATE TABLE [Institution].[CurrentClass] (
    [CurrentClassID] INT              CONSTRAINT [DF_CurrentClass_CurrentClassID] DEFAULT ([dbo].[Link_GetNewID]('Institution.CurrentClass')) NOT NULL,
    [StudentID]      INT              NOT NULL,
    [ClassID]        INT              NOT NULL,
    [StartDateTime]  DATETIME         NULL,
    [EndDateTime]    DATETIME         NULL,
    [IsActive]       BIT              CONSTRAINT [DF_CurrentClass_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_CurrentClass_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_CurrentClass_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_CurrentClass] PRIMARY KEY CLUSTERED ([CurrentClassID] ASC)
);




GO
CREATE TRIGGER [Institution].[TR_CurrentClass_UpdateID]
 ON [Institution].[CurrentClass] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.CurrentClass')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_CurrentClass_SetActive_EndDate]
 ON [Institution].[CurrentClass] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     UPDATE [Institution].[CurrentClass] SET IsActive = 0 WHERE CurrentClassID NOT IN 
  (SELECT CurrentClassID FROM inserted) AND StudentID IN (SELECT StudentID FROM inserted) 
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[CurrentClass] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[CurrentClass] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[CurrentClass] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[CurrentClass] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[CurrentClass] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[CurrentClass] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[CurrentClass] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[CurrentClass] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[CurrentClass] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[CurrentClass] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[CurrentClass] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[CurrentClass] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[CurrentClass] TO [Teacher]
    AS [dbo];

