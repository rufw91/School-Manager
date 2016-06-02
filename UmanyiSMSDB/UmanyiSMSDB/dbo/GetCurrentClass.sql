CREATE FUNCTION [dbo].[GetCurrentClass](@studentID int, @period datetime)
    RETURNS int
    AS
    BEGIN
 DECLARE @currentClassID int;
 IF NOT EXISTS (SELECT ClassID FROM [Institution].[CurrentClass] WHERE StudentID=@studentID AND StartDateTime<@period AND EndDateTime>@period)
 RETURN 0
 ELSE
    set @currentClassID = (SELECT ClassID FROM [Institution].[CurrentClass] WHERE StudentID=@studentID AND StartDateTime<@period AND EndDateTime>@period);
    
 RETURN @currentClassID;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentClass] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentClass] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentClass] TO [Principal]
    AS [dbo];


GO
GRANT CONTROL
    ON OBJECT::[dbo].[GetCurrentClass] TO [SystemAdmin]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentClass] TO [Teacher]
    AS [dbo];

