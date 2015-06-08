CREATE FUNCTION [dbo].[GetSubjectsTakenByStudent](@studentSubjectSelectionID int)
    RETURNS int
    AS
    BEGIN

    DECLARE @noOfSubjects int;
 IF NOT EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@studentSubjectSelectionID)
 RETURN 0
 ELSE
 BEGIN 
    set @noOfSubjects = (SELECT COUNT(*) FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@studentSubjectSelectionID)
 END
 RETURN @noOfSubjects
    END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectsTakenByStudent] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectsTakenByStudent] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectsTakenByStudent] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectsTakenByStudent] TO [Teacher]
    AS [dbo];

