-- Table: public.user_table

-- DROP TABLE public.user_table;
 
CREATE TABLE public.user_table
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    first_name character varying(300) COLLATE pg_catalog."default" NOT NULL,
    last_name character varying(300) COLLATE pg_catalog."default" NOT NULL,
    phone_number character varying(12) COLLATE pg_catalog."default" NOT NULL,
    alternate_phone_number character varying(12) COLLATE pg_catalog."default",
    password character varying(500) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT user_table_id PRIMARY KEY (id),
    CONSTRAINT user_table_phone_number_unique UNIQUE (phone_number)
)

TABLESPACE pg_default;

ALTER TABLE public.user_table
    OWNER to postgres;