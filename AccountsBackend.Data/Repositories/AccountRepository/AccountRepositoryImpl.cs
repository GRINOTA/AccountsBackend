using AccountsBackend.Data.DataContext;
using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data.Repositories.AccountRepository
{
    internal class AccountRepositoryImpl(AccountsContext context) : IAccountRepository
    {
        public async Task CreateAsync(Account account, CancellationToken cancellationToken = default)
        {
            await context.Accounts.AddAsync(account, cancellationToken);
            await context.SaveChangesAsync();
        }

        public async Task<List<Account>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default) 
        {
            return await context.Accounts
                .Include(a => a.Currency)
                .Where(a => a.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Account?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
