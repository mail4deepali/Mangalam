
	
ALTER TABLE public.address
    DROP COLUMN taluka;
	
ALTER TABLE public.address
    ADD COLUMN taluka_id smallint REFERENCES public.taluka(id);