using Microsoft.EntityFrameworkCore;
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

    public async Task<bool> EmailExists(string email)
    {
        var candidate = await _dbContext.User.CountAsync(p => p.Email == email);
        return candidate != 0;
    }

    public async Task<bool> VerifyCredential(string email, string passwordWithoutHash)
    {
        var candidate = await _dbContext.User.FirstOrDefaultAsync(p => p.Email == email);
        if (candidate == null) throw new Exception("User not found!");
        var result = BCrypt.Net.BCrypt.Verify(passwordWithoutHash, candidate.Password);
        if (!result) throw new Exception("Wrong data!");
        return result;
    }
}