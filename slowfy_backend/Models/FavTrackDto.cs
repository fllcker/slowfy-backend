namespace slowfy_backend.Models;

public class FavTrackDto
{
    public int Id { get; set; }
    public int AddingUser { get; set; }
    public int TargetTrack { get; set; }
}