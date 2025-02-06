using AccountsBackend.Data.Models;

namespace AccountsBackend.Data;

public interface ITransactionRepository
{
    Task CreateAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task<List<Transaction>?> GetByUserIdAsync (int userId, CancellationToken cancellationToken = default);
}