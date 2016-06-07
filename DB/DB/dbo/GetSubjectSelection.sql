CREATE FUNCTION [dbo].[GetSubjectSelection](@subjectID int,@studentID int)
    RETURNS bit
    AS
    BEGIN
 DECLARE @isSelected bit;
 IF EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN
 [Institution].[StudentSubjectSelectionHeader] sssh ON(sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) WHERE 
 sssd.SubjectID=@subjectID AND sssh.StudentID=@studentID)
SET @isSelected=(SELECT 1);
ELSE
SET @isSelected=(SELECT 0);

 RETURN @isSelected
    END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectSelection] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectSelection] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectSelection] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubjectSelection] TO [Teacher]
    AS [dbo];

