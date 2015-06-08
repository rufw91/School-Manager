CREATE FUNCTION [dbo].[GetStudentIsActive](@studentID int)
    RETURNS bit
    AS
    BEGIN
 IF EXISTS (SELECT * FROM [Institution].[StudentClearance] WHERE StudentID=@studentID)
 RETURN 0;
  IF EXISTS (SELECT * FROM [Institution].[StudentTransfer] WHERE StudentID=@studentID)
 RETURN 0;    
 RETURN 1;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStudentIsActive] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStudentIsActive] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStudentIsActive] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStudentIsActive] TO [Teacher]
    AS [dbo];

