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
            return Json(await _context.FavouriteTracks.ToListAsync());
        }
        
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

        private bool FavouriteTracksExists(int id)
        {
          return (_context.FavouriteTracks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
