namespace AccountsBackend.Data.Models;

public partial class Account
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CurrencyId { get; set; }

    public string Number { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual ICollection<Transaction> TransactionRecipientAccounts { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSenderAccounts { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
