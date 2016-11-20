using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class ScoreCardDbContext : DbContext
    {
        public DbSet<ScoreCard> ScoreCards { get; set; }
    }
}
