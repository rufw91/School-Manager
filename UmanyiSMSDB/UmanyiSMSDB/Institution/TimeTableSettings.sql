CREATE TABLE [Institution].[TimeTableSettings] (
    [TimeTableSettingsID] INT              NOT NULL,
    [NoOfLessons]         INT              NOT NULL,
    [LessonDuration]      INT              NOT NULL,
    [LessonsStartTime]    TIME (7)         NOT NULL,
    [BreakIndices]        VARCHAR (50)     NOT NULL,
    [BreakDuration]       VARCHAR (50)     NOT NULL,
    [IsActive]            BIT              CONSTRAINT [DF_TimeTableSettings_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_TimeTableSettings_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_TimeTableSettings_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_TimeTableSettings] PRIMARY KEY CLUSTERED ([TimeTableSettingsID] ASC)
);




GO
CREATE TRIGGER [Institution].[TR_TimeTableSettings_UpdateID]
 ON [Institution].[TimeTableSettings] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.TimeTableSettings')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_TimeTableSettings_SetActive_EndDate]
 ON [Institution].[TimeTableSettings] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN 
     UPDATE [Institution].[TimeTableSettings] SET IsActive = 0 WHERE TimeTableSettingsID NOT IN 
  (SELECT TimeTableSettingsID FROM inserted)
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[TimeTableSettings] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[TimeTableSettings] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableSettings] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[TimeTableSettings] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[TimeTableSettings] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[TimeTableSettings] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableSettings] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[TimeTableSettings] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableSettings] TO [User]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableSettings] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableSettings] TO [Accounts]
    AS [dbo];

