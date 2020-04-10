
update user_table set user_name = 'mladmin' 
where id = 1;

update user_table
set role_id = 1
where id = 1 and user_name = 'mladmin';