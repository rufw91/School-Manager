CREATE TABLE [Institution].[TimeTableDetail] (
    [TimeTableDetailID] INT              CONSTRAINT [DF_TimeTableDetail_TimeTableDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.TimeTableDetail')) NOT NULL,
    [TimeTableID]       INT              NOT NULL,
    [SubjectIndex]      INT              NOT NULL,
    [NameOfSubject]     VARCHAR (50)     NOT NULL,
    [Tutor]             VARCHAR (50)     NULL,
    [Day]               VARCHAR (50)     NOT NULL,
    [StartTime]         TIME (7)         NOT NULL,
    [EndTime]           TIME (7)         NOT NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_TimeTableDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_TimeTableDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_TimeTableDetail] PRIMARY KEY CLUSTERED ([TimeTableDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_TimeTableDetail_UpdateID]
 ON [Institution].[TimeTableDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.TimeTableDetail')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[TimeTableDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[TimeTableDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[TimeTableDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[TimeTableDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[TimeTableDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[TimeTableDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableDetail] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[TimeTableDetail] TO [User]
    AS [dbo];

