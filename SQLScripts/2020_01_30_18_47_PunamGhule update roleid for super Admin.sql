update user_table 
set roleid = (select roleid from user_role where user_role = 'Super Admin') where id = 1