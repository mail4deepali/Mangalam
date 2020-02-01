CREATE TABLE sub_caste (
   subcasteID integer primary key,
   casteID integer REFERENCES caste(casteid),
   subcaste_name varchar(500)	
)