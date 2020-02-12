ALTER TABLE public.user_table
    ADD COLUMN role_id smallint REFERENCES user_role(id)