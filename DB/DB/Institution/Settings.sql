CREATE TABLE [Institution].[Settings] (
    [SettingID]    INT              CONSTRAINT [DF_Settings_SettingID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Settings')) NOT NULL,
    [Type]         VARCHAR (50)     NOT NULL,
    [Key]          VARCHAR (50)     NOT NULL,
    [Value]        VARCHAR (1000)   NOT NULL,
    [Value2]       VARCHAR (1000)   NULL,
    [Value3]       VARCHAR (1000)   NULL,
    [Value4]       VARCHAR (1000)   NULL,
    [Value5]       VARCHAR (1000)   NULL,
    [Value6]       VARCHAR (1000)   NULL,
    [Value7]       VARCHAR (1000)   NULL,
    [Value8]       VARCHAR (1000)   NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Settings_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_Settings_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ([SettingID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_Settings_UpdateID]
 ON [Institution].[Settings] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Settings')
    END
   END
END
GO
GRANT UPDATE
    ON OBJECT::[Institution].[Settings] TO [User]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Settings] TO [User]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Settings] TO [User]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Settings] TO [User]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Settings] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Settings] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Settings] TO [Teacher]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Settings] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Settings] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Settings] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Settings] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Settings] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Settings] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Settings] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Settings] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Settings] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Settings] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Settings] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Settings] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Settings] TO [Accounts]
    AS [dbo];

