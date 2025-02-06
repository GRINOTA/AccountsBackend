using AccountsBackend.Data.DataContext;
using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data.Repositories.UserRepository
{
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

        public async Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}

