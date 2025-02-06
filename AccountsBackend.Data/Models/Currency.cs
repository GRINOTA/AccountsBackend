namespace AccountsBackend.Data.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<CurrencyRate> CurrencyRateBaseCurrencies { get; set; } = new List<CurrencyRate>();

    public virtual ICollection<CurrencyRate> CurrencyRateTargetCurrencies { get; set; } = new List<CurrencyRate>();
}
