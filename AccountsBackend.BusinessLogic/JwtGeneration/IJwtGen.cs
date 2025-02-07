using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinessLogic.JwtGeneration
{
    public interface IJwtGen
    {
        string GenAccessToken(User user);
        string GetRefreshToken();
    }
}

