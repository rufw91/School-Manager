CREATE FUNCTION [dbo].[GetExamTotalScore](@studentID int, @examID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 ELSE
    set @total = (SELECT SUM(ISNULL(erd.Score,0)) FROM [Institution].[ExamResultHeader] erh
 LEFT OUTER JOIN [Institution].[ExamResultDetail] erd ON(erh.ExamResultID= erd.ExamResultID) WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1)
    
 RETURN @total;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamTotalScore] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamTotalScore] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamTotalScore] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExamTotalScore] TO [Teacher]
    AS [dbo];

