using slowfy_backend.Models;

namespace slowfy_backend.Services;

public interface ITracksService
{
    public Task<Tracks> GetTrackById(int id);
}