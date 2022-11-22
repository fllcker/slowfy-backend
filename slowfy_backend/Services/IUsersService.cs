using slowfy_backend.Models;

namespace slowfy_backend.Services;

public interface IUsersService
{
    public Task<string> Create(User user);
    public Task<bool> EmailExists(string email);
    public Task<bool> VerifyCredential(string email, string passwordWithoutHash);
    public Task<User> GetUserById(int id);
}