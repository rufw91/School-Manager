
CREATE FUNCTION [dbo].[GetNewID](@nameOfTable varchar(max))
    RETURNS int
    AS
    BEGIN

    DECLARE @lastID int
 if (exists(SELECT [last_id] FROM [dbo].[sysids] where UPPER(table_name) = UPPER(@nameOfTable)))
    set @lastID = (SELECT [last_id]+1 FROM [dbo].[sysids] where UPPER(table_name) = UPPER(@nameOfTable))

    RETURN @lastID
    END
    ;



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNewID] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNewID] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNewID] TO [Principal]
    AS [dbo];


GO
GRANT CONTROL
    ON OBJECT::[dbo].[GetNewID] TO [SystemAdmin]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNewID] TO [Teacher]
    AS [dbo];

