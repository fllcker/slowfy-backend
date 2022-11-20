using slowfy_backend.Data;
using slowfy_backend.Models;

namespace slowfy_backend.Services;

public class UsersService : IUsersService
{
    private readonly slowfy_backendContext _dbContext;

    public UsersService(slowfy_backendContext context)
    {
        _dbContext = context;
    }

    public async Task<User> Create(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
}