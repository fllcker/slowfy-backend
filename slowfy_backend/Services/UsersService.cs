using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using slowfy_backend.Data;
using slowfy_backend.Models;

namespace slowfy_backend.Services;

public class UsersService : IUsersService
{
    private readonly slowfy_backendContext _dbContext;
    private readonly IConfiguration _configuration;

    public UsersService(slowfy_backendContext context, IConfiguration configuration)
    {
        _dbContext = context;
        _configuration = configuration;
    }

    public async Task<string> Create(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
        var token = CreateToken(user);
        return token;
    }

    public async Task<bool> EmailExists(string email)
    {
        var candidate = await _dbContext.User.CountAsync(p => p.Email == email);
        return candidate != 0;
    }

    public async Task<User> VerifyCredential(string email, string passwordWithoutHash)
    {
        var candidate = await _dbContext.User.FirstOrDefaultAsync(p => p.Email == email);
        if (candidate == null) throw new Exception("User not found!");
        var result = BCrypt.Net.BCrypt.Verify(passwordWithoutHash, candidate.Password);
        if (!result) throw new Exception("Wrong data!");
        return candidate;
    }

    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email)
        };
        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Config:Secret").Value));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: cred);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(p => p.Id == id);
        return user ?? throw new Exception("User not found");
    }

    public async Task<string> GetProfilePicture(string email)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(p => p.Email == email);
        if (user == null) throw new Exception("User not found");
        return user.AvatarSrc;
    }
}