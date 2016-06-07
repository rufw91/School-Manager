CREATE TABLE [Institution].[SubjectSetupDetail] (
    [SubjectSetupDetailID] INT              CONSTRAINT [DF_SubjectSetupDetail_SubjectSetupDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.SubjectSetupDetail')) NOT NULL,
    [SubjectSetupID]       INT              NOT NULL,
    [SubjectID]            INT              NOT NULL,
    [Tutor]                VARCHAR (50)     NULL,
    [ModifiedDate]         DATETIME         CONSTRAINT [DF_SubjectSetupDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]              UNIQUEIDENTIFIER CONSTRAINT [DF_SubjectSetupDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SubjectSetupDetail] PRIMARY KEY CLUSTERED ([SubjectSetupDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_SubjectSetupDetail_UpdateID]
 ON [Institution].[SubjectSetupDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.SubjectSetupDetail')
    END
   END
END

GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[SubjectSetupDetail] TO [User]
    AS [dbo];

