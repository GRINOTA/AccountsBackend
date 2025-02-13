using AccountsBackend.BusinessLogic.JwtGeneration;
using AccountsBackend.BusinessLogic.Services.AccountService;
using AccountsBackend.BusinessLogic.Services.AuthService;
using AccountsBackend.BusinessLogic.Services.CurrencyRatesService;
using AccountsBackend.BusinessLogic.Services.CurrencyService;
using AccountsBackend.BusinessLogic.Services.TransactionService;
using Microsoft.Extensions.DependencyInjection;

namespace AccountsBackend.BusinessLogic
{
    public static class Extensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAccountService, AccountServiceImpl>();
            serviceCollection.AddScoped<ITransactionService, TransactionServiceImpl>();
            serviceCollection.AddScoped<IAuthService, AuthServiceImpl>();
            serviceCollection.AddScoped<IJwtGen, JwtGenImpl>();
            serviceCollection.AddScoped<ICurrencyService, CurrencyServiceImpl>();
            serviceCollection.AddScoped<ICurrencyRatesService, CurrencyRatesServiceImpl>();
            return serviceCollection;
        }
    }
}