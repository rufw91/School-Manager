CREATE TABLE [Sales].[SupplierDetail] (
    [SupplierDetailID] INT              CONSTRAINT [DF_SupplierDetail_SupplierDetailID] DEFAULT ([dbo].[Link_GetNewID]('Sales.SupplierDetail')) NOT NULL,
    [SupplierID]       INT              NOT NULL,
    [ItemID]           BIGINT           NOT NULL,
    [ModifiedDate]     DATETIME         CONSTRAINT [DF_SupplierDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]          UNIQUEIDENTIFIER CONSTRAINT [DF_SupplierDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SupplierDetail] PRIMARY KEY CLUSTERED ([SupplierDetailID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[Sales].[SupplierDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SupplierDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SupplierDetail] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SupplierDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Sales].[SupplierDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Sales].[SupplierDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Sales].[SupplierDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Sales].[SupplierDetail] TO [Principal]
    AS [dbo];

