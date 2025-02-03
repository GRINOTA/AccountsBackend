using AccountsBackend.Data.Context;
using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data;

internal class AccountRepositoryImpl(AccountsContext context) : IAccountRepository
{
    public async Task CreateAsync(Account account, CancellationToken cancellationToken = default)
    {
        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync();
    }

    public async Task<List<Account?>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default) 
    {
        return await context.Accounts.Where(a => a.UserId == userId).ToListAsync(cancellationToken);
    }
}