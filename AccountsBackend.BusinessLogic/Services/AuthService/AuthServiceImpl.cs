using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountsBackend.Data;
using AccountsBackend.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CryptSharp;

namespace AccountsBackend.BusinesLogic;

internal class AuthServiceImpl: IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGen _jwtGen;

    public AuthServiceImpl(IUserRepository userRepositry, IJwtGen jwtGen)
    {
        _userRepository = userRepositry;
        _jwtGen = jwtGen;
    }

    public async Task RegisterUserAsync(string surname, string firstName, string? middleName, string login, string password, CancellationToken cancellationToken = default)
    {

        User user = new User
        {
            Surname = surname,
            FirstName = firstName,
            MiddleName = middleName,
            Login = login,
            Password = Crypter.Blowfish.Crypt(password, Crypter.Blowfish.GenerateSalt(12))
        };

        await _userRepository.CreateUserAsync(user, cancellationToken);
    }

    public async Task<string?> LoginUserAsync(UserRequest userRequest, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrWhiteSpace(userRequest.Login) || string.IsNullOrWhiteSpace(userRequest.Password))
            return null;

        var user = await _userRepository.GetUserByLoginAsync(userRequest.Login, userRequest.Password, cancellationToken);

        if (user is null || Crypter.CheckPassword(userRequest.Password, user.Password) == false)
            return null;

        return _jwtGen.Token(user);
    }
}