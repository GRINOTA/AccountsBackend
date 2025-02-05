namespace AccountsBackend.BusinesLogic;

public interface IAccountService
{
    Task CreateAsync(int userId, int currencyId, string number, CancellationToken cancellationToken = default);

    Task<List<AccountDto>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}