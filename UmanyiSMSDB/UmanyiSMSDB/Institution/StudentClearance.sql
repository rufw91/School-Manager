CREATE TABLE [Institution].[StudentClearance] (
    [StudentClearanceID] INT              CONSTRAINT [DF_StudentClearance_StudentClearanceID] DEFAULT ([dbo].[Link_GetNewID]('Institution.StudentClearance')) NOT NULL,
    [StudentID]          INT              NOT NULL,
    [DateCleared]        DATETIME         NOT NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_StudentClearance_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_StudentClearance_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StudentClearance] PRIMARY KEY CLUSTERED ([StudentClearanceID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_StudentClearance_UpdateID]
 ON [Institution].[StudentClearance] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentClearance')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentClearance] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentClearance] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentClearance] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentClearance] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentClearance] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentClearance] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentClearance] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentClearance] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentClearance] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentClearance] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentClearance] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentClearance] TO [Teacher]
    AS [dbo];

