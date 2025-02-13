using AccountsBackend.Data.Repositories.CurrencyRatesRepository;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.CurrencyRatesService
{
    internal class CurrencyRatesServiceImpl : ICurrencyRatesService
    {
        private readonly IMapper _mapper;
        private readonly ICurrencyRatesRepository _repository;
        public CurrencyRatesServiceImpl(IMapper mapper, ICurrencyRatesRepository repository) 
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<CurrencyRateDto> GetCurrencyRateByIdTargerCurrency(int idTargetCurrency)
        {
            var currencyRate = await _repository.GetCurrencyRateByIdTargerRate(idTargetCurrency);
            return _mapper.Map<CurrencyRateDto>(currencyRate);
        }
    }
}