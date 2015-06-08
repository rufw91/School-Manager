CREATE FUNCTION [dbo].[GetUnreturnedCopies](@bookID int)
    RETURNS int
    AS
    BEGIN
 DECLARE @unreturned int=(SELECT COUNT(*) FROM ((SELECT bid.BookID FROM [Institution].[BookIssueDetail] bid INNER JOIN [Institution].[BookIssueHeader] bih ON(bid.BookIssueID=bih.BookIssueID) WHERE NOT EXISTS(SELECT brd.BookID FROM [Institution].[BookReturnDetail] brd INNER JOIN [Institution].[BookReturnHeader] brh ON(brd.BookReturnID=brh.BookReturnID) WHERE brh.DateReturned>bih.DateIssued AND brd.BookID=bid.BookID)) x LEFT OUTER JOIN [Institution].[Book] b ON (x.BookID=b.BookID)) WHERE x.BookID=@bookID);
 
 RETURN @unreturned
    END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnreturnedCopies] TO [Accounts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnreturnedCopies] TO [Deputy]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnreturnedCopies] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnreturnedCopies] TO [Teacher]
    AS [dbo];

