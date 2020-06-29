CREATE TABLE public.marital_status
(
    id smallint NOT NULL,
    marital_status text NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE public.candidate
    ADD COLUMN marital_status_id smallint;

ALTER TABLE public.candidate
    ADD CONSTRAINT marital_status_id FOREIGN KEY (marital_status_id)
    REFERENCES public.marital_status (id) ;