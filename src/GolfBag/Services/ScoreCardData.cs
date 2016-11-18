using GolfBag.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Services
{
    public interface IScoreCardData
    {
        IEnumerable<ScoreCard> GetAll();
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
        public IEnumerable<ScoreCard> GetAll()
        {
            return _scoreCards;
        }        
    }
}
