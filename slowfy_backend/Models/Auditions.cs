namespace slowfy_backend.Models;

public class Auditions
{
    public int Id { get; set; }
    public Tracks Track { get; set; }
    public User Listener { get; set; }
}