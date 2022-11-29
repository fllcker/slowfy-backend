using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using slowfy_backend.Data;
using slowfy_backend.Models;

namespace slowfy_backend.Controllers
{
    public class TracksController : Controller
    {
        private readonly slowfy_backendContext _context;

        public TracksController(slowfy_backendContext context)
        {
            _context = context;
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
            return Json(await _context.Tracks.ToListAsync());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Author,Title,Duration")] Tracks tracks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tracks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return BadRequest("error");
        }

        private bool TracksExists(int id)
        {
            return (_context.Tracks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> GetMostPopularTracks(int count = 10)
        {
            var res = _context.Auditions.GroupBy(p => p.Track);
            
            var dic = new List<TrackAudDto>();
            
            foreach (var VARIABLE in res)
            {
                int countOfAud = VARIABLE.Count();
                dic.Add(new TrackAudDto()
                {
                    AudCount = countOfAud,
                    Track = VARIABLE.Key
                });
            }

            return Json(dic.OrderByDescending(p => p.AudCount).Select(p => p.Track).Take(count).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetTracksByAuthor(string author)
        {
            return Json(await _context.Tracks.Where(p => p.Author == author).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetTrackById(int id)
        {
            var tracks = await _context.Tracks.FirstOrDefaultAsync(p => p.Id == id);
            return tracks != null ? Json(tracks) : BadRequest("none");
        }
    }
}
