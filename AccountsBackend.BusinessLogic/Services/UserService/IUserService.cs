using AccountsBackend.Data;
using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinesLogic;

public interface IUserService
{
    Task CreateUserAsync(string surname, string firstName, string? middleName, string login, string password, CancellationToken cancellationToken = default);
    Task<User?> GetUserByLoginAsync(string login, string password, CancellationToken cancellationToken = default);
}