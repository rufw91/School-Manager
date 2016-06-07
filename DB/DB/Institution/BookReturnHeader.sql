CREATE TABLE [Institution].[BookReturnHeader] (
    [BookReturnID] INT              CONSTRAINT [DF_BookReturnHeader_BookReturnID] DEFAULT ([dbo].[Link_GetNewID]('Institution.BookReturnHeader')) NOT NULL,
    [StudentID]    INT              NOT NULL,
    [DateReturned] DATETIME         NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_BookReturnHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_BookReturnHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BookReturnHeader] PRIMARY KEY CLUSTERED ([BookReturnID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_BookReturnHeader_UpdateID]
 ON [Institution].[BookReturnHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookReturnHeader')
    END
   END
END




GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookReturnHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookReturnHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookReturnHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookReturnHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookReturnHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookReturnHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BookReturnHeader] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BookReturnHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BookReturnHeader] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BookReturnHeader] TO [Teacher]
    AS [dbo];

