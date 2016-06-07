CREATE TABLE [Sales].[BookReceiptHeader] (
    [BookReceiptID] INT              NOT NULL,
    [SupplierID]    INT              NOT NULL,
    [DateReceived]  DATETIME         NOT NULL,
    [RefNo]         VARCHAR (50)     NOT NULL,
    [TotalAmt]      AS               ([dbo].[GetBookPurchaseTotal]([BookReceiptID])),
    [IsCancelled]   BIT              NOT NULL,
    [ModiedDate]    DATETIME         CONSTRAINT [DF_BookReceiptHeader_ModiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_BookReceiptHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BookReceiptHeader] PRIMARY KEY CLUSTERED ([BookReceiptID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_BookReceiptHeader_UpdateID]
 ON Sales.BookReceiptHeader AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.BookReceiptHeader')
    END
   END
END