using AccountsBackend.Data.Models;

namespace AccountsBackend.Data;

public interface IAccountRepository
{
    Task CreateAsync(Account account, CancellationToken cancellationToken = default); 
}