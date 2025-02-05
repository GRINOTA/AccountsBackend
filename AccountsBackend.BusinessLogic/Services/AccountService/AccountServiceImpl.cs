using AccountsBackend.Data;
using AccountsBackend.Data.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AccountsBackend.BusinesLogic;

internal class AccountServiceImpl : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountServiceImpl(IAccountRepository accountRepository, IMapper mapper) 
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }
    
    public async Task CreateAsync(int userId, int currencyId, string number, CancellationToken cancellationToken = default)
    {
        var account = new Account
        {
            UserId = userId,
            CurrencyId = currencyId,
            Number = number,
            Balance = 1000
        };

        await _accountRepository.CreateAsync(account, cancellationToken);
    }

    public async Task<List<AccountDto>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default) 
    {
        var accounts = await _accountRepository.GetByUserIdAsync(userId, cancellationToken);
        
        return _mapper.Map<List<AccountDto>>(accounts);
    }
}