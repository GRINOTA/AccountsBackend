namespace AccountsBackend.BusinessLogic.Services.CurrencyService
{
    public interface ICurrencyService
    {
        Task<List<CurrencyDto>?> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CurrencyDto?> GetByIdAsync(int currencyId, CancellationToken cancellationToken = default);   
    }
}

