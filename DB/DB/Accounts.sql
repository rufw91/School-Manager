CREATE ROLE [Accounts]
    AUTHORIZATION [dbo];


GO
GRANT ALTER
    ON ROLE::[Accounts] TO [Principal]
    WITH GRANT OPTION;


GO
GRANT CONTROL
    ON ROLE::[Accounts] TO [Principal]
    WITH GRANT OPTION;

