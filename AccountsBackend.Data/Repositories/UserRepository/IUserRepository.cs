using AccountsBackend.Data.Models;

namespace AccountsBackend.Data;

public interface IUserRepository 
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken);

    Task<User?> GetUserByLoginAsync(string login, string password, CancellationToken cancellationToken);
}