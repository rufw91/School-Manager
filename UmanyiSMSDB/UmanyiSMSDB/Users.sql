CREATE SCHEMA [Users]
    AUTHORIZATION [dbo];


GO
GRANT CONTROL
    ON SCHEMA::[Users] TO [SystemAdmin];


GO
GRANT DELETE
    ON SCHEMA::[Users] TO [SystemAdmin];


GO
GRANT INSERT
    ON SCHEMA::[Users] TO [SystemAdmin];


GO
GRANT SELECT
    ON SCHEMA::[Users] TO [SystemAdmin];


GO
GRANT UPDATE
    ON SCHEMA::[Users] TO [SystemAdmin];


GO
GRANT VIEW DEFINITION
    ON SCHEMA::[Users] TO [SystemAdmin];

