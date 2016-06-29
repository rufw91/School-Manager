CREATE TABLE [Institution].[BudgetEntryDetail] (
    [BudgetEntryDetailID]   INT              CONSTRAINT [DF_BudgetEntryDetail_BudgetEntryDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.BudgetEntryDetail')) NOT NULL,
    [BudgetAccountDetailID] INT              NOT NULL,
    [ItemID]                INT              NOT NULL,
    [Description]           VARCHAR (50)     NOT NULL,
    [Quantity]              DECIMAL (18)     NOT NULL,
    [Price]                 DECIMAL (18)     NOT NULL,
    [Amount]                AS               ([Quantity]*[Price]),
    [ModifiedDate]          DATETIME         CONSTRAINT [DF_BudgetEntryDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]               UNIQUEIDENTIFIER CONSTRAINT [DF_BudgetEntryDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BudgetEntryDetail] PRIMARY KEY CLUSTERED ([BudgetEntryDetailID] ASC)
);


GO
CREATE TRIGGER [Institution].[TR_BudgetEntryDetail_UpdateID]
 ON Institution.BudgetEntryDetail AFTER INSERT 
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BudgetEntryDetail')
    END
   END
END
GO
GRANT UPDATE
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Principal]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Principal]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Principal]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Principal]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Accounts]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Accounts]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Accounts]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[Institution].[BudgetEntryDetail] TO [Accounts]
    AS [dbo];

