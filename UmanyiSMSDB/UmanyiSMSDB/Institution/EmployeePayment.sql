CREATE TABLE [Institution].[EmployeePayment] (
    [EmployeePaymentID] INT              NOT NULL,
    [EmployeeID]        INT              NOT NULL,
    [AmountPaid]        DECIMAL (18)     NOT NULL,
    [DatePaid]          DATETIME         NOT NULL,
    [Notes]             VARCHAR (MAX)    NULL,
    [ModifiedDate]      NCHAR (10)       CONSTRAINT [DF_EmployeePayment_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_EmployeePayment_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_EmployeePayment] PRIMARY KEY CLUSTERED ([EmployeePaymentID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_EmployeePayment_UpdateID]
 ON [Institution].[EmployeePayment] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.EmployeePayment')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Institution].[EmployeePayment] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[EmployeePayment] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[EmployeePayment] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[EmployeePayment] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[EmployeePayment] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[EmployeePayment] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[EmployeePayment] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[EmployeePayment] TO [Principal]
    AS [dbo];

