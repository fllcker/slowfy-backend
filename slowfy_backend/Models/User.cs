namespace slowfy_backend.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Hashed password (bcrypt)
    public string Name { get; set; } // Username
    public string AvatarSrc { get; set; } // Link to avatar image on server
}