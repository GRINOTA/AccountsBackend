drop trigger tg_update_balance_sender_after_transaction on transactions;
drop function update_balance_sender_transaction()

create or replace function update_balance_sender_transaction()
returns trigger as $$
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
$$ language plpgsql;

create trigger tg_update_balance_sender_after_transaction
after insert on transactions
for each row
execute procedure update_balance_sender_transaction()