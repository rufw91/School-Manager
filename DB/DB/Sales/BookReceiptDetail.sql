CREATE TABLE [Sales].[BookReceiptDetail] (
    [BookReceiptDetailID] INT              CONSTRAINT [DF_BookReceiptDetail_BookReceiptDetailID] DEFAULT ([dbo].[Link_GetNewID]('Sales.BookReceiptDetail')) NOT NULL,
    [BookReceiptID]       INT              NOT NULL,
    [BookID]              INT              NOT NULL,
    [UnitCost]            DECIMAL (18)     NOT NULL,
    [Quantity]            INT              NOT NULL,
    [LineTotal]           DECIMAL (18)     NOT NULL,
    [ModifiedDate]        DATETIME         CONSTRAINT [DF_BookReceiptDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_BookReceiptDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BookReceiptDetail] PRIMARY KEY CLUSTERED ([BookReceiptDetailID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_BookReceiptDetail_UpdateID]
 ON [Sales].[BookReceiptDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.BookReceiptDetail')
    END
   END
END