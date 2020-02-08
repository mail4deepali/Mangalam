INSERT INTO public.country(
	id, country_name, country_code)
	VALUES (1, 'INDIA', '+91');
	
INSERT INTO public.state(
	id, state_name, state_description, country_id)
	VALUES (1, 'MAHARASHTRA', '', 1),
	       (2, 'KARNATAK', '', 1),
		   (3, 'GUJRAT', '', 1),
		   (4, 'GAO', '', 1);
		   
INSERT INTO public.district(
	id, district_name, district_description, state_id)
	VALUES (1, 'KOLHAPUR', '', 1),
	       (2, 'NASHIK', '', 1),
	       (3, 'PUNE', '', 1),
	       (4, 'MUMBAI', '', 1),
	       (5, 'AHMEDNAGAR', '', 1);
		   
CREATE TABLE taluka (
   id integer primary key,
   taluka_name varchar(500),
   district_id smallint  REFERENCES district (id)
) ;
		   
INSERT INTO public.taluka(
	id, taluka_name , district_id)
	VALUES (1, 'SHIROL', 1),
	       (2, 'KAGAL', 1),
	       (3, 'DINDORI', 2),
	       (4, 'SINNER', 2),
	       (5, 'DAUND', 3);

	
INSERT INTO public.religion(
	id, religion_name, religion_description)
	VALUES (1, 'HINDU', ''),
	       (2, 'MUSLIM', ''),
		   (3, 'CHRIST', '');
		   
	
INSERT INTO public.highest_education(
	id, education_degree,education_field_description)
	VALUES (1, 'BE/B.Tech',''),
	       (2, 'BSC',''),
		   (3, 'MSC',''),
		   (4, 'BHMS',''),
		   (5, 'M.COM','');
		   
INSERT INTO public.language(
	id, name)
	VALUES (1, 'Hindi'),
	       (2, 'English'),
		   (3, 'Marathi'),
		   (4, 'Urdu');
	
INSERT INTO public.gender(
	id, gender, gender_description)
	VALUES (1, 'Male', ''),
	       (2, 'Female', '');
	
INSERT INTO public.family_type(
	id, family_type, family_type_description)
	VALUES (1, 'Joint', ''),
	       (2, 'Nuclear', '');
	

		   
		   
		   
		   