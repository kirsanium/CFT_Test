--
-- PostgreSQL database dump
--

-- Dumped from database version 10.6 (Ubuntu 10.6-0ubuntu0.18.04.1)
-- Dumped by pg_dump version 10.6 (Ubuntu 10.6-0ubuntu0.18.04.1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

CREATE DATABASE cftdb WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'en_US.utf8' LC_CTYPE = 'en_US.utf8';


ALTER DATABASE cftdb OWNER TO cftdb;

\connect cftdb
--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: Projects; Type: TABLE; Schema: public; Owner: cftdb
--

CREATE TABLE public."Projects" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "CreationDate" timestamp without time zone NOT NULL,
    "ModificationDate" timestamp without time zone NOT NULL
);


ALTER TABLE public."Projects" OWNER TO cftdb;

--
-- Name: Projects_Id_seq; Type: SEQUENCE; Schema: public; Owner: cftdb
--

CREATE SEQUENCE public."Projects_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Projects_Id_seq" OWNER TO cftdb;

--
-- Name: Projects_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: cftdb
--

ALTER SEQUENCE public."Projects_Id_seq" OWNED BY public."Projects"."Id";


--
-- Name: Tasks; Type: TABLE; Schema: public; Owner: cftdb
--

CREATE TABLE public."Tasks" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "Priority" smallint NOT NULL,
    "Status" integer NOT NULL,
    "ProjectId" integer NOT NULL,
    "CreationDate" timestamp without time zone NOT NULL,
    "ModificationDate" timestamp without time zone NOT NULL
);


ALTER TABLE public."Tasks" OWNER TO cftdb;

--
-- Name: Tasks_Id_seq; Type: SEQUENCE; Schema: public; Owner: cftdb
--

CREATE SEQUENCE public."Tasks_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Tasks_Id_seq" OWNER TO cftdb;

--
-- Name: Tasks_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: cftdb
--

ALTER SEQUENCE public."Tasks_Id_seq" OWNED BY public."Tasks"."Id";


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: cftdb
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO cftdb;

--
-- Name: Projects Id; Type: DEFAULT; Schema: public; Owner: cftdb
--

ALTER TABLE ONLY public."Projects" ALTER COLUMN "Id" SET DEFAULT nextval('public."Projects_Id_seq"'::regclass);


--
-- Name: Tasks Id; Type: DEFAULT; Schema: public; Owner: cftdb
--

ALTER TABLE ONLY public."Tasks" ALTER COLUMN "Id" SET DEFAULT nextval('public."Tasks_Id_seq"'::regclass);


--
-- Data for Name: Projects; Type: TABLE DATA; Schema: public; Owner: cftdb
--

COPY public."Projects" ("Id", "Name", "Description", "CreationDate", "ModificationDate") FROM stdin;
\.


--
-- Data for Name: Tasks; Type: TABLE DATA; Schema: public; Owner: cftdb
--

COPY public."Tasks" ("Id", "Name", "Description", "Priority", "Status", "ProjectId", "CreationDate", "ModificationDate") FROM stdin;
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: cftdb
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20190126114659_InitialCreate	2.2.1-servicing-10028
\.


--
-- Name: Projects_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: cftdb
--

SELECT pg_catalog.setval('public."Projects_Id_seq"', 12, true);


--
-- Name: Tasks_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: cftdb
--

SELECT pg_catalog.setval('public."Tasks_Id_seq"', 14, true);


--
-- Name: Projects PK_Projects; Type: CONSTRAINT; Schema: public; Owner: cftdb
--

ALTER TABLE ONLY public."Projects"
    ADD CONSTRAINT "PK_Projects" PRIMARY KEY ("Id");


--
-- Name: Tasks PK_Tasks; Type: CONSTRAINT; Schema: public; Owner: cftdb
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT "PK_Tasks" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: cftdb
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Tasks_ProjectId; Type: INDEX; Schema: public; Owner: cftdb
--

CREATE INDEX "IX_Tasks_ProjectId" ON public."Tasks" USING btree ("ProjectId");


--
-- Name: Tasks FK_Tasks_Projects_ProjectId; Type: FK CONSTRAINT; Schema: public; Owner: cftdb
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT "FK_Tasks_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES public."Projects"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

