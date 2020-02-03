CREATE TABLE public.address
(
    id smallint primary key,
    address_line_1 text,
    address_line_2 text,
    taluka text,
    city text,
    country_id smallint,
    state_id smallint,
    district_id smallint
);


ALTER TABLE public.address
    ADD CONSTRAINT address_country_id_fkey FOREIGN KEY (country_id)
    REFERENCES public.country (id);
	
ALTER TABLE public.address
    ADD CONSTRAINT address_state_id_fkey FOREIGN KEY (state_id)
    REFERENCES public.state (id);
	

ALTER TABLE public.address
    ADD CONSTRAINT address_district_id_fkey FOREIGN KEY (district_id)
    REFERENCES public.district (id);