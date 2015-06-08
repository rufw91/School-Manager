CREATE TABLE [Institution].[FeesPayment] (
    [FeesPaymentID] INT              NOT NULL,
    [StudentID]     INT              NOT NULL,
    [AmountPaid]    VARCHAR (50)     NOT NULL,
    [DatePaid]      DATETIME         NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_FeesPayment_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_FeesPayment_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_FeesPayment] PRIMARY KEY CLUSTERED ([FeesPaymentID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_FeesPayment_UpdateID]
 ON [Institution].[FeesPayment] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.FeesPayment')
    END
   END
END




GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesPayment] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesPayment] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesPayment] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesPayment] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesPayment] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesPayment] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesPayment] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesPayment] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[FeesPayment] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[FeesPayment] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[FeesPayment] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[FeesPayment] TO [Principal]
    AS [dbo];

