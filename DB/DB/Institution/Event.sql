CREATE TABLE [Institution].[Event] (
    [EventID]       INT              CONSTRAINT [DF_Event_EventID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Event')) NOT NULL,
    [Name]          VARCHAR (50)     NULL,
    [StartDateTime] VARCHAR (50)     NULL,
    [EndDateTime]   VARCHAR (50)     NULL,
    [Location]      VARCHAR (50)     NULL,
    [Subject]       VARCHAR (50)     NULL,
    [Message]       VARCHAR (50)     NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_Event_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_Event_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([EventID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_Event_UpdateID]
 ON [Institution].[Event] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Event')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[Event] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Event] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Event] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Event] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Event] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Event] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Event] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Event] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Event] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Event] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Event] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Event] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Event] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Event] TO [User]
    AS [dbo];

