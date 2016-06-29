CREATE TABLE [Institution].[QBSync] (
    [QBSyncID]     INT              CONSTRAINT [DF_QBSync_QBSyncID] DEFAULT ([dbo].[Link_GetNewID]('Institution.QBSync')) NOT NULL,
    [SyncType]     VARCHAR (50)     NOT NULL,
    [RefNo]        VARCHAR (50)     NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_QBSync_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_QBSync_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_QBSync] PRIMARY KEY CLUSTERED ([QBSyncID] ASC)
);




GO
CREATE TRIGGER [Institution].[TR_QBSync_UpdateID]
 ON [Institution].[QBSync] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.QBSync')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[QBSync] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[QBSync] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[QBSync] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[QBSync] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[QBSync] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[QBSync] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[QBSync] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[QBSync] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[QBSync] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[QBSync] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[QBSync] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[QBSync] TO [Principal]
    AS [dbo];

