﻿CREATE TABLE [Institution].[ClassGroupDetail] (
    [ClassGroupDetailID] INT              CONSTRAINT [DF_ClassGroupDetail_ClassGroupDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.ClassGroupDetail')) NOT NULL,
    [ClassGroupID]       INT              NOT NULL,
    [ClassID]            INT              NOT NULL,
    [ModifiedDate]       DATETIME         CONSTRAINT [DF_ClassGroupDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_ClassGroupDetail_rowguid] DEFAULT (newid()) NOT NULL
);


GO
CREATE TRIGGER [Institution].[TR_ClassGroupDetail_UpdateID]
 ON [Institution].[ClassGroupDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ClassGroupDetail')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[ClassGroupDetail] TO [Teacher]
    AS [dbo];

