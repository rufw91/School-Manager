CREATE TABLE [Institution].[Donor] (
    [DonorID]      INT              CONSTRAINT [DF_Donor_DonorID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Donor')) NOT NULL,
    [NameOfDonor]  VARCHAR (50)     NOT NULL,
    [PhoneNo]      VARCHAR (50)     NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_Donor_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_Donor_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Donor] PRIMARY KEY CLUSTERED ([DonorID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[Institution].[Donor] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Donor] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Donor] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Donor] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Donor] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Donor] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Donor] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Donor] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Donor] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Donor] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Donor] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Donor] TO [Accounts]
    AS [dbo];

