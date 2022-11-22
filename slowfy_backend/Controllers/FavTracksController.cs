using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
              return _context.FavouriteTracks != null ? 
                          View(await _context.FavouriteTracks.ToListAsync()) :
                          Problem("Entity set 'slowfy_backendContext.FavouriteTracks'  is null.");
        }

        // GET: FavTracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FavouriteTracks == null)
            {
                return NotFound();
            }

            var favouriteTracks = await _context.FavouriteTracks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favouriteTracks == null)
            {
                return NotFound();
            }

            return View(favouriteTracks);
        }

        // GET: FavTracks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FavTracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,AddingUser,TargetTrack")] FavTrackDto favouriteTracks)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersService.GetUserById(favouriteTracks.AddingUser);
                var track = await _tracksService.GetTrackById(favouriteTracks.TargetTrack);
                var favTrack = new FavouriteTracks()
                {
                    AddingUser = user,
                    TargetTrack = track
                };

                _context.Add(favTrack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return BadRequest("err");
            //return View(favouriteTracks);
        }

        // GET: FavTracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FavouriteTracks == null)
            {
                return NotFound();
            }

            var favouriteTracks = await _context.FavouriteTracks.FindAsync(id);
            if (favouriteTracks == null)
            {
                return NotFound();
            }
            return View(favouriteTracks);
        }

        // POST: FavTracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] FavouriteTracks favouriteTracks)
        {
            if (id != favouriteTracks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favouriteTracks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavouriteTracksExists(favouriteTracks.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(favouriteTracks);
        }

        // GET: FavTracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FavouriteTracks == null)
            {
                return NotFound();
            }

            var favouriteTracks = await _context.FavouriteTracks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favouriteTracks == null)
            {
                return NotFound();
            }

            return View(favouriteTracks);
        }

        // POST: FavTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FavouriteTracks == null)
            {
                return Problem("Entity set 'slowfy_backendContext.FavouriteTracks'  is null.");
            }
            var favouriteTracks = await _context.FavouriteTracks.FindAsync(id);
            if (favouriteTracks != null)
            {
                _context.FavouriteTracks.Remove(favouriteTracks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavouriteTracksExists(int id)
        {
          return (_context.FavouriteTracks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
