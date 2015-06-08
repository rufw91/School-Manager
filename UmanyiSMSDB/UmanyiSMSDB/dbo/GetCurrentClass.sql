CREATE FUNCTION [dbo].[GetCurrentClass](@studentID int)
    RETURNS int
    AS
    BEGIN
 DECLARE @currentClassID int;
 IF NOT EXISTS (SELECT ClassID FROM [Institution].[CurrentClass] WHERE StudentID=@studentID AND IsActive=1)
 RETURN 0
 ELSE
    set @currentClassID = (SELECT ClassID FROM [Institution].[CurrentClass] WHERE StudentID=@studentID AND IsActive=1);
    
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

