using AccountsBackend.Data.Models;

namespace AccountsBackend.Data.Repositories.CurrencyRatesRepository
{
    public interface ICurrencyRatesRepository
    {
        public Task<CurrencyRate?> GetCurrencyRateByIdTargerRate(int idTargetCurrency);      
    }
}