using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace RecommendationSystem.Data
{
    public class LastFmContext : DbContext
    {
        public LastFmContext()
            : base("RecommendationSystem")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
