namespace slowfy_backend.Models;

public class FavouriteTracks
{
    public int Id { get; set; }
    public User AddingUser { get; set; }
    public Tracks TargetTrack { get; set; }
}