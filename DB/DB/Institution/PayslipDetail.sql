CREATE TABLE [Institution].[PayslipDetail] (
    [PayslipDetailID] INT              CONSTRAINT [DF_PayslipDetail_PayslipDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.PayslipDetail')) NOT NULL,
    [PayslipID]       INT              NOT NULL,
    [Description]     VARCHAR (50)     NOT NULL,
    [Amount]          DECIMAL (18)     NOT NULL,
    [ModifiedDate]    DATETIME         CONSTRAINT [DF_PayslipDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_PayslipDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_PayslipDetail] PRIMARY KEY CLUSTERED ([PayslipDetailID] ASC)
);




GO
CREATE TRIGGER [Institution].[TR_PayslipDetail_UpdateID]
 ON [Institution].[PayslipDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.PayslipDetail')
    END
   END
END
GO
GRANT UPDATE
    ON OBJECT::[Institution].[PayslipDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[PayslipDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[PayslipDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[PayslipDetail] TO [Deputy]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[PayslipDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[PayslipDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[PayslipDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[PayslipDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[PayslipDetail] TO [Accounts]
    AS [dbo];

