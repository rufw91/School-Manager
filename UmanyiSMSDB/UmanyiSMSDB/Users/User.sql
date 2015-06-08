CREATE TABLE [Users].[User] (
    [UserID]       VARCHAR (50)     NOT NULL,
    [Name]         VARCHAR (50)     NOT NULL,
    [SPhoto]       VARBINARY (MAX)  NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_User_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_User_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[Users].[User] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[User] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Users].[User] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Users].[User] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[User] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Users].[User] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[User] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Users].[User] TO [User]
    AS [dbo];

