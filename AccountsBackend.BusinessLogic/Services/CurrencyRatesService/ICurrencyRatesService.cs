namespace AccountsBackend.BusinessLogic.Services.CurrencyRatesService
{
    public interface ICurrencyRatesService
    {
       public Task<CurrencyRateDto> GetCurrencyRateByIdTargerCurrency(int idTargetCurrency);
    }
}