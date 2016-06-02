CREATE TABLE [Institution].[PayslipHeader] (
    [PayslipID]     INT              NOT NULL,
    [StaffID]       INT              NOT NULL,
    [AmountPaid]    DECIMAL (18)     NOT NULL,
    [DatePaid]      DATETIME         NOT NULL,
    [Designation]   VARCHAR (50)     NULL,
    [PaymentPeriod] VARCHAR (50)     NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_PayslipHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_PayslipHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_PayslipHeader] PRIMARY KEY CLUSTERED ([PayslipID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_PayslipHeader_UpdateID]
 ON Institution.PayslipHeader AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.PayslipHeader')
    END
   END
END