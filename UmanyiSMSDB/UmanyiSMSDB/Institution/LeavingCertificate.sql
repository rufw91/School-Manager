﻿CREATE TABLE [Institution].[LeavingCertificate] (
    [LeavingCertificateID] INT              CONSTRAINT [DF_LeavingCertificate_LeavingCertificateID] DEFAULT ([dbo].[Link_GetNewID]('Institution.LeavingCertificate')) NOT NULL,
    [StudentID]            INT              NOT NULL,
    [DateOfIssue]          DATETIME         NOT NULL,
    [DateOfBirth]          DATETIME         NOT NULL,
    [DateOfAdmission]      DATETIME         NOT NULL,
    [DateOfLeaving]        DATETIME         NOT NULL,
    [Nationality]          VARCHAR (50)     NOT NULL,
    [ClassEntered]         VARCHAR (50)     NOT NULL,
    [ClassLeft]            VARCHAR (50)     NOT NULL,
    [Remarks]              VARCHAR (1000)   NOT NULL,
    [ModifiedDate]         DATETIME         CONSTRAINT [DF_LeavingCertificate_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]              UNIQUEIDENTIFIER CONSTRAINT [DF_LeavingCertificate_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_LeavingCertificate] PRIMARY KEY CLUSTERED ([LeavingCertificateID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_LeavingCertificate_UpdateID]
 ON [Institution].[LeavingCertificate] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.LeavingCertificate')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[LeavingCertificate] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[LeavingCertificate] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[LeavingCertificate] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[LeavingCertificate] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[LeavingCertificate] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[LeavingCertificate] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[LeavingCertificate] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[LeavingCertificate] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[LeavingCertificate] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[LeavingCertificate] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[LeavingCertificate] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[LeavingCertificate] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[LeavingCertificate] TO [Teacher]
    AS [dbo];

