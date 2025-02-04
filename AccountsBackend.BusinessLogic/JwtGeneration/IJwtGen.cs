using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinesLogic;

public interface IJwtGen
{
    string Token(User user);
}