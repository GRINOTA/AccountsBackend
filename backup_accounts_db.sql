--
-- PostgreSQL database dump
--

-- Dumped from database version 17.2
-- Dumped by pg_dump version 17.2

-- Started on 2025-02-06 16:29:11

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
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 4850 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 238 (class 1255 OID 16559)
-- Name: process_transaction(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.process_transaction() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare
	sender_balance decimal;
	sender_currency_id int;
	reciver_currency_id int;
	exchange_rate decimal;
	amount_convert decimal;
begin
	amount_convert := new.amount;
	
	select accounts.balance, accounts.currency_id into sender_balance, sender_currency_id
	from accounts
	where accounts.id = new.sender_account_id
	for update;

	if new.amount <= 0.00 then
		RAISE EXCEPTION 'Некорректное значение';
	end if;

	if sender_balance < new.amount then
		RAISE EXCEPTION 'Недостаточно средств';
	end if;

	select accounts.currency_id into reciver_currency_id
	from accounts
	where accounts.id = new.recipient_account_id;

	if sender_currency_id != reciver_currency_id then
		select rate into exchange_rate
		from currency_rates
		where currency_rates.base_currency_id = sender_currency_id and currency_rates.target_currency_id = reciver_currency_id
		order by currency_rates.date desc
		limit 1;

		if exchange_rate is null then
			raise exception 'Курс конвертации валюты не найден';
		end if;

		amount_convert := amount_convert * exchange_rate;
	end if;
	
	update accounts
	set balance = balance - new.amount
	where accounts.id = new.sender_account_id;

	update accounts 
	set balance = balance + amount_convert
	where accounts.id = new.recipient_account_id;

	return new;
end;
$$;


ALTER FUNCTION public.process_transaction() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 217 (class 1259 OID 16389)
-- Name: accounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.accounts (
    id integer NOT NULL,
    user_id integer NOT NULL,
    currency_id integer NOT NULL,
    number character(20) NOT NULL,
    balance numeric(12,2) DEFAULT 0.00
);


ALTER TABLE public.accounts OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16393)
-- Name: accounts_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.accounts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.accounts_id_seq OWNER TO postgres;

--
-- TOC entry 4851 (class 0 OID 0)
-- Dependencies: 218
-- Name: accounts_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.accounts_id_seq OWNED BY public.accounts.id;


--
-- TOC entry 219 (class 1259 OID 16394)
-- Name: currencies; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.currencies (
    id integer NOT NULL,
    name text NOT NULL,
    code character(3) NOT NULL
);


ALTER TABLE public.currencies OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16399)
-- Name: currencies_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.currencies_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.currencies_id_seq OWNER TO postgres;

--
-- TOC entry 4852 (class 0 OID 0)
-- Dependencies: 220
-- Name: currencies_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.currencies_id_seq OWNED BY public.currencies.id;


--
-- TOC entry 221 (class 1259 OID 16400)
-- Name: currency_rates; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.currency_rates (
    id integer NOT NULL,
    base_currency_id integer NOT NULL,
    target_currency_id integer NOT NULL,
    rate numeric(12,2) NOT NULL,
    date date NOT NULL
);


ALTER TABLE public.currency_rates OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16403)
-- Name: currency_rates_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.currency_rates_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.currency_rates_id_seq OWNER TO postgres;

--
-- TOC entry 4853 (class 0 OID 0)
-- Dependencies: 222
-- Name: currency_rates_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.currency_rates_id_seq OWNED BY public.currency_rates.id;


--
-- TOC entry 223 (class 1259 OID 16404)
-- Name: transactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.transactions (
    id integer NOT NULL,
    sender_account_id integer NOT NULL,
    recipient_account_id integer NOT NULL,
    amount numeric(12,2) NOT NULL,
    date timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.transactions OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16407)
-- Name: transactions_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.transactions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.transactions_id_seq OWNER TO postgres;

--
-- TOC entry 4854 (class 0 OID 0)
-- Dependencies: 224
-- Name: transactions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.transactions_id_seq OWNED BY public.transactions.id;


--
-- TOC entry 225 (class 1259 OID 16408)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    surname text NOT NULL,
    first_name text NOT NULL,
    middle_name text,
    login character varying(16) NOT NULL,
    password text
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 16413)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- TOC entry 4855 (class 0 OID 0)
-- Dependencies: 226
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- TOC entry 4662 (class 2604 OID 16414)
-- Name: accounts id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts ALTER COLUMN id SET DEFAULT nextval('public.accounts_id_seq'::regclass);


--
-- TOC entry 4664 (class 2604 OID 16415)
-- Name: currencies id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.currencies ALTER COLUMN id SET DEFAULT nextval('public.currencies_id_seq'::regclass);


--
-- TOC entry 4665 (class 2604 OID 16416)
-- Name: currency_rates id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.currency_rates ALTER COLUMN id SET DEFAULT nextval('public.currency_rates_id_seq'::regclass);


--
-- TOC entry 4666 (class 2604 OID 16417)
-- Name: transactions id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions ALTER COLUMN id SET DEFAULT nextval('public.transactions_id_seq'::regclass);


--
-- TOC entry 4668 (class 2604 OID 16418)
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- TOC entry 4835 (class 0 OID 16389)
-- Dependencies: 217
-- Data for Name: accounts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.accounts (id, user_id, currency_id, number, balance) FROM stdin;
7	1	1	63874453025716900814	998.00
8	1	2	63874453032273739778	1001.00
\.


--
-- TOC entry 4837 (class 0 OID 16394)
-- Dependencies: 219
-- Data for Name: currencies; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.currencies (id, name, code) FROM stdin;
1	Рубль	RUB
2	Доллар	USD
\.


--
-- TOC entry 4839 (class 0 OID 16400)
-- Dependencies: 221
-- Data for Name: currency_rates; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.currency_rates (id, base_currency_id, target_currency_id, rate, date) FROM stdin;
\.


--
-- TOC entry 4841 (class 0 OID 16404)
-- Dependencies: 223
-- Data for Name: transactions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.transactions (id, sender_account_id, recipient_account_id, amount, date) FROM stdin;
39	7	8	1.00	2025-02-06 15:59:45.32251
40	7	8	1.00	2025-02-06 16:04:15.813931
41	7	8	1.00	2025-02-06 16:04:34.446835
42	7	8	1.00	2025-02-06 16:07:05.410174
43	7	8	1.00	2025-02-06 16:08:56.833662
44	7	8	1.00	2025-02-06 16:10:51.715079
45	7	8	1.00	2025-02-06 16:14:01.729308
46	7	8	1.00	2025-02-06 16:14:24.82421
47	7	8	1.00	2025-02-06 16:16:02.31468
48	7	8	1.00	2025-02-06 16:19:18.340901
\.


--
-- TOC entry 4843 (class 0 OID 16408)
-- Dependencies: 225
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, surname, first_name, middle_name, login, password) FROM stdin;
1	Нозиков	Григорий	Евгеньевич	grinotka	gri
2	312312	123213	21321	123213	12321
3	rtert	erter	ertet	ertert	ertert
4	rwer	ewrwe	werew	werwe	$2a$12$UfbcfHLX9VgPNxGcO0gfQes366.8JkdVKOn6Welj2jMtw7n4zeZJK
5	dsfdsf	fdsfsdf	sdfsd	grinch	$2a$12$g4YKXNotj03CP6MIJErtmuLl3jh1IB2dPl1Y0K9me1YIHY9oXIckW
\.


--
-- TOC entry 4856 (class 0 OID 0)
-- Dependencies: 218
-- Name: accounts_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.accounts_id_seq', 8, true);


--
-- TOC entry 4857 (class 0 OID 0)
-- Dependencies: 220
-- Name: currencies_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.currencies_id_seq', 2, true);


--
-- TOC entry 4858 (class 0 OID 0)
-- Dependencies: 222
-- Name: currency_rates_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.currency_rates_id_seq', 2, true);


--
-- TOC entry 4859 (class 0 OID 0)
-- Dependencies: 224
-- Name: transactions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.transactions_id_seq', 54, true);


--
-- TOC entry 4860 (class 0 OID 0)
-- Dependencies: 226
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 5, true);


--
-- TOC entry 4670 (class 2606 OID 16420)
-- Name: accounts pk_accounts_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT pk_accounts_id PRIMARY KEY (id);


--
-- TOC entry 4674 (class 2606 OID 16422)
-- Name: currencies pk_currencies_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.currencies
    ADD CONSTRAINT pk_currencies_id PRIMARY KEY (id);


--
-- TOC entry 4676 (class 2606 OID 16424)
-- Name: currency_rates pk_currency_rates_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT pk_currency_rates_id PRIMARY KEY (id);


--
-- TOC entry 4678 (class 2606 OID 16426)
-- Name: transactions pk_transaction_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT pk_transaction_id PRIMARY KEY (id);


--
-- TOC entry 4680 (class 2606 OID 16428)
-- Name: users pk_users_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT pk_users_id PRIMARY KEY (id);


--
-- TOC entry 4672 (class 2606 OID 16511)
-- Name: accounts uq_accounts_number; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT uq_accounts_number UNIQUE (number);


--
-- TOC entry 4682 (class 2606 OID 16509)
-- Name: users uq_users_login; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT uq_users_login UNIQUE (login);


--
-- TOC entry 4689 (class 2620 OID 16560)
-- Name: transactions tr_transaction_balance_update; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER tr_transaction_balance_update BEFORE INSERT ON public.transactions FOR EACH ROW EXECUTE FUNCTION public.process_transaction();


--
-- TOC entry 4683 (class 2606 OID 16429)
-- Name: accounts fk_accounts_currencies; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT fk_accounts_currencies FOREIGN KEY (currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE NOT VALID;


--
-- TOC entry 4684 (class 2606 OID 16434)
-- Name: accounts fk_accounts_users; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT fk_accounts_users FOREIGN KEY (user_id) REFERENCES public.users(id) ON UPDATE CASCADE;


--
-- TOC entry 4685 (class 2606 OID 16439)
-- Name: currency_rates fk_base_currency_rates_currencies; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT fk_base_currency_rates_currencies FOREIGN KEY (base_currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE;


--
-- TOC entry 4686 (class 2606 OID 16444)
-- Name: currency_rates fk_target_currency_rates_currencies; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT fk_target_currency_rates_currencies FOREIGN KEY (target_currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE;


--
-- TOC entry 4687 (class 2606 OID 16449)
-- Name: transactions fk_transactions_recipient_accounts; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT fk_transactions_recipient_accounts FOREIGN KEY (recipient_account_id) REFERENCES public.accounts(id) ON UPDATE CASCADE;


--
-- TOC entry 4688 (class 2606 OID 16454)
-- Name: transactions fk_transactions_sender_accounts; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT fk_transactions_sender_accounts FOREIGN KEY (sender_account_id) REFERENCES public.accounts(id) ON UPDATE CASCADE;


-- Completed on 2025-02-06 16:29:11

--
-- PostgreSQL database dump complete
--

