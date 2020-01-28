-- Table: public.run_scripts

-- DROP TABLE public.run_scripts;

CREATE TABLE public.run_scripts
(
    id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    name character varying(2000) COLLATE pg_catalog."default",
    date_run timestamp with time zone,
    CONSTRAINT run_scripts_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.run_scripts
    OWNER to postgres;
COMMENT ON TABLE public.run_scripts
    IS 'table to save already run scripts on database';
	
	
INSERT INTO public.run_scripts
(name, date_run)
values
('2020_01_23_17_52_Create table run_scripts.sql', NOW());