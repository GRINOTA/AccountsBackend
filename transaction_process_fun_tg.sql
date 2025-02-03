create or replace function process_transaction()
returns trigger as $$
declare
	sender_balance decimal;
begin
	select accounts.balance::decimal into sender_balance
	from accounts
	where accounts.id = new.sender_account_id
	for update;

	if new.amount::decimal <= 0.00 then
		RAISE EXCEPTION 'Некорректная сумма';
	end if;

	if sender_balance < new.amount::decimal  then
		RAISE EXCEPTION 'Недостаточно средств';
	end if;
	
	update accounts
	set balance = balance - new.amount
	where id = new.sender_account_id;

	update accounts 
	set balance = balance + new.amount 
	where id = new.recipient_account_id;

	return new;
end;
$$ LANGUAGE plpgsql;

create trigger tr_transaction_balance_update
before insert on transactions
for each row 
execute function process_transaction();