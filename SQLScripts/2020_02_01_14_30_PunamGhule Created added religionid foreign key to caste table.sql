ALTER TABLE caste 
ADD COLUMN  religionID integer  REFERENCES religion(religionid)
