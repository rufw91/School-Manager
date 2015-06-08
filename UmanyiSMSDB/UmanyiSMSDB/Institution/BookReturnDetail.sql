CREATE TABLE [Institution].[BookReturnDetail] (
    [BookReturnDetailID] INT              CONSTRAINT [DF_BookReturnDetail_BookReturnDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.BookReturnDetail')) NOT NULL,
    [BookReturnID]       INT              NOT NULL,
    [BookID]             INT              NOT NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_BookReturnDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_BookReturnDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BookReturnDetail] PRIMARY KEY CLUSTERED ([BookReturnDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_BookReturnDetail_UpdateID]
 ON [Institution].[BookReturnDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookReturnDetail')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookReturnDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookReturnDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookReturnDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookReturnDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookReturnDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookReturnDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookReturnDetail] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookReturnDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnDetail] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookReturnDetail] TO [Teacher]
    AS [dbo];

