
alter table user_table 
add column  user_name character varying(300) COLLATE pg_catalog."default" NULL;

update  user_table
set user_name = 'Vinayak'
where id = 1;