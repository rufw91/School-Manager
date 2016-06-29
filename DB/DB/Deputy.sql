CREATE ROLE [Deputy]
    AUTHORIZATION [dbo];




GO
GRANT ALTER
    ON ROLE::[Deputy] TO [Principal]
    WITH GRANT OPTION;


GO
GRANT CONTROL
    ON ROLE::[Deputy] TO [Principal]
    WITH GRANT OPTION;


GO