CREATE ROLE [None]
    AUTHORIZATION [dbo];




GO
GRANT ALTER
    ON ROLE::[None] TO [Principal]
    WITH GRANT OPTION;


GO
GRANT CONTROL
    ON ROLE::[None] TO [Principal]
    WITH GRANT OPTION;


GO
EXECUTE sp_addrolemember @rolename = N'None', @membername = N'3';

