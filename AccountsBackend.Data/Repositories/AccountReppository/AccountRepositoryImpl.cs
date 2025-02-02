using AccountsBackend.Data.Context;
using AccountsBackend.Data.Models;

namespace AccountsBackend.Data;

internal class AccountRepositoryImpl(AccountsContext context) : IAccountRepository
{
    public async Task CreateAsync(Account account, CancellationToken cancellationToken = default)
    {
        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync();
    }
}