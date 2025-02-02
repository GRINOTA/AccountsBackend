using AccountsBackend.Data;
using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinesLogic;

internal class AccountServiceImpl(IAccountRepository accountRepository) : IAccountService
{
    public async Task CreateAsync(int userId, int currencyId, string number, decimal balance, CancellationToken cancellationToken = default)
    {
        var account = new Account
        {
            UserId = userId,
            CurrencyId = currencyId,
            Number = number,
            Balance = balance
        };

        await accountRepository.CreateAsync(account, cancellationToken);
    }
}