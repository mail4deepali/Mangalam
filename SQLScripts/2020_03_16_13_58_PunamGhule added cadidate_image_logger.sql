CREATE TABLE public.candidate_image_logger
(
    id integer,
    user_id integer NOT NULL,
    candidate_id smallint NOT NULL,
    image_name text NOT NULL,
    image_path text NOT NULL,
    content_type text,
    image_upload_time date,
    PRIMARY KEY (id)
);

ALTER TABLE public.candidate_image_logger
    OWNER to postgres;

ALTER TABLE public.candidate_image_logger
    ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 );

GRANT SELECT, INSERT, UPDATE ON ALL TABLES IN SCHEMA public TO mangalam_app;



