CREATE TABLE district (
   districtID integer primary key,
   stateID integer,
   district_name varchar(500),
   district_description varchar(500)
) ;

ALTER TABLE state 
ADD CONSTRAINT state_key FOREIGN KEY (stateID) REFERENCES state(stateID);