ALTER TABLE user_table 
ADD COLUMN  roleID integer;


ALTER TABLE user_table 
ADD CONSTRAINT user_role_key FOREIGN KEY (roleID) REFERENCES user_role(roleID);