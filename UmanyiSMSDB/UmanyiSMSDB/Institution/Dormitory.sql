CREATE TABLE [Institution].[Dormitory] (
    [DormitoryID]     INT              CONSTRAINT [DF_Dormitory_DormitoryID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Dormitory')) NOT NULL,
    [NameOfDormitory] VARCHAR (50)     NOT NULL,
    [ModifiedDate]    DATETIME         CONSTRAINT [DF_Dormitory_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_Dormitory_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Dormitory] PRIMARY KEY CLUSTERED ([DormitoryID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_Dormitory_UpdateID]
 ON [Institution].[Dormitory] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Dormitory')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[Dormitory] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Dormitory] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Dormitory] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Dormitory] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Dormitory] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Dormitory] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Dormitory] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Dormitory] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Dormitory] TO [Teacher]
    AS [dbo];

