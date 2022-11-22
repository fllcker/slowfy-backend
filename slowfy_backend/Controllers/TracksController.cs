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
    }
}
