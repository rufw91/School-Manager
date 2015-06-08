CREATE TABLE [Institution].[Discipline] (
    [DisciplineID] INT              CONSTRAINT [DF_Discipline_DisciplineID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Discipline')) NOT NULL,
    [StudentID]    INT              NOT NULL,
    [Issue]        VARCHAR (50)     NOT NULL,
    [DateAdded]    DATETIME         NOT NULL,
    [SPhoto]       VARBINARY (50)   NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Discipline_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_Discipline_rowguid] DEFAULT (newid()) NOT NULL
);


GO
CREATE TRIGGER [Institution].[TR_Discipline_UpdateID]
 ON [Institution].[Discipline] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Discipline')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[Discipline] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Discipline] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Discipline] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Discipline] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Discipline] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Discipline] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Discipline] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Discipline] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Discipline] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Discipline] TO [Teacher]
    AS [dbo];

