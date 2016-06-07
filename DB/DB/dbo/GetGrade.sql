CREATE FUNCTION [dbo].[GetGrade](@score decimal)
    RETURNS varchar(2)
    AS
    BEGIN
    DECLARE @grade varchar(2)

    set @grade = (SELECT 
   CASE 
     WHEN (@score >= 80) AND( @score <= 100) THEN 'A'

            WHEN (@score >= 75 AND @score <= 79)
                THEN 'A-'
            WHEN (@score >= 70 AND @score <= 74)
                THEN 'B+'
            WHEN (@score >= 65 AND @score <= 69)
                THEN 'B'
            WHEN (@score >= 60 AND @score <= 64)
                THEN 'B-'
            WHEN (@score >= 55 AND @score <= 59)
                THEN 'C+'
            WHEN (@score >= 50 AND @score <= 54)
                THEN 'C'
            WHEN (@score >= 45 AND @score <= 49)
                THEN 'C-'
            WHEN (@score >= 40 AND @score <= 44)
                THEN 'D+'
            WHEN (@score >= 35 AND @score <= 39)
                THEN 'D'
            WHEN (@score >= 30 AND @score <= 34)
                THEN 'D-'
            WHEN (@score >= 0 AND @score <= 29)
                THEN 'E' 
   WHEN (@score < 0 or @score > 100)
                THEN '-' 
   END)

    RETURN @grade
    END
    ;



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGrade] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGrade] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGrade] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGrade] TO [Teacher]
    AS [dbo];

