CREATE FUNCTION [dbo].[GetCurrentSupplierBalance](@supplierID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @currentBl decimal;


 DECLARE  @pur1 decimal=(SELECT SUM(ISNULL(TotalAmt,0)) FROM  [Sales].[ItemReceiptHeader] WHERE SupplierID =@supplierID);
DECLARE  @pur2 decimal=(SELECT SUM(ISNULL(TotalAmt,0)) FROM  [Sales].[BookReceiptHeader] WHERE SupplierID =@supplierID);
DECLARE  @pay decimal=(SELECT SUM(ISNULL(AmountPaid,0)) FROM  [Sales].[SupplierPayment] WHERE SupplierID=@supplierID)
SET @currentBl=(select (ISNULL(@pur1,0)+ISNULL(@pur2,0))-ISNULL(@pay,0));

 RETURN @currentBl
    END