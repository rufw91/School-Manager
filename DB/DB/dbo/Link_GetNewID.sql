CREATE FUNCTION [dbo].[Link_GetNewID](@nameOfTable varchar(max))
    RETURNS int
    AS
    BEGIN

    RETURN dbo.GetNewID(@nameOfTable);
    END
    ;



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Link_GetNewID] TO [Teacher]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Link_GetNewID] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Link_GetNewID] TO [Deputy]
    AS [dbo];

