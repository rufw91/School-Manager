CREATE FUNCTION [dbo].[GetTermOverAllPosition](@studentID int,@form varchar(2),@startDateTime datetime,@endDateTime datetime)
    RETURNS varchar(10)
    AS
    BEGIN
    DECLARE @pos varchar(50);

    set @pos = (SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),studs) FROM(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC)
  row_no, res.StudentID, (SELECT COUNT(*) FROM [Institution].[Student] st LEFT OUTER JOIN [Institution].[Class] cl 
  ON(st.ClassID=cl.ClassID) WHERE cl.NameOfClass LIKE '%'+@form+'%') studs FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN 
  (SELECT s.StudentID,erh.ExamResultID FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[ExamResultHeader] erh 
  ON (s.StudentID=erh.StudentID) LEFT OUTER JOIN [Institution].[ExamHeader] e 
  ON(e.ExamID=erh.ExamID) LEFT OUTER JOIN [Institution].[ExamClassDetail] ecd 
  ON(ecd.ExamID = e.ExamID) LEFT OUTER JOIN (SELECT NameOfClass,ClassID FROM [Institution].[Class] WHERE NameOfClass LIKE '%'+@form+'%') fc
  ON(ecd.ClassID=fc.ClassID)
  WHERE s.IsActive=1 AND erh.IsActive=1 AND fc.NameOfClass LIKE '%'+@form+'%' AND 
  e.ExamDatetime>=@startDateTime AND e.ExamDatetime<=@endDateTime ) res 
  ON (erd.ExamResultID=res.ExamResultID) GROUP BY res.StudentID)x WHERE x.StudentID=@studentID)

    RETURN @pos
    END
    ;



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermOverAllPosition] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermOverAllPosition] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermOverAllPosition] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTermOverAllPosition] TO [Teacher]
    AS [dbo];

