CREATE TABLE [Institution].[BudgetAccountDetail] (
    [BudgetAccountDetailID] INT              NOT NULL,
    [BudgetID]              INT              NOT NULL,
    [AccountID]             INT              NOT NULL,
    [BudgetAmount]          DECIMAL (18)     NOT NULL,
    [ModifiedDate]          DATETIME         CONSTRAINT [DF_BudgetAccountDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]               UNIQUEIDENTIFIER CONSTRAINT [DF_BudgetAccountDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BudgetAccountDetail] PRIMARY KEY CLUSTERED ([BudgetAccountDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_BudgetAccountDetail_UpdateID]
 ON Institution.BudgetAccountDetail AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BudgetAccountDetail')
    END
   END
END
GO
GRANT UPDATE
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BudgetAccountDetail] TO [Accounts]
    AS [dbo];

