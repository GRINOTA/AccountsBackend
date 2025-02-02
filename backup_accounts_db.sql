PGDMP                       }            accounts_db    17.2    17.2 -    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            �           1262    16499    accounts_db    DATABASE        CREATE DATABASE accounts_db WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE accounts_db;
                     postgres    false            �            1259    16500    accounts    TABLE     �   CREATE TABLE public.accounts (
    id integer NOT NULL,
    user_id integer NOT NULL,
    currency_id integer NOT NULL,
    number character(20) NOT NULL,
    balance money DEFAULT 0.00 NOT NULL
);
    DROP TABLE public.accounts;
       public         heap r       postgres    false            �            1259    16504    accounts_id_seq    SEQUENCE     �   CREATE SEQUENCE public.accounts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.accounts_id_seq;
       public               postgres    false    217            �           0    0    accounts_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.accounts_id_seq OWNED BY public.accounts.id;
          public               postgres    false    218            �            1259    16505 
   currencies    TABLE     t   CREATE TABLE public.currencies (
    id integer NOT NULL,
    name text NOT NULL,
    code character(3) NOT NULL
);
    DROP TABLE public.currencies;
       public         heap r       postgres    false            �            1259    16510    currencies_id_seq    SEQUENCE     �   CREATE SEQUENCE public.currencies_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.currencies_id_seq;
       public               postgres    false    219            �           0    0    currencies_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.currencies_id_seq OWNED BY public.currencies.id;
          public               postgres    false    220            �            1259    16511    currency_rates    TABLE     �   CREATE TABLE public.currency_rates (
    id integer NOT NULL,
    base_currency_id integer NOT NULL,
    target_currency_id integer NOT NULL,
    rate money NOT NULL,
    date date NOT NULL
);
 "   DROP TABLE public.currency_rates;
       public         heap r       postgres    false            �            1259    16514    currency_rates_id_seq    SEQUENCE     �   CREATE SEQUENCE public.currency_rates_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.currency_rates_id_seq;
       public               postgres    false    221            �           0    0    currency_rates_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.currency_rates_id_seq OWNED BY public.currency_rates.id;
          public               postgres    false    222            �            1259    16515    transactions    TABLE     �   CREATE TABLE public.transactions (
    id integer NOT NULL,
    sender_account_id integer NOT NULL,
    recipient_account_id integer NOT NULL,
    amount money NOT NULL,
    date date NOT NULL
);
     DROP TABLE public.transactions;
       public         heap r       postgres    false            �            1259    16518    transactions_id_seq    SEQUENCE     �   CREATE SEQUENCE public.transactions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.transactions_id_seq;
       public               postgres    false    223            �           0    0    transactions_id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.transactions_id_seq OWNED BY public.transactions.id;
          public               postgres    false    224            �            1259    16519    users    TABLE     �   CREATE TABLE public.users (
    id integer NOT NULL,
    surname text NOT NULL,
    first_name text NOT NULL,
    middle_name text,
    login character varying(16) NOT NULL,
    password character varying(16)
);
    DROP TABLE public.users;
       public         heap r       postgres    false            �            1259    16524    users_id_seq    SEQUENCE     �   CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public               postgres    false    225            �           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public               postgres    false    226            5           2604    16525    accounts id    DEFAULT     j   ALTER TABLE ONLY public.accounts ALTER COLUMN id SET DEFAULT nextval('public.accounts_id_seq'::regclass);
 :   ALTER TABLE public.accounts ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    218    217            7           2604    16526    currencies id    DEFAULT     n   ALTER TABLE ONLY public.currencies ALTER COLUMN id SET DEFAULT nextval('public.currencies_id_seq'::regclass);
 <   ALTER TABLE public.currencies ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219            8           2604    16527    currency_rates id    DEFAULT     v   ALTER TABLE ONLY public.currency_rates ALTER COLUMN id SET DEFAULT nextval('public.currency_rates_id_seq'::regclass);
 @   ALTER TABLE public.currency_rates ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    222    221            9           2604    16528    transactions id    DEFAULT     r   ALTER TABLE ONLY public.transactions ALTER COLUMN id SET DEFAULT nextval('public.transactions_id_seq'::regclass);
 >   ALTER TABLE public.transactions ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    224    223            :           2604    16529    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    226    225            �          0    16500    accounts 
   TABLE DATA           M   COPY public.accounts (id, user_id, currency_id, number, balance) FROM stdin;
    public               postgres    false    217   5       �          0    16505 
   currencies 
   TABLE DATA           4   COPY public.currencies (id, name, code) FROM stdin;
    public               postgres    false    219   Y5       �          0    16511    currency_rates 
   TABLE DATA           ^   COPY public.currency_rates (id, base_currency_id, target_currency_id, rate, date) FROM stdin;
    public               postgres    false    221   �5       �          0    16515    transactions 
   TABLE DATA           a   COPY public.transactions (id, sender_account_id, recipient_account_id, amount, date) FROM stdin;
    public               postgres    false    223   �5       �          0    16519    users 
   TABLE DATA           V   COPY public.users (id, surname, first_name, middle_name, login, password) FROM stdin;
    public               postgres    false    225   �5       �           0    0    accounts_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.accounts_id_seq', 2, true);
          public               postgres    false    218            �           0    0    currencies_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.currencies_id_seq', 2, true);
          public               postgres    false    220            �           0    0    currency_rates_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.currency_rates_id_seq', 1, false);
          public               postgres    false    222            �           0    0    transactions_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.transactions_id_seq', 1, false);
          public               postgres    false    224            �           0    0    users_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.users_id_seq', 1, true);
          public               postgres    false    226            <           2606    16531    accounts pk_accounts_id 
   CONSTRAINT     U   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT pk_accounts_id PRIMARY KEY (id);
 A   ALTER TABLE ONLY public.accounts DROP CONSTRAINT pk_accounts_id;
       public                 postgres    false    217            >           2606    16533    currencies pk_currencies_id 
   CONSTRAINT     Y   ALTER TABLE ONLY public.currencies
    ADD CONSTRAINT pk_currencies_id PRIMARY KEY (id);
 E   ALTER TABLE ONLY public.currencies DROP CONSTRAINT pk_currencies_id;
       public                 postgres    false    219            @           2606    16535 #   currency_rates pk_currency_rates_id 
   CONSTRAINT     a   ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT pk_currency_rates_id PRIMARY KEY (id);
 M   ALTER TABLE ONLY public.currency_rates DROP CONSTRAINT pk_currency_rates_id;
       public                 postgres    false    221            B           2606    16537    transactions pk_transaction_id 
   CONSTRAINT     \   ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT pk_transaction_id PRIMARY KEY (id);
 H   ALTER TABLE ONLY public.transactions DROP CONSTRAINT pk_transaction_id;
       public                 postgres    false    223            D           2606    16539    users pk_users_id 
   CONSTRAINT     O   ALTER TABLE ONLY public.users
    ADD CONSTRAINT pk_users_id PRIMARY KEY (id);
 ;   ALTER TABLE ONLY public.users DROP CONSTRAINT pk_users_id;
       public                 postgres    false    225            E           2606    16540    accounts fk_accounts_currencies    FK CONSTRAINT     �   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT fk_accounts_currencies FOREIGN KEY (currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE NOT VALID;
 I   ALTER TABLE ONLY public.accounts DROP CONSTRAINT fk_accounts_currencies;
       public               postgres    false    4670    217    219            F           2606    16545    accounts fk_accounts_users    FK CONSTRAINT     �   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT fk_accounts_users FOREIGN KEY (user_id) REFERENCES public.users(id) ON UPDATE CASCADE;
 D   ALTER TABLE ONLY public.accounts DROP CONSTRAINT fk_accounts_users;
       public               postgres    false    225    4676    217            G           2606    16550 0   currency_rates fk_base_currency_rates_currencies    FK CONSTRAINT     �   ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT fk_base_currency_rates_currencies FOREIGN KEY (base_currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE;
 Z   ALTER TABLE ONLY public.currency_rates DROP CONSTRAINT fk_base_currency_rates_currencies;
       public               postgres    false    221    219    4670            H           2606    16555 2   currency_rates fk_target_currency_rates_currencies    FK CONSTRAINT     �   ALTER TABLE ONLY public.currency_rates
    ADD CONSTRAINT fk_target_currency_rates_currencies FOREIGN KEY (target_currency_id) REFERENCES public.currencies(id) ON UPDATE CASCADE;
 \   ALTER TABLE ONLY public.currency_rates DROP CONSTRAINT fk_target_currency_rates_currencies;
       public               postgres    false    221    219    4670            I           2606    16560 /   transactions fk_transactions_recipient_accounts    FK CONSTRAINT     �   ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT fk_transactions_recipient_accounts FOREIGN KEY (recipient_account_id) REFERENCES public.accounts(id) ON UPDATE CASCADE;
 Y   ALTER TABLE ONLY public.transactions DROP CONSTRAINT fk_transactions_recipient_accounts;
       public               postgres    false    223    217    4668            J           2606    16565 ,   transactions fk_transactions_sender_accounts    FK CONSTRAINT     �   ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT fk_transactions_sender_accounts FOREIGN KEY (sender_account_id) REFERENCES public.accounts(id) ON UPDATE CASCADE;
 V   ALTER TABLE ONLY public.transactions DROP CONSTRAINT fk_transactions_sender_accounts;
       public               postgres    false    217    223    4668            �   .   x�3�4A#cS33s$ �14R��2*0)�P\I� �l
�      �   1   x�3估�b�v_��
u�2�0�¾��p����`�=... �E      �      x������ � �      �      x������ � �      �   J   x�3�0�¾�/츰Ho�0�b����>0�������ܭ�^���.��Ι^���_��bp��qqq ��*�     