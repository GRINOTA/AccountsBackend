using AccountsBackend.Data;
using AccountsBackend.Data.Models;
using AccountsBackend.Data.Repositories.AccountRepository;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.AccountService
{
    internal class AccountServiceImpl : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountServiceImpl(IAccountRepository accountRepository, IMapper mapper) 
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        
        public async Task CreateAsync(int userId, int currencyId, CancellationToken cancellationToken = default)
        {
            string timePart = DateTime.Now.Ticks.ToString();
            Random random = new Random();
            string randomPart = new string(Enumerable.Repeat("0123456789", 20 - timePart.Length).Select(s => s[random.Next(s.Length)]).ToArray());
            var account = new Account
            {
                UserId = userId,
                CurrencyId = currencyId,
                Number = timePart + randomPart,
                Balance = 1000
            };

            await _accountRepository.CreateAsync(account, cancellationToken);
        }

        public async Task<List<AccountDto>?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default) 
        {
            var accounts = await _accountRepository.GetByUserIdAsync(userId, cancellationToken);
            
            return _mapper.Map<List<AccountDto>>(accounts);
        }

        public async Task<AboutAccountDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return _mapper.Map<AboutAccountDto>(account);
        }
    }
}