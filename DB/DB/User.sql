CREATE ROLE [User]
    AUTHORIZATION [dbo];






GO
GRANT ALTER
    ON ROLE::[User] TO [Principal]
    WITH GRANT OPTION;


GO
GRANT CONTROL
    ON ROLE::[User] TO [Principal]
    WITH GRANT OPTION;


GO
