
ALTER TABLE public.state
    ADD COLUMN country_id smallint;
	
ALTER TABLE public.state
    ADD CONSTRAINT state_country_id_fkey FOREIGN KEY (country_id)
    REFERENCES public.country (id) ;
	

ALTER TABLE public.district
    ADD COLUMN state_id smallint;
	
ALTER TABLE public.district
    ADD CONSTRAINT district_state_id_fkey FOREIGN KEY (state_id)
    REFERENCES public.state (id) ;
	

ALTER TABLE public.sub_caste
    ADD COLUMN caste_id smallint;
	
ALTER TABLE public.sub_caste
    ADD CONSTRAINT sub_caste_caste_id_fkey FOREIGN KEY (caste_id)
    REFERENCES public.caste (id) ;