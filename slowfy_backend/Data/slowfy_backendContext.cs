using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using slowfy_backend.Models;

namespace slowfy_backend.Data
{
    public class slowfy_backendContext : DbContext
    {
        public slowfy_backendContext (DbContextOptions<slowfy_backendContext> options)
            : base(options)
        {
        }

        public DbSet<slowfy_backend.Models.User> User { get; set; } = default!;
        public DbSet<slowfy_backend.Models.Tracks> Tracks { get; set; } = default!;
        public DbSet<slowfy_backend.Models.FavouriteTracks> FavouriteTracks { get; set; } = default!;
        public DbSet<slowfy_backend.Models.Auditions> Auditions { get; set; } = default!;
    }
}
