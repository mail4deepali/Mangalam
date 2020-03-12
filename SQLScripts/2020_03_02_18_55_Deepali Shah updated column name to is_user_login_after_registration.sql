ALTER TABLE user_table 
DROP COLUMN "IsUserloginAfterRegistration"


ALTER TABLE user_table
    ADD COLUMN is_user_login_after_registration boolean;