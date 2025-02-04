using AccountsBackend.Data;
using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinesLogic;

internal class UserServiceImpl: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserServiceImpl(IUserRepository userRepositry)
    {
        _userRepository = userRepositry;
    }

    public async Task CreateUserAsync(string surname, string firstName, string? middleName, string login, string password, CancellationToken cancellationToken = default)
    {
        User user = new User
        {
            Surname = surname,
            FirstName = firstName,
            MiddleName = middleName,
            Login = login,
            Password = password
        };

        await _userRepository.CreateUserAsync(user, cancellationToken);
    }

    public async Task<User?> GetUserByLoginAsync(string login, string password, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetUserByLoginAsync(login, cancellationToken);
    }
}