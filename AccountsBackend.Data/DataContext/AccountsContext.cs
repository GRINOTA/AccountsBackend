using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data.DataContext;

public partial class AccountsContext : DbContext
{
    public AccountsContext()
    {
    }

    public AccountsContext(DbContextOptions<AccountsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=accounts_db;Username=postgres;Password=Moun13");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_accounts_id");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Number, "uq_accounts_number").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("balance");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("number");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Currency).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_accounts_currencies");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_accounts_users");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_currencies_id");

            entity.ToTable("currencies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<CurrencyRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_currency_rates_id");

            entity.ToTable("currency_rates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BaseCurrencyId).HasColumnName("base_currency_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("date");
            entity.Property(e => e.Rate)
                .HasPrecision(18, 6)
                .HasColumnName("rate");
            entity.Property(e => e.TargetCurrencyId).HasColumnName("target_currency_id");

            entity.HasOne(d => d.BaseCurrency).WithMany(p => p.CurrencyRateBaseCurrencies)
                .HasForeignKey(d => d.BaseCurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_base_currency_rates_currencies");

            entity.HasOne(d => d.TargetCurrency).WithMany(p => p.CurrencyRateTargetCurrencies)
                .HasForeignKey(d => d.TargetCurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_target_currency_rates_currencies");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_transaction_id");

            entity.ToTable("transactions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.BalanceAccountSenderUpdate)
                .HasPrecision(12, 2)
                .HasColumnName("balance_account_sender_update");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.RecipientAccountId).HasColumnName("recipient_account_id");
            entity.Property(e => e.SenderAccountId).HasColumnName("sender_account_id");

            entity.HasOne(d => d.RecipientAccount).WithMany(p => p.TransactionRecipientAccounts)
                .HasForeignKey(d => d.RecipientAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_transactions_recipient_accounts");

            entity.HasOne(d => d.SenderAccount).WithMany(p => p.TransactionSenderAccounts)
                .HasForeignKey(d => d.SenderAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_transactions_sender_accounts");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_users_id");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "uq_users_login").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.Login)
                .HasMaxLength(16)
                .HasColumnName("login");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Surname).HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
