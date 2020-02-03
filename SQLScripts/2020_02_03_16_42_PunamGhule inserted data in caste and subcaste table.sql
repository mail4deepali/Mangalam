INSERT INTO public.caste(
	id, caste_name, caste_description)
	VALUES (1, 'maratha', ''),
	       (2, 'Shimpi', ''),
		   (3, 'Matang', ''),
		   (4, 'Vanjari', ''),
		   (5, 'Dhangar', ''),
		   (6, 'Chambhar', ''),
		   (7, 'Mahar', ''),
		   (8, 'Sutar', ''),
		   (9, 'Parit', ''),
		   (10, 'Jain', ''),
		   (11, 'Bhoi', ''),
		   (12, 'Bramhan', '');
		   
INSERT INTO public.sub_caste(
	id, subcaste_name, caste_id)
	VALUES (1, 'Maratha 96 kuli' , 1),
	       (2, 'Maratha' , 1),
		   (3, 'Ladjin Vanjari' , 4),
		   (4, 'Raojin Vanjari' , 4),		   
		   (5, 'Mathurjin Vanjari' , 4),	   
		   (6, 'Bhusarjin Vanjari' , 4)
		   
		   