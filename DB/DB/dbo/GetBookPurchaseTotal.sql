Create FUNCTION [dbo].[GetBookPurchaseTotal](@bookPurchaseID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Sales].[BookReceiptDetail] WHERE BookReceiptID=@bookPurchaseID)
 RETURN 0
 ELSE
    set @total = (SELECT SUM(LineTotal) FROM [Sales].[BookReceiptDetail] WHERE BookReceiptID=@bookPurchaseID);
    
 RETURN @total;
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBookPurchaseTotal] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBookPurchaseTotal] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBookPurchaseTotal] TO [Accounts]
    AS [dbo];

