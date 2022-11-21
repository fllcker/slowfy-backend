using slowfy_backend.Models;

namespace slowfy_backend.Services;

public interface IUsersService
{
    public Task<User> Create(User user);
    public Task<bool> EmailExists(string email);
    public Task<bool> VerifyCredential(string email, string passwordWithoutHash);
}