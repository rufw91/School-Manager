CREATE TABLE [Sales].[SupplierPayment] (
    [SupplierPaymentID] INT              CONSTRAINT [DF_SupplierPayment_SupplierPaymentID] DEFAULT ([dbo].[Link_GetNewID]('Sales.SupplierPayment')) NOT NULL,
    [SupplierID]        INT              NOT NULL,
    [DatePaid]          DATETIME         NOT NULL,
    [AmountPaid]        DECIMAL (18)     NOT NULL,
    [Notes]             VARCHAR (50)     NOT NULL,
    [ModifiedDate]      DATETIME         CONSTRAINT [DF_SupplierPayment_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_SupplierPayment_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SupplierPayment] PRIMARY KEY CLUSTERED ([SupplierPaymentID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_SupplierPayment_UpdateID]
 ON [Sales].[SupplierPayment] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.SupplierPayment')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[SupplierPayment] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SupplierPayment] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SupplierPayment] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SupplierPayment] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[SupplierPayment] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SupplierPayment] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SupplierPayment] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SupplierPayment] TO [Principal]
    AS [dbo];

