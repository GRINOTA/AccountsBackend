using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinesLogic;

public interface IAccountService
{
    Task CreateAsync(int userId, int currencyId, CancellationToken cancellationToken = default);

    Task<List<AccountDto>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<AboutAccountDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}