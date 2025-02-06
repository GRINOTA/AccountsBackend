using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinessLogic.JwtGeneration
{
    public interface IJwtGen
    {
        string Token(User user);
    }
}

