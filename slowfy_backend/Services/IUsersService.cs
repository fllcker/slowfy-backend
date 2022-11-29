using slowfy_backend.Models;

namespace slowfy_backend.Services;

public interface IUsersService
{
    public Task<string> Create(User user);
    public Task<bool> EmailExists(string email);
    public Task<User> VerifyCredential(string email, string passwordWithoutHash);
    public Task<User> GetUserById(int id);
    public string CreateToken(User user);
    public Task<string> GetProfilePicture(string email);
}