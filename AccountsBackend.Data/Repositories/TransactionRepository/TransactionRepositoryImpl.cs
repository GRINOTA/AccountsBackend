using AccountsBackend.Data.DataContext;
using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data;

internal class TransactionRepositoryImpl(AccountsContext context) : ITransactionRepository
{
    public async Task CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
       await context.AddAsync(transaction, cancellationToken);
       await context.SaveChangesAsync();
    }    
}