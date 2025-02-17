using AccountsBackend.Data.Models;

namespace AccountsBackend.Data.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        Task CreateAsync(Account account, CancellationToken cancellationToken = default); 

        Task<List<Account>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        Task<Account?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}

