using AccountsBackend.Data.Models;

namespace AccountsBackend.Data;

public interface IAccountRepository
{
    Task CreateAsync(Account account, CancellationToken cancellationToken = default); 

    Task<List<Account?>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}