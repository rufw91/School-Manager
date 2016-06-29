CREATE TABLE [Institution].[Donation] (
    [DonationID]    INT              CONSTRAINT [DF_Donation_DonationID] DEFAULT ([dbo].[Link_GetNewID]('Institution.Donation')) NOT NULL,
    [DonorID]       INT              NOT NULL,
    [AmountDonated] DECIMAL (18)     NOT NULL,
    [DonateTo]      VARCHAR (50)     NOT NULL,
    [Type]          INT              NOT NULL,
    [DateDonated]   DATETIME         NOT NULL,
    [ModifiedDate]  DATETIME         CONSTRAINT [DF_Donation_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]       UNIQUEIDENTIFIER CONSTRAINT [DF_Donation_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Donation] PRIMARY KEY CLUSTERED ([DonationID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[Institution].[Donation] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Donation] TO [Deputy]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[Donation] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Donation] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Donation] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[Donation] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Donation] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Donation] TO [Deputy]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[Donation] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Donation] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Donation] TO [Deputy]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[Donation] TO [Accounts]
    AS [dbo];

