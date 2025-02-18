using AccountsBackend.Data;
using AccountsBackend.Data.Models;
using CryptSharp;
using AutoMapper;
using AccountsBackend.BusinessLogic.JwtGeneration;
using AccountsBackend.Data.Repositories.UserRepository;
using System.Runtime.InteropServices;
using BCrypt.Net;

namespace AccountsBackend.BusinessLogic.Services.AuthService
{
    internal class AuthServiceImpl: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtGen _jwtGen;
        private readonly IMapper _mapper;

        public AuthServiceImpl(IUserRepository userRepositry, IJwtGen jwtGen, IMapper mapper)
        {
            _userRepository = userRepositry;
            _jwtGen = jwtGen;
            _mapper = mapper;
        }

        public async Task RegisterUserAsync(UserRegisterRequest userRequest, CancellationToken cancellationToken = default)
        {
            User user = new User
            {
                Surname = userRequest.Surname,
                FirstName = userRequest.FirstName,
                MiddleName = userRequest.MiddleName,
                Login = userRequest.Login,
                Password = Crypter.Blowfish.Crypt(userRequest.Password, Crypter.Blowfish.GenerateSalt(12))
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
                
            return _jwtGen.GenAccessToken(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }
    }
}

