CREATE TABLE user_candidate_map (
   id smallint primary key,
   user_id smallint,
   candidate_id smallint
) ;

ALTER TABLE public.user_candidate_map
    ADD CONSTRAINT user_user_id_map_fkey FOREIGN KEY (user_id)
    REFERENCES public.user_table (id);

ALTER TABLE public.user_candidate_map
    ADD CONSTRAINT user_candidate_id_map_fkey FOREIGN KEY (candidate_id)
    REFERENCES public.candidate (id);
