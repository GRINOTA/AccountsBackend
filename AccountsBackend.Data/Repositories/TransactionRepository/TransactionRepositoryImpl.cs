using AccountsBackend.Data.DataContext;
using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data.Repositories.TransactionRepository
{
    internal class TransactionRepositoryImpl(AccountsContext context) : ITransactionRepository
    {
        public async Task CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
        await context.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync();
        }

        public async Task<List<Transaction>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var transactions = await context.Transactions
                .Include(t => t.RecipientAccount)
                .Include(t => t.SenderAccount)
                .Include(t => t.RecipientAccount.Currency)
                .Include(t => t.SenderAccount.Currency)
                .Where(t => t.SenderAccount.UserId == userId || t.RecipientAccount.UserId == userId)
                .ToListAsync(cancellationToken);

            return transactions;
        }
    }
}
