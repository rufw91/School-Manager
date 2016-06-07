CREATE TABLE [Institution].[TimeTableHeader] (
    [TimeTableID]  INT              NOT NULL,
    [ClassID]      INT              NOT NULL,
    [IsActive]     BIT              CONSTRAINT [DF_TimeTableHeader_IsActive] DEFAULT ((1)) NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_TimeTableHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_TimeTableHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_TimeTableHeader] PRIMARY KEY CLUSTERED ([TimeTableID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_TimeTableHeader_UpdateID]
 ON [Institution].[TimeTableHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.TimeTableHeader')
    END
   END
END



GO
CREATE TRIGGER [Institution].[TR_TimeTableHeader_SetActive_EndDate]
 ON [Institution].[TimeTableHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN 
     UPDATE [Institution].[TimeTableHeader] SET IsActive = 0 WHERE TimeTableID NOT IN 
  (SELECT TimeTableID FROM inserted) AND ClassID IN (SELECT ClassID FROM inserted) 
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[TimeTableHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[TimeTableHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[TimeTableHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[TimeTableHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[TimeTableHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[TimeTableHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableHeader] TO [User]
    AS [dbo];

