using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class ScoreCardDbContext : IdentityDbContext<User>
    {
        private IConfigurationRoot _config;

        public ScoreCardDbContext(IConfigurationRoot config, DbContextOptions options) : base (options)
        {
            _config = config;
        }
        public DbSet<RoundOfGolf> RoundsOfGolf { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["database:connection"]);
        }
    }
}
