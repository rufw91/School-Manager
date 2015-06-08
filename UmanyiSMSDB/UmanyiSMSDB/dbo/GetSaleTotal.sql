CREATE FUNCTION [dbo].[GetSaleTotal](@saleID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Sales].[SaleDetail] WHERE SaleID=@saleID)
 RETURN 0
 ELSE
    set @total = (SELECT SUM(Amount) FROM [Sales].[SaleDetail] WHERE SaleID=@saleID);
    
 RETURN @total;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSaleTotal] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSaleTotal] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSaleTotal] TO [Principal]
    AS [dbo];

