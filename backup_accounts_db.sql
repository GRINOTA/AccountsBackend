PGDMP                      }            accounts_db    17.2    17.2 -    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            �           1262    16388    accounts_db    DATABASE        CREATE DATABASE accounts_db WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE accounts_db;
                     postgres    false                        2615    2200    public    SCHEMA        CREATE SCHEMA public;
    DROP SCHEMA public;
                     pg_database_owner    false            �           0    0    SCHEMA public    COMMENT     6   COMMENT ON SCHEMA public IS 'standard public schema';
                        pg_database_owner    false    4            �            1255    16578    insert_reverse_currency_rate()    FUNCTION     +  CREATE FUNCTION public.insert_reverse_currency_rate() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if not exists (
		select 1 
		from currency_rates
		where base_currency_id=new.target_currency_id and 
			target_currency_id = new.base_currency_id and 
			date = new.date)
	then
		insert into currency_rates(base_currency_id, target_currency_id, rate, date)
		values (new.target_currency_id, new.base_currency_id, 1/new.rate, new.date);
	end if;

	-- on conflict (base_currency_id, target_currency_id, date)
	-- do nothing;

	return new;
end;
$$;
 5   DROP FUNCTION public.insert_reverse_currency_rate();
       public               postgres    false    4            �            1255    16582    process_transaction()    FUNCTION     �  CREATE FUNCTION public.process_transaction() RETURNS trigger
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
 ,   DROP FUNCTION public.process_transaction();
       public               postgres    false    4            �            1255    16602 #   update_balance_sender_transaction()    FUNCTION     7  CREATE FUNCTION public.update_balance_sender_transaction() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare 
	current_balance numeric;
begin
	select balance into current_balance from accounts where accounts.id = new.sender_account_id;
	-- raise notice 'баланс: %', current_balance;
	-- if current_balance is null then
	-- 	current_balance = 0;
	-- end if;

	-- if new.sender_account_id is null then
	-- 	current_balance = 0;
	-- end if;
	
	update transactions
	set balance_account_sender_update = current_balance
	where id = new.id;
	return new;
end;
$$;
 :   DROP FUNCTION public.update_balance_sender_transaction();
       public               postgres    false    4            �            1259    16389    accounts    TABLE     �   CREATE TABLE public.accounts (
    id integer NOT NULL,
    user_id integer NOT NULL,
    currency_id integer NOT NULL,
    number character(20) NOT NULL,
    balance numeric(12,2) DEFAULT 0.00 NOT NULL
);
    DROP TABLE public.accounts;
       public         heap r       postgres    false    4            �            1259    16393    accounts_id_seq    SEQUENCE     �   CREATE SEQUENCE public.accounts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.accounts_id_seq;
       public               postgres    false    4    217            �           0    0    accounts_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.accounts_id_seq OWNED BY public.accounts.id;
          public               postgres    false    218            �            1259    16394 
   currencies    TABLE     t   CREATE TABLE public.currencies (
    id integer NOT NULL,
    name text NOT NULL,
    code character(3) NOT NULL
);
    DROP TABLE public.currencies;
       public         heap r       postgres    false    4            �            1259    16399    currencies_id_seq    SEQUENCE     �   CREATE SEQUENCE public.currencies_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.currencies_id_seq;
       public               postgres    false    219    4            �           0    0    currencies_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.currencies_id_seq OWNED BY public.currencies.id;
          public               postgres    false    220            �            1259    16400    currency_rates    TABLE     �   CREATE TABLE public.currency_rates (
    id integer NOT NULL,
    base_currency_id integer NOT NULL,
    target_currency_id integer NOT NULL,
    rate numeric(18,6) NOT NULL,
    date date DEFAULT CURRENT_TIMESTAMP NOT NULL
);
 "   DROP TABLE public.currency_rates;
       public         heap r       postgres    false    4            �            1259    16403    currency_rates_id_seq    SEQUENCE     �   CREATE SEQUENCE public.currency_rates_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.currency_rates_id_seq;
       public               postgres    false    4    221            �           0    0    currency_rates_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.currency_rates_id_seq OWNED BY public.currency_rates.id;
          public               postgres    false    222            �            1259    16404    transactions    TABLE     .  CREATE TABLE public.transactions (
    id integer NOT NULL,
    sender_account_id integer NOT NULL,
    recipient_account_id integer NOT NULL,
    amount numeric(12,2) NOT NULL,
    date timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    balance_account_sender_update numeric(12,2)
);
     DROP TABLE public.transactions;
       public         heap r       postgres    false    4            �            1259    16407    transactions_id_seq    SEQUENCE     �   CREATE SEQUENCE public.transactions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.transactions_id_seq;
       public               postgres    false    223    4            �           0    0    transactions_id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.transactions_id_seq OWNED BY public.transactions.id;
          public               postgres    false    224            �            1259    16408    users    TABLE     �   CREATE TABLE public.users (
    id integer NOT NULL,
    surname text NOT NULL,
    first_name text NOT NULL,
    middle_name text,
    login character varying(16) NOT NULL,
    password text
);
    DROP TABLE public.users;
       public         heap r       postgres    false    4            �            1259    16413    users_id_seq    SEQUENCE     �   CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public               postgres    false    4    225            �           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public               postgres    false    226            8           2604    16414    accounts id    DEFAULT     j   ALTER TABLE ONLY public.accounts ALTER COLUMN id SET DEFAULT nextval('public.accounts_id_seq'::regclass);
 :   ALTER TABLE public.accounts ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    218    217            :           2604    16415    currencies id    DEFAULT     n   ALTER TABLE ONLY public.currencies ALTER COLUMN id SET DEFAULT nextval('public.currencies_id_seq'::regclass);
 <   ALTER TABLE public.currencies ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219            ;           2604    16416    currency_rates id    DEFAULT     v   ALTER TABLE ONLY public.currency_rates ALTER COLUMN id SET DEFAULT nextval('public.currency_rates_id_seq'::regclass);
 @   ALTER TABLE public.currency_rates ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    222    221            =           2604    16417    transactions id    DEFAULT     r   ALTER TABLE ONLY public.transactions ALTER COLUMN id SET DEFAULT nextval('public.transactions_id_seq'::regclass);
 >   ALTER TABLE public.transactions ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    224    223            ?           2604    16418    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    226    225            A           2606    16420    accounts pk_accounts_id 
   CONSTRAINT     U   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT pk_accounts_id PRIMARY KEY (id);
 A   ALTER TABLE ONLY public.accounts DROP CONSTRAINT pk_accounts_id;
       public                 postgres    false    217            E           2606    16422    currencies pk_currencies_id 
   CONSTRAINT     Y   ALTER TABLE ONLY public.currencies
    ADD CONSTRAINT pk_currencies_id PRIMARY KEY (id);
 E   ALTER TABLE ONLY public.currencies DROP CONSTRAINT pk_currencies_id;
       public                 postgres    false    219            G           2606    16424 #   currency_rates pk_currency_rates_id 
   CONSTRAINT     a   ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT pk_currency_rates_id PRIMARY KEY (id);
 M   ALTER TABLE ONLY public.currency_rates DROP CONSTRAINT pk_currency_rates_id;
       public                 postgres    false    221            I           2606    16426    transactions pk_transaction_id 
   CONSTRAINT     \   ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT pk_transaction_id PRIMARY KEY (id);
 H   ALTER TABLE ONLY public.transactions DROP CONSTRAINT pk_transaction_id;
       public                 postgres    false    223            K           2606    16428    users pk_users_id 
   CONSTRAINT     O   ALTER TABLE ONLY public.users
    ADD CONSTRAINT pk_users_id PRIMARY KEY (id);
 ;   ALTER TABLE ONLY public.users DROP CONSTRAINT pk_users_id;
       public                 postgres    false    225            C           2606    16511    accounts uq_accounts_number 
   CONSTRAINT     X   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT uq_accounts_number UNIQUE (number);
 E   ALTER TABLE ONLY public.accounts DROP CONSTRAINT uq_accounts_number;
       public                 postgres    false    217            M           2606    16509    users uq_users_login 
   CONSTRAINT     P   ALTER TABLE ONLY public.users
    ADD CONSTRAINT uq_users_login UNIQUE (login);
 >   ALTER TABLE ONLY public.users DROP CONSTRAINT uq_users_login;
       public                 postgres    false    225            T           2620    16579 (   currency_rates tg_currency_rates_reverse    TRIGGER     �   CREATE TRIGGER tg_currency_rates_reverse AFTER INSERT ON public.currency_rates FOR EACH ROW EXECUTE FUNCTION public.insert_reverse_currency_rate();
 A   DROP TRIGGER tg_currency_rates_reverse ON public.currency_rates;
       public               postgres    false    227    221            U           2620    16603 7   transactions tg_update_balance_sender_after_transaction    TRIGGER     �   CREATE TRIGGER tg_update_balance_sender_after_transaction AFTER INSERT ON public.transactions FOR EACH ROW EXECUTE FUNCTION public.update_balance_sender_transaction();
 P   DROP TRIGGER tg_update_balance_sender_after_transaction ON public.transactions;
       public               postgres    false    223    228            V           2620    16583 *   transactions tr_transaction_balance_update    TRIGGER     �   CREATE TRIGGER tr_transaction_balance_update BEFORE INSERT ON public.transactions FOR EACH ROW EXECUTE FUNCTION public.process_transaction();
 C   DROP TRIGGER tr_transaction_balance_update ON public.transactions;
       public               postgres    false    240    223            N           2606    16429    accounts fk_accounts_currencies    FK CONSTRAINT     �   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT fk_accounts_currencies FOREIGN KEY (currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE NOT VALID;
 I   ALTER TABLE ONLY public.accounts DROP CONSTRAINT fk_accounts_currencies;
       public               postgres    false    217    219    4677            O           2606    16434    accounts fk_accounts_users    FK CONSTRAINT     �   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT fk_accounts_users FOREIGN KEY (user_id) REFERENCES public.users(id) ON UPDATE CASCADE;
 D   ALTER TABLE ONLY public.accounts DROP CONSTRAINT fk_accounts_users;
       public               postgres    false    217    225    4683            P           2606    16439 0   currency_rates fk_base_currency_rates_currencies    FK CONSTRAINT     �   ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT fk_base_currency_rates_currencies FOREIGN KEY (base_currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE;
 Z   ALTER TABLE ONLY public.currency_rates DROP CONSTRAINT fk_base_currency_rates_currencies;
       public               postgres    false    219    221    4677            Q           2606    16444 2   currency_rates fk_target_currency_rates_currencies    FK CONSTRAINT     �   ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT fk_target_currency_rates_currencies FOREIGN KEY (target_currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE;
 \   ALTER TABLE ONLY public.currency_rates DROP CONSTRAINT fk_target_currency_rates_currencies;
       public               postgres    false    219    4677    221            R           2606    16449 /   transactions fk_transactions_recipient_accounts    FK CONSTRAINT     �   ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT fk_transactions_recipient_accounts FOREIGN KEY (recipient_account_id) REFERENCES public.accounts(id) ON UPDATE CASCADE;
 Y   ALTER TABLE ONLY public.transactions DROP CONSTRAINT fk_transactions_recipient_accounts;
       public               postgres    false    223    4673    217            S           2606    16454 ,   transactions fk_transactions_sender_accounts    FK CONSTRAINT     �   ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT fk_transactions_sender_accounts FOREIGN KEY (sender_account_id) REFERENCES public.accounts(id) ON UPDATE CASCADE;
 V   ALTER TABLE ONLY public.transactions DROP CONSTRAINT fk_transactions_sender_accounts;
       public               postgres    false    223    217    4673           