ALTER TABLE public.candidate
    ADD COLUMN address_id smallint;

ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_address_id_fkey FOREIGN KEY (address_id)
    REFERENCES public.address (id);
	
ALTER TABLE public.user_table
    ADD COLUMN address_id smallint;

ALTER TABLE public.user_table
    ADD CONSTRAINT user_table_address_id_fkey FOREIGN KEY (address_id)
    REFERENCES public.address (id);