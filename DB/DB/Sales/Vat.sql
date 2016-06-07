CREATE TABLE [Sales].[Vat] (
    [VatID]        INT              CONSTRAINT [DF_Vat_VatID] DEFAULT ([dbo].[Link_GetNewID]('Sales.Vat')) NOT NULL,
    [Description]  VARCHAR (50)     NOT NULL,
    [Rate]         DECIMAL (18)     NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Vat_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_Vat_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Vat] PRIMARY KEY CLUSTERED ([VatID] ASC)
);


GO
CREATE TRIGGER [Sales].[TR_Vat_UpdateID]
 ON [Sales].[Vat] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.Vat')
    END
   END
END



GO
GRANT DELETE
    ON OBJECT::[Sales].[Vat] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[Vat] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[Vat] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[Vat] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[Vat] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[Vat] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[Vat] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[Vat] TO [Principal]
    AS [dbo];

