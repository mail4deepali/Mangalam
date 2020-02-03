ALTER TABLE caste
  RENAME COLUMN casteid TO id;

ALTER TABLE caste 
  DROP COLUMN religionid;
  
ALTER TABLE color_complexion
  RENAME COLUMN colorid TO id;
  
ALTER TABLE country
  RENAME COLUMN countryid TO id;

ALTER TABLE district
  RENAME COLUMN districtid TO id;
  

ALTER TABLE family_type
  RENAME COLUMN family_typeid TO id;
  
ALTER TABLE gender
  RENAME COLUMN genderid TO id;
  
ALTER TABLE highest_education
  RENAME COLUMN educationid TO id;

ALTER TABLE religion
  RENAME COLUMN religionid TO id;  

ALTER TABLE state
  RENAME COLUMN stateid TO id;
  
ALTER TABLE sub_caste
  RENAME COLUMN subcasteid TO id;  

  
ALTER TABLE user_role
  RENAME COLUMN roleid TO id;
  

alter table district 
drop column stateid;

alter table state 
drop column countryid;

alter table sub_caste 
drop column casteid;

alter table user_table 
drop column roleid;
