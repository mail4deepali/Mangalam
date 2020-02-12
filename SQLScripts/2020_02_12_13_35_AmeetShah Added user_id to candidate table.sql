Alter table public.candidate
Add column user_id integer not null;

ALTER TABLE public.candidate
    ADD CONSTRAINT candidate_user_table_user_id_id FOREIGN KEY (user_id)
    REFERENCES public.user_table (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;