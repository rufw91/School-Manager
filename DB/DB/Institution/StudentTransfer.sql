CREATE TABLE [Institution].[StudentTransfer] (
    [StudentTransferID] INT              CONSTRAINT [DF_StudentTransfer_StudentTransferID] DEFAULT ([dbo].[Link_GetNewID]('Institution.StudentTransfer')) NOT NULL,
    [StudentID]         INT              NOT NULL,
    [DateTransferred]   DATETIME         NOT NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_StudentTransfer_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [riwguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_StudentTransfer_riwguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StudentTransfer] PRIMARY KEY CLUSTERED ([StudentTransferID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_StudentTransfer_UpdateID]
 ON [Institution].[StudentTransfer] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentTransfer')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTransfer] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTransfer] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTransfer] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTransfer] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTransfer] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTransfer] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTransfer] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTransfer] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTransfer] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTransfer] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTransfer] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTransfer] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTransfer] TO [Teacher]
    AS [dbo];

