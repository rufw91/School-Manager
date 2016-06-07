CREATE TABLE [Institution].[ClassSetupDetail] (
    [ClassSetupDetailID] INT              CONSTRAINT [DF_ClassSetupDetail_ClassSetupDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.ClassSetupDetail')) NOT NULL,
    [ClassSetupID]       INT              NOT NULL,
    [ClassID]            INT              NOT NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_ClassSetupDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_ClassSetupDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ClassSetupDetail] PRIMARY KEY CLUSTERED ([ClassSetupDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_ClassSetupDetail_UpdateID]
 ON [Institution].[ClassSetupDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ClassSetupDetail')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassSetupDetail] TO [User]
    AS [dbo];

