CREATE TABLE [Institution].[PayoutDetail] (
    [PayoutDetailID] INT              CONSTRAINT [DF_PayoutDetail_PayoutDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.PayoutDetail')) NOT NULL,
    [PayoutID]       INT              NOT NULL,
    [Description]    VARCHAR (50)     NOT NULL,
    [DatePaid]       DATETIME         NOT NULL,
    [Amount]         VARCHAR (50)     NOT NULL,
    [ModifiedDate]   DATETIME         CONSTRAINT [DF_PayoutDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_PayoutDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_PayoutDetail] PRIMARY KEY CLUSTERED ([PayoutDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_PayoutDetail_UpdateID]
 ON [Institution].[PayoutDetail] AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.PayoutDetail')
    END
   END
END




GO
GRANT DELETE
    ON OBJECT::[Institution].[PayoutDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[PayoutDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[PayoutDetail] TO [Accounts]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[PayoutDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[PayoutDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[PayoutDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[PayoutDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[PayoutDetail] TO [Principal]
    AS [dbo];

