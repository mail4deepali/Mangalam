alter table user_table
add user_name varchar(500) not null;


Insert into user_table
(user_name, first_name, last_name, phone_number, alternate_phone_number, password)
values
('mladmin','Vinayak', 'Jondhale', '9850762309', null, 'f78d959290c92c50bcce1a83dddaafbd7f5fe01b696a9e904c3fa37febea71d1')

Select * from user_table