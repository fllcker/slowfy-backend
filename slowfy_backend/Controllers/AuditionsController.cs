using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using slowfy_backend.Data;
using slowfy_backend.Models;
using slowfy_backend.Services;

namespace slowfy_backend.Controllers
{
    public class AuditionsController : Controller
    {
        private readonly slowfy_backendContext _context;
        private readonly ITracksService _tracksService;
        
        public AuditionsController(slowfy_backendContext context, ITracksService tracksService)
        {
            _context = context;
            _tracksService = tracksService;
        }
        
        [Authorize]
        public async Task<IActionResult> AddAudition(int trackId = -1)
        {
            var user = await _context.User.FirstOrDefaultAsync(p => p.Email == User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("Authorize error");
            if (trackId == -1) return BadRequest("trackId is null");

            var track = await _tracksService.GetTrackById(trackId);

            var audition = new Auditions()
            {
                Track = track,
                Listener = user
            };
            _context.Add(audition);
            await _context.SaveChangesAsync();
            return Json(audition);
        }

        public async Task<IActionResult> CountAuditionsOnTrack(int trackId = -1)
        {
            return Json(await _context.Auditions.CountAsync(p => p.Track.Id == trackId));
        }

        public async Task<IActionResult> CountAllAuditions()
        {
            return Json(await _context.Auditions.CountAsync());
        }
    }
}