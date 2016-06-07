CREATE FUNCTION [dbo].[AddValuesIgnoringNull](@score1 decimal,@score2 decimal,@score3 decimal,@score4 decimal,@score5 decimal)
    RETURNS decimal
    AS
    BEGIN
    DECLARE @total decimal;

    set @total = NULL;

 if NOT @score1 IS NULL
 set @total = @score1;
 if NOT @score2 IS NULL
 set @total = @total+@score2;
   if NOT @score3 IS NULL
 set @total = @total+@score3;
 if NOT @score4 IS NULL
 set @total = @total+@score4;
 if NOT @score5 IS NULL
 set @total = @total+@score5;

    RETURN @total
    END
    ;



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddValuesIgnoringNull] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddValuesIgnoringNull] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddValuesIgnoringNull] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddValuesIgnoringNull] TO [Teacher]
    AS [dbo];

