CREATE FUNCTION [dbo].[GetWeightedExamTotalScore](@studentID int, @examID int,@weight decimal)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 ELSE
    set @total = (SELECT SUM(ISNULL(erd.Score,0)) FROM [Institution].[ExamResultHeader] erh
 LEFT OUTER JOIN [Institution].[ExamResultDetail] erd ON(erh.ExamResultID= erd.ExamResultID) 
 WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1)
    
 IF  NOT @total IS NULL
 SET @total= @total*(@weight/(SELECT OutOf FROM [Institution].[ExamHeader] WHERE ExamID=@examID))
 RETURN @total;
 END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamTotalScore] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamTotalScore] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamTotalScore] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWeightedExamTotalScore] TO [Teacher]
    AS [dbo];

