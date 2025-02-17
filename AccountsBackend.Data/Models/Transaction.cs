namespace AccountsBackend.Data.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int SenderAccountId { get; set; }

    public int RecipientAccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public decimal? BalanceAccountSenderUpdate { get; set; }

    public virtual Account RecipientAccount { get; set; } = null!;

    public virtual Account SenderAccount { get; set; } = null!;
}
