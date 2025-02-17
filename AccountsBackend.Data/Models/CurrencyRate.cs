namespace AccountsBackend.Data.Models;

public partial class CurrencyRate
{
    public int Id { get; set; }

    public int BaseCurrencyId { get; set; }

    public int TargetCurrencyId { get; set; }

    public decimal Rate { get; set; }

    public DateOnly Date { get; set; }

    public virtual Currency BaseCurrency { get; set; } = null!;

    public virtual Currency TargetCurrency { get; set; } = null!;
}
