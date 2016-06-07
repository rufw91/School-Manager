CREATE FUNCTION [dbo].[GetExamSubjectScore](@studentID int, @examID int, @subjectID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @score decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 ELSE
    set @score = (SELECT erd.Score FROM [Institution].[ExamResultDetail] erd LEFT OUTER JOIN [Institution].[ExamResultHeader] erh
 ON(erh.ExamResultID= erd.ExamResultID) WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1 AND erd.SubjectID=@subjectID)
    
 RETURN @score;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamSubjectScore] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamSubjectScore] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamSubjectScore] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamSubjectScore] TO [Teacher]
    AS [dbo];

