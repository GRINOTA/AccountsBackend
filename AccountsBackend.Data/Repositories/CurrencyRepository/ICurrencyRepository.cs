using AccountsBackend.Data.Models;

namespace AccountsBackend.Data;

public interface ICurrencyRepository
{
    Task<List<Currency>?> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Currency?> GetByIdAsync(int currencyId, CancellationToken cancellationToken = default);
}

