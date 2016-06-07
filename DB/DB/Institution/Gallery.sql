CREATE TABLE [Institution].[Gallery] (
    [GalleryID]    INT              CONSTRAINT [DF_Gallery_GalleryID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Gallery')) NOT NULL,
    [Name]         VARCHAR (255)    NOT NULL,
    [Data]         VARBINARY (MAX)  NOT NULL,
    [DateAdded]    DATETIME         NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Gallery_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_Gallery_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Gallery] PRIMARY KEY CLUSTERED ([GalleryID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_Gallery_UpdateID]
 ON [Institution].[Gallery] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Gallery')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[Gallery] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Gallery] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Gallery] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Gallery] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Gallery] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Gallery] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Gallery] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Gallery] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Gallery] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Gallery] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Gallery] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Gallery] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Gallery] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Gallery] TO [User]
    AS [dbo];

