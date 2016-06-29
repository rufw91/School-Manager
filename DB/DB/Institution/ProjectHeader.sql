CREATE TABLE [Institution].[ProjectHeader] (
    [ProjectID]     INT              NOT NULL,
    [NameOfProject] VARCHAR (50)     NOT NULL,
    [StartDateTime] DATETIME         NOT NULL,
    [EndDateTime]   DATETIME         NOT NULL,
    [Budget]        DECIMAL (18)     NOT NULL,
    [Description]   VARCHAR (MAX)    NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_ProjectHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_ProjectHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ProjectHeader] PRIMARY KEY CLUSTERED ([ProjectID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[Institution].[ProjectHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ProjectHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ProjectHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ProjectHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ProjectHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ProjectHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ProjectHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ProjectHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ProjectHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ProjectHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ProjectHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ProjectHeader] TO [Accounts]
    AS [dbo];

