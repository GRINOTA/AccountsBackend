drop trigger tr_transaction_balance_update on transactions;
drop function process_transaction();

create or replace function process_transaction()
returns trigger as $$
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
$$ LANGUAGE plpgsql;

create trigger tr_transaction_balance_update
before insert on transactions
for each row 
execute function process_transaction();