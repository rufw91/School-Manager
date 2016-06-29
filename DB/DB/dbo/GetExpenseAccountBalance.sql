CREATE FUNCTION [dbo].[GetExpenseAccountBalance](@accountID int, @startDateTime datetime, @endDateTime datetime)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @currentBl decimal,@payroll decimal;
 
 SET @payroll =(SELECT ISNULL(SUM(AmountPaid),0) FROM [Institution].[PayslipHeader] WHERE DatePaid BETWEEN @startDateTime AND @endDateTime);

WITH AccountsCTE(AccountID,ParentAccountID, Balance ,lvl)
AS
(
SELECT ic.ItemCategoryID,ic.ParentCategoryID,ISNULL(CONVERT(decimal,SUM(ird.LineTotal)),0),0
FROM Sales.ItemCategory ic
LEFT OUTER JOIN [Sales].[Item] i 
ON (ic.ItemCategoryID= i.ItemCategoryID)
LEFT OUTER JOIN [Sales].[ItemReceiptDetail] ird 
ON (ird.ItemID = i.ItemID)
LEFT OUTER JOIN [Sales].[ItemReceiptHeader] irh
ON (irh.ItemReceiptID = ird.ItemReceiptID AND irh.OrderDate BETWEEN @startDateTime AND @endDateTime)
WHERE ic.ItemCategoryID =@accountID 
GROUP BY ic.ItemCategoryID,ic.ParentCategoryID
UNION ALL
SELECT P.ItemCategoryID,p.ParentCategoryID,ISNULL(ird.LineTotal,0) ,PP.lvl+1
FROM Sales.ItemCategory as P
 JOIN AccountsCTE as PP
ON (P.ParentCategoryID = PP.AccountID) 
 JOIN [Sales].[Item] i 
ON (p.ItemCategoryID= i.ItemCategoryID)
 JOIN [Sales].[ItemReceiptDetail] ird 
ON (ird.ItemID = i.ItemID)
JOIN [Sales].[ItemReceiptHeader] irh
ON (irh.ItemReceiptID = ird.ItemReceiptID AND irh.OrderDate BETWEEN @startDateTime AND @endDateTime)

)
SELECT @currentBl= ISNULL(SUM(ISNULL(Balance,0)),0) FROM AccountsCTE

IF EXISTS(SELECT * FROM [Sales].[ItemCategory] WHERE ItemCategoryID=@accountID AND Description='PAYROLL EXPENSES')
SET @currentBl = @payroll
IF EXISTS(SELECT * FROM [Sales].[ItemCategory] WHERE ItemCategoryID=@accountID AND Description='EXPENSES')
 SET @currentBl = @currentBl+@payroll

 RETURN ISNULL(@currentBl,0)
    END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExpenseAccountBalance] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExpenseAccountBalance] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExpenseAccountBalance] TO [Accounts]
    AS [dbo];

