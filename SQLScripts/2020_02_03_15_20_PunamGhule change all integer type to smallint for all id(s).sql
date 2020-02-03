

ALTER TABLE public.candidate
    ALTER COLUMN gender_id TYPE smallint;
	
ALTER TABLE public.candidate
    ALTER COLUMN religion_id TYPE smallint;
	
ALTER TABLE public.candidate
    ALTER COLUMN caste_id TYPE smallint;
	
ALTER TABLE public.candidate
    ALTER COLUMN sub_caste_id TYPE smallint;

ALTER TABLE public.candidate
    ALTER COLUMN education_id TYPE smallint;
	
ALTER TABLE public.candidate
    ALTER COLUMN color_complexion_id TYPE smallint;

ALTER TABLE public.candidate
    ALTER COLUMN family_type_id TYPE smallint;
	
ALTER TABLE public.color_complexion
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.caste
    ALTER COLUMN id TYPE smallint;

ALTER TABLE public.country
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.district
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.family_type
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.gender
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.highest_education
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.religion
    ALTER COLUMN id TYPE smallint;

ALTER TABLE public.state
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.sub_caste
    ALTER COLUMN id TYPE smallint;
	
ALTER TABLE public.user_role
    ALTER COLUMN id TYPE smallint;


	
