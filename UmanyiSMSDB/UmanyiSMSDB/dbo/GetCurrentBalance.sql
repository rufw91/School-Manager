CREATE FUNCTION [dbo].[GetCurrentBalance](@studentID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @currentBl decimal;


 DECLARE  @sal decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,TotalAmt),0)) FROM  [Sales].[SaleHeader] WHERE CustomerID =@studentID);
DECLARE  @pur decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,AmountPaid),0)) FROM  [Institution].[FeesPayment] WHERE StudentID =@studentID);
DECLARE  @prev decimal=(SELECT CONVERT(DECIMAL,PreviousBalance) FROM  [Institution].[Student] WHERE StudentID=@studentID)
SET @currentBl=(select (ISNULL(@sal,0)+ISNULL(@prev,0))-ISNULL(@pur,0));

 RETURN @currentBl
    END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentBalance] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentBalance] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentBalance] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentBalance] TO [Teacher]
    AS [dbo];

