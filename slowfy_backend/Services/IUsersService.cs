using slowfy_backend.Models;

namespace slowfy_backend.Services;

public interface IUsersService
{
    public Task<User> Create(User user);
}