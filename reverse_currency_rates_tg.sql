drop function insert_reverse_currency_rate();
drop trigger tg_currency_rates_reverse on currency_rates;

create or replace function insert_reverse_currency_rate()
returns trigger as $$
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

	return new;
end;
$$ language plpgsql;

create trigger tg_currency_rates_reverse
after insert on currency_rates
for each row
execute function insert_reverse_currency_rate();