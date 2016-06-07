CREATE TABLE [Users].[UserRole] (
    [UserRoleID]   INT              NOT NULL,
    [Description]  VARCHAR (20)     NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_UserRole_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_UserRole_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserRoleID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[Users].[UserRole] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserRole] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Users].[UserRole] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Users].[UserRole] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserRole] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Users].[UserRole] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserRole] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[UserRole] TO [User]
    AS [dbo];

