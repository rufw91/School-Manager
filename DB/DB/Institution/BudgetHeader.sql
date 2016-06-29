CREATE TABLE [Institution].[BudgetHeader] (
    [BudgetID]     INT              NOT NULL,
    [StartDate]    DATETIME         NOT NULL,
    [EndDate]      DATETIME         NOT NULL,
    [TotalBudget]  DECIMAL (18)     NOT NULL,
    [ModifiedDate] DATETIME         CONSTRAINT [DF_BudgetHeader_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_BudgetHeader_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BudgetHeader] PRIMARY KEY CLUSTERED ([BudgetID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_BudgetHeader_UpdateID]
 ON Institution.BudgetHeader AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BudgetHeader')
    END
   END
END
GO
GRANT UPDATE
    ON OBJECT::[Institution].[BudgetHeader] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BudgetHeader] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BudgetHeader] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BudgetHeader] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BudgetHeader] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BudgetHeader] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BudgetHeader] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BudgetHeader] TO [Accounts]
    AS [dbo];

