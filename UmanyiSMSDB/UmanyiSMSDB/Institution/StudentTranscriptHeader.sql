CREATE TABLE [Institution].[StudentTranscriptHeader] (
    [StudentTranscriptID]  INT              NOT NULL,
    [StudentID]            INT              NOT NULL,
    [Exam1ID]              INT              NULL,
    [Exam2ID]              INT              NULL,
    [Exam3ID]              INT              NULL,
    [IsActive]             BIT              CONSTRAINT [DF_StudentTranscriptHeader_IsActive] DEFAULT ((1)) NULL,
    [Responsibilities]     VARCHAR (50)     NULL,
    [ClubsAndSport]        VARCHAR (50)     NULL,
    [Boarding]             VARCHAR (50)     NULL,
    [ClassTeacher]         VARCHAR (50)     NULL,
    [ClassTeacherComments] VARCHAR (50)     NULL,
    [Principal]            VARCHAR (50)     NULL,
    [PrincipalComments]    VARCHAR (50)     NULL,
    [OpeningDay]           DATETIME         NULL,
    [ClosingDay]           DATETIME         NULL,
    [Term1Pos]             VARCHAR (50)     NULL,
    [Term2Pos]             VARCHAR (50)     NULL,
    [Term3Pos]             VARCHAR (50)     NULL,
    [DateSaved]            DATETIME         NOT NULL,
    [ModifiedDate]         DATETIME         CONSTRAINT [DF_StudentTranscriptHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]              UNIQUEIDENTIFIER CONSTRAINT [DF_StudentTranscriptHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_StudentTranscriptHeader] PRIMARY KEY CLUSTERED ([StudentTranscriptID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_StudentTranscriptHeader_UpdateID]
 ON [Institution].[StudentTranscriptHeader] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentTranscriptHeader')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[StudentTranscriptHeader] TO [Teacher]
    AS [dbo];

