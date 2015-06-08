CREATE FUNCTION [dbo].[GetWeightedExamSubjectScore](@studentID int, @examID int, @subjectID int,@weight decimal)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @score decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 IF @weight=0
 RETURN 0
 ELSE
 
    set @score = (SELECT erd.Score FROM [Institution].[ExamResultDetail] erd LEFT OUTER JOIN [Institution].[ExamResultHeader] erh
 ON(erh.ExamResultID= erd.ExamResultID) WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1 AND erd.SubjectID=@subjectID)
    
 IF  NOT @score IS NULL
 SET @score= @score*(@weight/(SELECT OutOf FROM [Institution].[ExamHeader] WHERE ExamID=@examID))
 RETURN @score;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamSubjectScore] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamSubjectScore] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamSubjectScore] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamSubjectScore] TO [Teacher]
    AS [dbo];

