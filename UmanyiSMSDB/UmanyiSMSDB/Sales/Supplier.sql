CREATE TABLE [Sales].[Supplier] (
    [SupplierID]     INT              CONSTRAINT [DF_Supplier_SupplierID] DEFAULT ([dbo].[Link_GetNewID]('Sales.Supplier')) NOT NULL,
    [NameOfSupplier] VARCHAR (50)     NOT NULL,
    [PhoneNo]        VARCHAR (50)     NOT NULL,
    [AltPhoneNo]     VARCHAR (50)     NOT NULL,
    [Email]          VARCHAR (50)     NOT NULL,
    [Address]        VARCHAR (50)     NOT NULL,
    [PostalCode]     VARCHAR (50)     NULL,
    [City]           VARCHAR (50)     NULL,
    [PINNo]          VARCHAR (50)     NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_Vendor_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_Vendor_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED ([SupplierID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_Supplier_UpdateID]
 ON [Sales].[Supplier] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.Supplier')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[Supplier] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[Supplier] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[Supplier] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[Supplier] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[Supplier] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[Supplier] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[Supplier] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[Supplier] TO [Principal]
    AS [dbo];

