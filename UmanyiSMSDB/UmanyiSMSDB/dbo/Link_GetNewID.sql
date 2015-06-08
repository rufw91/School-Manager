CREATE FUNCTION [dbo].[Link_GetNewID](@nameOfTable varchar(max))
    RETURNS int
    AS
    BEGIN

    RETURN dbo.GetNewID(@nameOfTable);
    END
    ;


