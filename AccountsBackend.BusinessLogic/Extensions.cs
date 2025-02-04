using Microsoft.Extensions.DependencyInjection;

namespace AccountsBackend.BusinesLogic;

public static class Extensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountService, AccountServiceImpl>();
        serviceCollection.AddScoped<ITransactionService, TransactionServiceImpl>();
        // serviceCollection.AddScoped<IUserService, UserServiceImpl>();
        return serviceCollection;
    }
}