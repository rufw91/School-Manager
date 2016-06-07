CREATE TABLE [Institution].[Staff] (
    [StaffID]         INT              NOT NULL,
    [Name]            VARCHAR (50)     NULL,
    [NationalID]      VARCHAR (50)     NULL,
    [DateOfAdmission] DATETIME         NULL,
    [PhoneNo]         VARCHAR (50)     NULL,
    [Email]           VARCHAR (50)     NULL,
    [Address]         VARCHAR (50)     NULL,
    [City]            VARCHAR (50)     NULL,
    [PostalCode]      VARCHAR (50)     NULL,
    [SPhoto]          VARBINARY (MAX)  NULL,
    [Designation]     VARCHAR (50)     NULL,
    [ModifiedDate]    DATETIME         CONSTRAINT [DF_Staff_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_Staff_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED ([StaffID] ASC)
);




GO
CREATE TRIGGER [Institution].[TR_Staff_UpdateID]
 ON [Institution].[Staff] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Staff')
    END
   END
END



GO
GRANT SELECT
    ON OBJECT::[Institution].[Staff] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Staff] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Staff] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Staff] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Staff] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Staff] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Staff] TO [Principal]
    AS [dbo];

