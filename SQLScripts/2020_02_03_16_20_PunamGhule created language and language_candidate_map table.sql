
CREATE TABLE public.language
(
    id smallint primary key,
    name text   
);

CREATE TABLE public.candidate_language_map
(
    id smallint primary key,
    candidate_id smallint,
	language_id smallint
);


ALTER TABLE public.candidate_language_map
    ADD CONSTRAINT candidate_language_map_candidate_id_fkey FOREIGN KEY (candidate_id)
    REFERENCES public.candidate(id);
	
ALTER TABLE public.candidate_language_map
    ADD CONSTRAINT candidate_language_map_language_id_fkey FOREIGN KEY (language_id)
    REFERENCES public.language (id);
	
