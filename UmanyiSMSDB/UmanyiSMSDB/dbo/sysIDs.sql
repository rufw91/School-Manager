CREATE TABLE [dbo].[sysIDs] (
    [table_name] VARCHAR (50) NOT NULL,
    [last_id]    INT          NOT NULL,
    CONSTRAINT [PK_sysIDs] PRIMARY KEY CLUSTERED ([table_name] ASC)
);


GO
GRANT INSERT
    ON OBJECT::[dbo].[sysIDs] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[sysIDs] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[sysIDs] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[sysIDs] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[sysIDs] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[sysIDs] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[sysIDs] TO [Principal]
    WITH GRANT OPTION
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[sysIDs] TO [Principal]
    WITH GRANT OPTION
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[sysIDs] TO [Principal]
    WITH GRANT OPTION
    AS [dbo];


GO
GRANT CONTROL
    ON OBJECT::[dbo].[sysIDs] TO [SystemAdmin]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[sysIDs] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[sysIDs] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[sysIDs] TO [Teacher]
    AS [dbo];

