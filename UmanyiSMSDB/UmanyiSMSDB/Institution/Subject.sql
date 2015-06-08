CREATE TABLE [Institution].[Subject] (
    [SubjectID]     INT              NOT NULL,
    [NameOfSubject] VARCHAR (50)     NOT NULL,
    [Code]          INT              NOT NULL,
    [MaximumScore]  VARCHAR (50)     CONSTRAINT [DF_SubjectHeader_MaximumScore] DEFAULT ((100)) NOT NULL,
    [IsOptional]    BIT              CONSTRAINT [DF_Subject_IsOptional] DEFAULT ((0)) NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_Subject_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_Subject_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED ([SubjectID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_Subject_UpdateID]
 ON [Institution].[Subject] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Subject')
    END
   END
END

GO
GRANT SELECT
    ON OBJECT::[Institution].[Subject] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Subject] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Subject] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Subject] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Subject] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Subject] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Subject] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Subject] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Subject] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Subject] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Subject] TO [User]
    AS [dbo];

