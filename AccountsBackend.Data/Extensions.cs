using AccountsBackend.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountsBackend.Data;

public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration) 
    {
        serviceCollection.AddScoped<IAccountRepository, AccountRepositoryImpl>();
        serviceCollection.AddScoped<ITransactionRepository, TransactionRepositoryImpl>();
        serviceCollection.AddScoped<IUserRepository, UserRepositryImpl>();
        serviceCollection.AddDbContext<AccountsContext>( x =>
        {
            var connectionString = configuration.GetConnectionString("AccountDb"); 
            x.UseNpgsql(connectionString);
        });

        return serviceCollection;
    }
}