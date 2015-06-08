CREATE FUNCTION [dbo].[GetCurrentQuantity](@itemID bigint)
    RETURNS decimal
    AS
    BEGIN

    DECLARE @startQuantity decimal;
 DECLARE @totalSold decimal;
 DECLARE @totalBought decimal;
 DECLARE @currentQty decimal;
 IF NOT EXISTS (SELECT * FROM [Sales].[Item] WHERE ItemID=@itemID)
 RETURN 0
 ELSE
 BEGIN 
    set @startQuantity = ISNULL((SELECT StartQuantity FROM [Sales].[Item] where ItemID = @itemID),0)
 set @totalSold = ISNULL((SELECT SUM(Quantity) FROM [Sales].[ItemIssueDetail] where ItemID = @itemID),0)
 set @totalBought = ISNULL((SELECT SUM(Quantity) FROM [Sales].[ItemReceiptDetail] where ItemID = @itemID),0)

    set @currentQty=@startQuantity+@totalBought-@totalSold;
    END
 RETURN @currentQty
    END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentQuantity] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentQuantity] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentQuantity] TO [Principal]
    AS [dbo];


GO
GRANT CONTROL
    ON OBJECT::[dbo].[GetCurrentQuantity] TO [SystemAdmin]
    AS [dbo];

