CREATE TABLE [Institution].[Student] (
    [StudentID]           INT              NOT NULL,
    [FirstName]           VARCHAR (50)     NOT NULL,
    [LastName]            VARCHAR (50)     NOT NULL,
    [MiddleName]          VARCHAR (50)     NOT NULL,
    [NameOfStudent]       AS               (((([FirstName]+' ')+[MiddleName])+' ')+[LastName]),
    [Gender]              VARCHAR (50)     NOT NULL,
    [ClassID]             AS               ([dbo].[GetCurrentClass]([StudentID])),
    [DateOfBirth]         VARCHAR (50)     NOT NULL,
    [DateOfAdmission]     VARCHAR (50)     NOT NULL,
    [NameOfGuardian]      VARCHAR (50)     NOT NULL,
    [GuardianPhoneNo]     VARCHAR (50)     NOT NULL,
    [Email]               VARCHAR (50)     NOT NULL,
    [Address]             VARCHAR (50)     NOT NULL,
    [City]                VARCHAR (50)     NOT NULL,
    [PostalCode]          VARCHAR (50)     NOT NULL,
    [IsActive]            AS               ([dbo].[GetStudentIsActive]([StudentID])),
    [PreviousBalance]     VARCHAR (50)     CONSTRAINT [DF_Student_PreviousBalance] DEFAULT ('0') NOT NULL,
    [PreviousInstitution] VARCHAR (50)     NULL,
    [KCPEScore]           INT              NULL,
    [DormitoryID]         INT              NULL,
    [BedNo]               VARCHAR (50)     NULL,
    [SPhoto]              VARBINARY (MAX)  NULL,
    [IsBoarder]           BIT              CONSTRAINT [DF_Student_IsBoarder] DEFAULT ((1)) NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_Student_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_Student_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED ([StudentID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_Student_UpdateID]
 ON [Institution].[Student] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Student')
    END
   END
END

GO
GRANT SELECT
    ON OBJECT::[Institution].[Student] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Student] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Student] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Student] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Student] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Student] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Student] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Student] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Student] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Student] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Student] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Student] TO [Teacher]
    AS [dbo];

