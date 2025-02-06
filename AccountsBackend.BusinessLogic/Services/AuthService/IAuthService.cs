namespace AccountsBackend.BusinessLogic.Services.AuthService
{
    public interface IAuthService
    {
        Task RegisterUserAsync(string surname, string firstName, string? middleName, string login, string password, CancellationToken cancellationToken = default);
        Task<string?> LoginUserAsync(UserRequest user, CancellationToken cancellationToken = default);
        Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
