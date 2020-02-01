CREATE TABLE state (
   stateID integer primary key,
   countryID integer,
   state_name varchar(500),
   state_description varchar(500)
) ;

ALTER TABLE state 
ADD CONSTRAINT country_key FOREIGN KEY (countryID) REFERENCES country(countryID);