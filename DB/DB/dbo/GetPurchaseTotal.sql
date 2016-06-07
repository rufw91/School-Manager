CREATE FUNCTION [dbo].[GetPurchaseTotal](@purchaseID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemReceiptDetail] WHERE ItemReceiptID=@purchaseID)
 RETURN 0
 ELSE
    set @total = (SELECT SUM(LineTotal) FROM [Sales].[ItemReceiptDetail] WHERE ItemReceiptID=@purchaseID);
    
 RETURN @total;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseTotal] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseTotal] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseTotal] TO [Principal]
    AS [dbo];

