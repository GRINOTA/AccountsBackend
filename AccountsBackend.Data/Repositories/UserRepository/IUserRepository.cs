using AccountsBackend.Data.Models;

namespace AccountsBackend.Data.Repositories.UserRepository
{
    public interface IUserRepository 
    {
        Task CreateUserAsync(User user, CancellationToken cancellationToken);

        Task<User?> GetUserByLoginAsync(string login, string password, CancellationToken cancellationToken);

        Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
