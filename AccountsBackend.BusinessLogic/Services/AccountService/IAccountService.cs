namespace AccountsBackend.BusinesLogic;

public interface IAccountService
{
    Task CreateAsync(int userId, int currencyId, string number, decimal balance, CancellationToken cancellationToken = default);

    Task<List<string>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}