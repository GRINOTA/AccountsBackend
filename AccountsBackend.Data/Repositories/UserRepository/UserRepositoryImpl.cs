using AccountsBackend.Data.Context;
using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data;

internal class UserRepositryImpl(AccountsContext context) : IUserRepository
{
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync();
    }   

    public async Task<User?> GetUserByLoginAsync(string login, string password, CancellationToken cancellationToken = default)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login);
    } 
}