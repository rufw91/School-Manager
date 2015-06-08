CREATE TABLE [Institution].[PayoutHeader] (
    [PayoutID]     INT              NOT NULL,
    [Payee]        VARCHAR (50)     NOT NULL,
    [Address]      VARCHAR (50)     NOT NULL,
    [Description]  VARCHAR (200)    NULL,
    [TotalPaid]    VARCHAR (50)     NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_PayoutHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_PayoutHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_PayoutHeader] PRIMARY KEY CLUSTERED ([PayoutID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_PayoutHeader_UpdateID]
 ON Institution.PayoutHeader AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.PayoutHeader')
    END
   END
END

GO
GRANT DELETE
    ON OBJECT::[Institution].[PayoutHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[PayoutHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[PayoutHeader] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[PayoutHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[PayoutHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[PayoutHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[PayoutHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[PayoutHeader] TO [Principal]
    AS [dbo];

