using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Services
{
    public interface IScoreCardData
    {
        IEnumerable<ScoreCard> GetAll();
        ScoreCard Get(int id);
        void Add(ScoreCard newScoreCard);
    }

    public class SqlScoreCardData : IScoreCardData
    {
        private ScoreCardDbContext _context;

        public SqlScoreCardData(ScoreCardDbContext context)
        {
            _context = context;
        }
        public void Add(ScoreCard newScoreCard)
        {
            _context.Add(newScoreCard);
            _context.SaveChanges();
        }

        public ScoreCard Get(int id)
        {
            return _context.ScoreCards.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<ScoreCard> GetAll()
        {
            return _context.ScoreCards.ToList();
        }
    }
    public class ScoreCardData : IScoreCardData
    {
        List<ScoreCard> _scoreCards;
        public ScoreCardData()
        {
            _scoreCards = new List<ScoreCard>
            {
                new ScoreCard {Id = 1, CourseName = "Blacks", PlayerName = "Bill" },
                new ScoreCard {Id = 2, CourseName = "Doosie", PlayerName = "Passersbye" }
            };
        }

        public void Add(ScoreCard newScoreCard)
        {
            newScoreCard.Id = _scoreCards.Max(r => r.Id) + 1;
            _scoreCards.Add(newScoreCard);
        }

        public ScoreCard Get(int id)
        {
            return _scoreCards.FirstOrDefault(r => r.Id == id);
        }
        public IEnumerable<ScoreCard> GetAll()
        {
            return _scoreCards;
        }        
    }
}
