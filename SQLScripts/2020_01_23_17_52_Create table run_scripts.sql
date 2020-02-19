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

DO
$do$
BEGIN
   IF NOT EXISTS (
      SELECT                       
      FROM   pg_catalog.pg_roles
      WHERE  rolname = 'mangalam_app') THEN

	CREATE ROLE mangalam_app WITH
		  LOGIN
		  NOSUPERUSER
		  INHERIT
		  NOCREATEDB
		  NOCREATEROLE
		  NOREPLICATION
		  ENCRYPTED PASSWORD 'md57966a42f1514899e7bbb9a0708d86bb6';

		COMMENT ON ROLE mangalam_app IS 'user for mangalam app';
   END IF;
END
$do$;

GRANT SELECT, INSERT, UPDATE ON ALL TABLES IN SCHEMA public TO mangalam_app;



