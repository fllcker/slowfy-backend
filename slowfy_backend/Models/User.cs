using System.ComponentModel.DataAnnotations;

namespace slowfy_backend.Models;

public class User
{
    public int Id { get; set; }
    [EmailAddress]
    [MaxLength(64)]
    public string Email { get; set; }
    [MinLength(8)]
    public string Password { get; set; } // Hashed password (bcrypt)
    [MaxLength(32)]
    public string Name { get; set; } // Username
    public string AvatarSrc { get; set; } // Link to avatar image on server
}