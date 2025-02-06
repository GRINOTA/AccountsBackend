using AccountsBackend.Data.DataContext;
using AccountsBackend.Data.Repositories.AccountRepository;
using AccountsBackend.Data.Repositories.CurrencyRepository;
using AccountsBackend.Data.Repositories.TransactionRepository;
using AccountsBackend.Data.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountsBackend.Data;

public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration) 
    {
        serviceCollection.AddDbContext<AccountsContext>( x =>
        {
            var connectionString = configuration.GetConnectionString("AccountDb"); 
            x.UseNpgsql(connectionString);
        });
        serviceCollection.AddScoped<IAccountRepository, AccountRepositoryImpl>();
        serviceCollection.AddScoped<ITransactionRepository, TransactionRepositoryImpl>();
        serviceCollection.AddScoped<IUserRepository, UserRepositryImpl>();
        serviceCollection.AddScoped<ICurrencyRepository, CurrencyRepositoryImpl>();

        return serviceCollection;
    }
}