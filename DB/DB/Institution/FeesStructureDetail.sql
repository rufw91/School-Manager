CREATE TABLE [Institution].[FeesStructureDetail] (
    [FeesStructureDetailID] INT              CONSTRAINT [DF_FeesStructureDetail_FeesStructureDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.FeesStructureDetail')) NOT NULL,
    [FeesStructureID]       INT              NULL,
    [Name]                  VARCHAR (50)     NULL,
    [Amount]                DECIMAL (18)     NULL,
    [ModifiedDate]          DATETIME         CONSTRAINT [DF_FeesStructureDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]               UNIQUEIDENTIFIER CONSTRAINT [DF_FeesStructureDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_FeesStructureDetail] PRIMARY KEY CLUSTERED ([FeesStructureDetailID] ASC)
);




GO
CREATE TRIGGER [Institution].[TR_FeesStructureDetail_UpdateID]
 ON [Institution].[FeesStructureDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.FeesStructureDetail')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesStructureDetail] TO [User]
    AS [dbo];

