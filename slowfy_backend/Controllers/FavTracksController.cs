using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using slowfy_backend.Data;
using slowfy_backend.Models;
using slowfy_backend.Services;

namespace slowfy_backend.Controllers
{
    public class FavTracksController : Controller
    {
        private readonly slowfy_backendContext _context;

        private readonly IUsersService _usersService;
        private readonly ITracksService _tracksService;

        public FavTracksController(slowfy_backendContext context, IUsersService usersService, ITracksService tracksService)
        {
            _context = context;
            _usersService = usersService;
            _tracksService = tracksService;
        }

        // GET: FavTracks
        public async Task<IActionResult> Index()
        {
            return Json(await _context.FavouriteTracks.ToListAsync());
        }

        // serverUrl/FavTracks/AddToFavourite     [form-data]: trackId   [auth]: bearer
        [Authorize]
        public async Task<IActionResult> AddToFavourite(int trackId = -1)
        {
            var user = await _context.User.FirstOrDefaultAsync(p => p.Email == User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("Authorize error");
            if (trackId == -1) return BadRequest("trackId is null");

            var track = await _tracksService.GetTrackById(trackId);

            var alreadyExists = _context.FavouriteTracks
                .Where(p => p.AddingUser.Id == user.Id)
                .Count(p => p.TargetTrack.Id == track.Id);

            if (alreadyExists != 0) return BadRequest("Track already is favourite!");
            
            var newFavTrack = new FavouriteTracks()
            {
                AddingUser = user,
                TargetTrack = track
            };
            _context.Add(newFavTrack);
            await _context.SaveChangesAsync();
            return Json(newFavTrack);
        }

        // serverUrl/FavTracks/RemoveFromFavourites     [form-data]: trackId   [auth]: bearer
        [Authorize]
        public async Task<IActionResult> RemoveFromFavourites(int trackId = -1)
        {
            var user = await _context.User.FirstOrDefaultAsync(p => p.Email == User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("Authorize error");
            if (trackId == -1) return BadRequest("trackId is null");

            var favTrack = await _context.FavouriteTracks
                .Where(p => p.TargetTrack.Id == trackId)
                .FirstOrDefaultAsync(p => p.AddingUser.Id == user.Id);

            if (favTrack == null) return BadRequest("Track is not a favourite");

            var result = _context.FavouriteTracks.Remove(favTrack);
            return Ok();
        }

        private bool FavouriteTracksExists(int id)
        {
          return (_context.FavouriteTracks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
