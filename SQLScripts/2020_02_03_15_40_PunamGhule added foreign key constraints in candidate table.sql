ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_gender_id_fkey FOREIGN KEY (gender_id)
    REFERENCES public.gender (id) ;
	
ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_religion_id_fkey FOREIGN KEY (religion_id)
    REFERENCES public.religion (id) ;	
	
ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_caste_id_fkey FOREIGN KEY (caste_id)
    REFERENCES public.caste (id) ;

ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_sub_caste_id_fkey FOREIGN KEY (sub_caste_id)
    REFERENCES public.sub_caste (id) ;
	
ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_education_id_fkey FOREIGN KEY (education_id)
    REFERENCES public.highest_education (id) ;
	
	
ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_color_id_fkey FOREIGN KEY (color_complexion_id)
    REFERENCES public.color_complexion (id) ;
	
ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_family_type_id_fkey FOREIGN KEY (family_type_id)
    REFERENCES public.family_type (id) ;
	
	
	
	