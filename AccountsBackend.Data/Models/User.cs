namespace AccountsBackend.Data.Models;

public partial class User
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Login { get; set; } = null!;

    public string? Password { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
