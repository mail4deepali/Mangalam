
ALTER TABLE public.user_table
DROP COLUMN "IsUserloginAfterRegistration";


ALTER TABLE public.user_table
    ADD COLUMN is_user_login_first_time boolean;



	