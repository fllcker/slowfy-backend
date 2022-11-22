using Microsoft.EntityFrameworkCore;
using slowfy_backend.Data;
using slowfy_backend.Models;

namespace slowfy_backend.Services;

public class TracksService : ITracksService
{
    private readonly slowfy_backendContext _context;
    
    public TracksService(slowfy_backendContext context)
    {
        _context = context;
    }

    public async Task<Tracks> GetTrackById(int id)
    {
        var track = await _context.Tracks.FirstOrDefaultAsync(p => p.Id == id);
        return track ?? throw new Exception("Track not found");
    }
}