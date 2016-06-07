CREATE FUNCTION [dbo].[GetTermClassPosition](@studentID int,@classID int,@startDateTime varchar(50),@endDateTime varchar(50))
    RETURNS varchar(10)
    AS
    BEGIN
    DECLARE @pos varchar(50);

    set @pos = (select CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() 
 OVER(ORDER BY ISNULL(SUM(ISNULL(erd.Score,0)),0) DESC) row_no, 
 res.StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =@classID AND IsActive=1)no_of_students 
 FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN (SELECT s.StudentID,ExamResultID 
 FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[ExamResultHeader] erh 
 ON(s.StudentID=erh.StudentID) LEFT OUTER JOIN [Institution].[Class] c 
 ON (s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[ExamHeader] e 
 ON (erh.ExamID=e.ExamID) LEFT OUTER JOIN [Institution].[ExamClassDetail] ecd 
 ON (ecd.ExamID=e.ExamID) WHERE s.IsActive=1 AND erh.IsActive=1 AND  s.ClassID=@classID AND ecd.ClassID=@classID AND
  e.ExamDatetime>=@startDateTime AND e.ExamDatetime<=@endDateTime
  GROUP BY s.StudentID,erh.ExamResultID )res 
  ON (res.ExamResultID=erd.ExamResultID) 
  GROUP BY res.StudentID )x WHERE x.StudentID=@studentID)

    RETURN @pos
    END
    ;



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermClassPosition] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermClassPosition] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermClassPosition] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermClassPosition] TO [Teacher]
    AS [dbo];

