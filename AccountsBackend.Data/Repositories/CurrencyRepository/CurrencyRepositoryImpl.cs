using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
using AccountsBackend.Data.DataContext;

namespace AccountsBackend.Data.Repositories.CurrencyRepository
{
    internal class CurrencyRepositoryImpl(AccountsContext context): ICurrencyRepository
    {

        public async Task<List<Currency>?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Currencies.ToListAsync(cancellationToken);
        }

        public async Task<Currency?> GetByIdAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            return await context.Currencies.FirstOrDefaultAsync(c => c.Id == currencyId);
        }
    }
}

