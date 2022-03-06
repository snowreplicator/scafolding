DO $$
    BEGIN

        CREATE SCHEMA IF NOT EXISTS public AUTHORIZATION portal;

        CREATE SEQUENCE if not exists public.horse_horseid_seq
            INCREMENT 1
            START 1
            MINVALUE 1
            MAXVALUE 2147483647
            CACHE 1;

        ALTER SEQUENCE public.horse_horseid_seq OWNER TO portal;

        CREATE TABLE if not exists public.horse (
            horseid integer NOT NULL DEFAULT nextval('public.horse_horseid_seq'::regclass),
            name text not NULL,
            weight double precision not NULL,
            birthday timestamp without time zone not NULL,
            CONSTRAINT horse_pkey PRIMARY KEY (horseid)
        )
        WITH (
            OIDS= FALSE
        );

        ALTER TABLE public.horse OWNER TO portal;

    END;
$$