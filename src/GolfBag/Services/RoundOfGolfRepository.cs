using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfBag.Entities;

namespace GolfBag.Services
{
    public class RoundOfGolfRepository : IRoundOfGolf
    {
        private ScoreCardDbContext _context;

        public RoundOfGolfRepository(ScoreCardDbContext context)
        {
            _context = context;
        }
        public void Add(RoundOfGolf newRoundOfGolf)
        {
            _context.Add(newRoundOfGolf);
            Commit();
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Delete(RoundOfGolf roundOfGolf)
        {
            _context.Remove(roundOfGolf);
            Commit();
        }

        public IEnumerable<RoundOfGolf> GetAll(string playerName)
        {
            return _context.RoundsOfGolf.Where(r => r.PlayerName == playerName);
        }
    }
}
