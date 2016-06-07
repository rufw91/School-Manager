CREATE TABLE [Institution].[Book] (
    [BookID]       INT              CONSTRAINT [DF_Book_BookID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Book')) NOT NULL,
    [ISBN]         VARCHAR (50)     NOT NULL,
    [Name]         VARCHAR (50)     NOT NULL,
    [Author]       VARCHAR (50)     NOT NULL,
    [Publisher]    VARCHAR (50)     NOT NULL,
    [Price]        DECIMAL (18)     NOT NULL,
    [SPhoto]       VARBINARY (MAX)  NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Book_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_Book_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED ([BookID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_Book_UpdateID]
 ON [Institution].[Book] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Book')
    END
   END
END




GO
GRANT DELETE
    ON OBJECT::[Institution].[Book] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Book] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Book] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Book] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Book] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Book] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Book] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Book] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Book] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Book] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Book] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Book] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Book] TO [Teacher]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Book] TO [Teacher]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Book] TO [Teacher]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Book] TO [Teacher]
    AS [dbo];

