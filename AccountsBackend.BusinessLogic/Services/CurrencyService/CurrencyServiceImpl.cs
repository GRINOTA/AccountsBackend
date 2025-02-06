using AccountsBackend.Data;
using AccountsBackend.Data.Repositories.CurrencyRepository;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.CurrencyService
{
    internal class CurrencyServiceImpl: ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyServiceImpl(ICurrencyRepository currencyRepository, IMapper mapper) 
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDto>?> GetAllAsync(CancellationToken cancellationToken = default) 
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return _mapper.Map<List<CurrencyDto>>(currencies);
        }

        public async Task<CurrencyDto?> GetByIdAsync(int currencyId, CancellationToken cancellationToken = default)
        {
            var currency = await _currencyRepository.GetByIdAsync(currencyId);
            return _mapper.Map<CurrencyDto>(currency);
        }
    }
}

