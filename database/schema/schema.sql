--
-- PostgreSQL database dump
--

\restrict l6a323dYMxfqPxL2JJorbEo8tjTy1X3qvhQi54zN2JiERKgdLOOWfcj8Z3dxgD5

-- Dumped from database version 17.6
-- Dumped by pg_dump version 17.6

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: pgagent; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA pgagent;


ALTER SCHEMA pgagent OWNER TO postgres;

--
-- Name: SCHEMA pgagent; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA pgagent IS 'pgAgent system tables';


--
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO postgres;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA public IS '';


--
-- Name: pgagent; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pgagent WITH SCHEMA pgagent;


--
-- Name: EXTENSION pgagent; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pgagent IS 'A PostgreSQL job scheduler';


--
-- Name: form_status; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.form_status AS ENUM (
    'Draft',
    'Completed',
    'Exported'
);


ALTER TYPE public.form_status OWNER TO postgres;

--
-- Name: message_role; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.message_role AS ENUM (
    'User',
    'Assistant',
    'System'
);


ALTER TYPE public.message_role OWNER TO postgres;

--
-- Name: payment_status; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.payment_status AS ENUM (
    'Pending',
    'Completed',
    'Failed',
    'Refunded'
);


ALTER TYPE public.payment_status OWNER TO postgres;

--
-- Name: payment_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.payment_type AS ENUM (
    'OneTime',
    'Subscription'
);


ALTER TYPE public.payment_type OWNER TO postgres;

--
-- Name: subscription_status; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.subscription_status AS ENUM (
    'Active',
    'Cancelled',
    'Expired',
    'PastDue'
);


ALTER TYPE public.subscription_status OWNER TO postgres;

--
-- Name: user_role; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.user_role AS ENUM (
    'Client',
    'Admin'
);


ALTER TYPE public.user_role OWNER TO postgres;

--
-- Name: update_updated_at_column(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_updated_at_column() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    NEW."UpdatedAt" = NOW();
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.update_updated_at_column() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: AIConversations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AIConversations" (
    "Id" integer NOT NULL,
    "UserId" integer NOT NULL,
    "Title" character varying(255) NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."AIConversations" OWNER TO postgres;

--
-- Name: AIConversations_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."AIConversations_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."AIConversations_Id_seq" OWNER TO postgres;

--
-- Name: AIConversations_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."AIConversations_Id_seq" OWNED BY public."AIConversations"."Id";


--
-- Name: AIMessages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AIMessages" (
    "Id" integer NOT NULL,
    "ConversationId" integer NOT NULL,
    "Content" text NOT NULL,
    "Role" public.message_role NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE public."AIMessages" OWNER TO postgres;

--
-- Name: AIMessages_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."AIMessages_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."AIMessages_Id_seq" OWNER TO postgres;

--
-- Name: AIMessages_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."AIMessages_Id_seq" OWNED BY public."AIMessages"."Id";


--
-- Name: FormTemplates; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."FormTemplates" (
    "Id" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Description" text DEFAULT ''::text NOT NULL,
    "Category" character varying(100) NOT NULL,
    "Price" numeric(10,2) DEFAULT 0.00 NOT NULL,
    "IsPremium" boolean DEFAULT false NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL,
    "FormSchema" jsonb NOT NULL,
    "HtmlTemplate" text DEFAULT ''::text NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "CreatedByUserId" integer NOT NULL,
    "Status" integer
);


ALTER TABLE public."FormTemplates" OWNER TO postgres;

--
-- Name: FormTemplates_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."FormTemplates_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."FormTemplates_Id_seq" OWNER TO postgres;

--
-- Name: FormTemplates_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."FormTemplates_Id_seq" OWNED BY public."FormTemplates"."Id";


--
-- Name: Payments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Payments" (
    "Id" integer NOT NULL,
    "UserId" integer NOT NULL,
    "StripePaymentIntentId" character varying(255) NOT NULL,
    "Amount" numeric(10,2) NOT NULL,
    "Currency" character varying(3) DEFAULT 'USD'::character varying NOT NULL,
    "FormTemplateId" integer,
    "SubscriptionId" integer,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "CompletedAt" timestamp with time zone,
    "Type" integer,
    "Status" integer
);


ALTER TABLE public."Payments" OWNER TO postgres;

--
-- Name: Payments_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Payments_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Payments_Id_seq" OWNER TO postgres;

--
-- Name: Payments_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Payments_Id_seq" OWNED BY public."Payments"."Id";


--
-- Name: Subscriptions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Subscriptions" (
    "Id" integer NOT NULL,
    "UserId" integer NOT NULL,
    "StripeSubscriptionId" character varying(255) NOT NULL,
    "PlanName" character varying(100) NOT NULL,
    "MonthlyPrice" numeric(10,2) NOT NULL,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone,
    "NextBillingDate" timestamp with time zone NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "Status" integer
);


ALTER TABLE public."Subscriptions" OWNER TO postgres;

--
-- Name: Subscriptions_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Subscriptions_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Subscriptions_Id_seq" OWNER TO postgres;

--
-- Name: Subscriptions_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Subscriptions_Id_seq" OWNED BY public."Subscriptions"."Id";


--
-- Name: UserForms; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserForms" (
    "Id" integer NOT NULL,
    "UserId" integer NOT NULL,
    "FormTemplateId" integer NOT NULL,
    "FormData" jsonb NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "CompletedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone,
    "Status" integer
);


ALTER TABLE public."UserForms" OWNER TO postgres;

--
-- Name: UserForms_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."UserForms_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."UserForms_Id_seq" OWNER TO postgres;

--
-- Name: UserForms_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."UserForms_Id_seq" OWNED BY public."UserForms"."Id";


--
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PasswordHash" character varying(255) NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100) NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "IsActive" boolean DEFAULT true NOT NULL,
    "Role" integer
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Users_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Users_Id_seq" OWNER TO postgres;

--
-- Name: Users_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Users_Id_seq" OWNED BY public."Users"."Id";


--
-- Name: AIConversations Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AIConversations" ALTER COLUMN "Id" SET DEFAULT nextval('public."AIConversations_Id_seq"'::regclass);


--
-- Name: AIMessages Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AIMessages" ALTER COLUMN "Id" SET DEFAULT nextval('public."AIMessages_Id_seq"'::regclass);


--
-- Name: FormTemplates Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FormTemplates" ALTER COLUMN "Id" SET DEFAULT nextval('public."FormTemplates_Id_seq"'::regclass);


--
-- Name: Payments Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payments" ALTER COLUMN "Id" SET DEFAULT nextval('public."Payments_Id_seq"'::regclass);


--
-- Name: Subscriptions Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Subscriptions" ALTER COLUMN "Id" SET DEFAULT nextval('public."Subscriptions_Id_seq"'::regclass);


--
-- Name: UserForms Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserForms" ALTER COLUMN "Id" SET DEFAULT nextval('public."UserForms_Id_seq"'::regclass);


--
-- Name: Users Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users" ALTER COLUMN "Id" SET DEFAULT nextval('public."Users_Id_seq"'::regclass);


--
-- Name: AIConversations AIConversations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AIConversations"
    ADD CONSTRAINT "AIConversations_pkey" PRIMARY KEY ("Id");


--
-- Name: AIMessages AIMessages_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AIMessages"
    ADD CONSTRAINT "AIMessages_pkey" PRIMARY KEY ("Id");


--
-- Name: FormTemplates FormTemplates_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FormTemplates"
    ADD CONSTRAINT "FormTemplates_pkey" PRIMARY KEY ("Id");


--
-- Name: Payments Payments_StripePaymentIntentId_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payments"
    ADD CONSTRAINT "Payments_StripePaymentIntentId_key" UNIQUE ("StripePaymentIntentId");


--
-- Name: Payments Payments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payments"
    ADD CONSTRAINT "Payments_pkey" PRIMARY KEY ("Id");


--
-- Name: Subscriptions Subscriptions_StripeSubscriptionId_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Subscriptions"
    ADD CONSTRAINT "Subscriptions_StripeSubscriptionId_key" UNIQUE ("StripeSubscriptionId");


--
-- Name: Subscriptions Subscriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Subscriptions"
    ADD CONSTRAINT "Subscriptions_pkey" PRIMARY KEY ("Id");


--
-- Name: UserForms UserForms_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserForms"
    ADD CONSTRAINT "UserForms_pkey" PRIMARY KEY ("Id");


--
-- Name: Users Users_Email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_Email_key" UNIQUE ("Email");


--
-- Name: Users Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("Id");


--
-- Name: IX_AIConversations_CreatedAt; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AIConversations_CreatedAt" ON public."AIConversations" USING btree ("CreatedAt");


--
-- Name: IX_AIConversations_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AIConversations_UserId" ON public."AIConversations" USING btree ("UserId");


--
-- Name: IX_AIMessages_ConversationId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AIMessages_ConversationId" ON public."AIMessages" USING btree ("ConversationId");


--
-- Name: IX_AIMessages_CreatedAt; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AIMessages_CreatedAt" ON public."AIMessages" USING btree ("CreatedAt");


--
-- Name: IX_AIMessages_Role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AIMessages_Role" ON public."AIMessages" USING btree ("Role");


--
-- Name: IX_FormTemplates_Category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FormTemplates_Category" ON public."FormTemplates" USING btree ("Category");


--
-- Name: IX_FormTemplates_CreatedByUserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FormTemplates_CreatedByUserId" ON public."FormTemplates" USING btree ("CreatedByUserId");


--
-- Name: IX_FormTemplates_IsActive; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FormTemplates_IsActive" ON public."FormTemplates" USING btree ("IsActive");


--
-- Name: IX_FormTemplates_IsPremium; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_FormTemplates_IsPremium" ON public."FormTemplates" USING btree ("IsPremium");


--
-- Name: IX_Payments_CreatedAt; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payments_CreatedAt" ON public."Payments" USING btree ("CreatedAt");


--
-- Name: IX_Payments_FormTemplateId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payments_FormTemplateId" ON public."Payments" USING btree ("FormTemplateId");


--
-- Name: IX_Payments_SubscriptionId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payments_SubscriptionId" ON public."Payments" USING btree ("SubscriptionId");


--
-- Name: IX_Payments_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Payments_UserId" ON public."Payments" USING btree ("UserId");


--
-- Name: IX_Subscriptions_NextBillingDate; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Subscriptions_NextBillingDate" ON public."Subscriptions" USING btree ("NextBillingDate");


--
-- Name: IX_Subscriptions_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Subscriptions_UserId" ON public."Subscriptions" USING btree ("UserId");


--
-- Name: IX_UserForms_CreatedAt; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserForms_CreatedAt" ON public."UserForms" USING btree ("CreatedAt");


--
-- Name: IX_UserForms_FormTemplateId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserForms_FormTemplateId" ON public."UserForms" USING btree ("FormTemplateId");


--
-- Name: IX_UserForms_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserForms_UserId" ON public."UserForms" USING btree ("UserId");


--
-- Name: IX_Users_Email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");


--
-- Name: IX_Users_IsActive; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Users_IsActive" ON public."Users" USING btree ("IsActive");


--
-- Name: AIConversations update_ai_conversations_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER update_ai_conversations_updated_at BEFORE UPDATE ON public."AIConversations" FOR EACH ROW EXECUTE FUNCTION public.update_updated_at_column();


--
-- Name: FormTemplates update_form_templates_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER update_form_templates_updated_at BEFORE UPDATE ON public."FormTemplates" FOR EACH ROW EXECUTE FUNCTION public.update_updated_at_column();


--
-- Name: UserForms update_user_forms_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER update_user_forms_updated_at BEFORE UPDATE ON public."UserForms" FOR EACH ROW EXECUTE FUNCTION public.update_updated_at_column();


--
-- Name: Users update_users_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON public."Users" FOR EACH ROW EXECUTE FUNCTION public.update_updated_at_column();


--
-- Name: AIConversations AIConversations_UserId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AIConversations"
    ADD CONSTRAINT "AIConversations_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: AIMessages AIMessages_ConversationId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AIMessages"
    ADD CONSTRAINT "AIMessages_ConversationId_fkey" FOREIGN KEY ("ConversationId") REFERENCES public."AIConversations"("Id") ON DELETE CASCADE;


--
-- Name: FormTemplates FormTemplates_CreatedByUserId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."FormTemplates"
    ADD CONSTRAINT "FormTemplates_CreatedByUserId_fkey" FOREIGN KEY ("CreatedByUserId") REFERENCES public."Users"("Id") ON DELETE RESTRICT;


--
-- Name: Payments Payments_FormTemplateId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payments"
    ADD CONSTRAINT "Payments_FormTemplateId_fkey" FOREIGN KEY ("FormTemplateId") REFERENCES public."FormTemplates"("Id") ON DELETE SET NULL;


--
-- Name: Payments Payments_SubscriptionId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payments"
    ADD CONSTRAINT "Payments_SubscriptionId_fkey" FOREIGN KEY ("SubscriptionId") REFERENCES public."Subscriptions"("Id") ON DELETE SET NULL;


--
-- Name: Payments Payments_UserId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payments"
    ADD CONSTRAINT "Payments_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Subscriptions Subscriptions_UserId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Subscriptions"
    ADD CONSTRAINT "Subscriptions_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserForms UserForms_FormTemplateId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserForms"
    ADD CONSTRAINT "UserForms_FormTemplateId_fkey" FOREIGN KEY ("FormTemplateId") REFERENCES public."FormTemplates"("Id") ON DELETE RESTRICT;


--
-- Name: UserForms UserForms_UserId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserForms"
    ADD CONSTRAINT "UserForms_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

\unrestrict l6a323dYMxfqPxL2JJorbEo8tjTy1X3qvhQi54zN2JiERKgdLOOWfcj8Z3dxgD5

