CREATE ROLE [Teacher]
    AUTHORIZATION [dbo];




GO
GRANT ALTER
    ON ROLE::[Teacher] TO [Principal]
    WITH GRANT OPTION;


GO
GRANT CONTROL
    ON ROLE::[Teacher] TO [Principal]
    WITH GRANT OPTION;


GO