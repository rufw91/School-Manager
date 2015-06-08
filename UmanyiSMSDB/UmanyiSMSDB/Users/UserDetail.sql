CREATE TABLE [Users].[UserDetail] (
    [UserDetailID] INT              CONSTRAINT [DF_UserDetail_UserDetailID] DEFAULT ([dbo].[Link_GetNewID]('Users.UserDetail')) NOT NULL,
    [UserID]       VARCHAR (50)     NOT NULL,
    [UserRoleID]   INT              NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_UserDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_UserDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_UserDetail] PRIMARY KEY CLUSTERED ([UserDetailID] ASC)
);


GO
CREATE TRIGGER [Users].[TR_UserDetail_UpdateID]
 ON [Users].[UserDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Users.UserDetail')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Users].[UserDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Users].[UserDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Users].[UserDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Users].[UserDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserDetail] TO [User]
    AS [dbo];

