using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountsBackend.Data;
using AccountsBackend.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AccountsBackend.BusinesLogic;

internal class UserServiceImpl: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGen _jwtGen;

    public UserServiceImpl(IUserRepository userRepositry, IJwtGen jwtGen)
    {
        _userRepository = userRepositry;
        _jwtGen = jwtGen;

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

    public async Task<string?> GetUserByLoginAsync(UserRequest userRequest, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrWhiteSpace(userRequest.Login) || string.IsNullOrWhiteSpace(userRequest.Password))
            return null;

        var user = await _userRepository.GetUserByLoginAsync(userRequest.Login, userRequest.Password, cancellationToken);

        if (user is null)
            return null;

        return _jwtGen.Token(user);
    }
}