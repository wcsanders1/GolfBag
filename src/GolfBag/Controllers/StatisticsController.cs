using GolfBag.Entities;
using GolfBag.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Controllers
{
    public class StatisticsController : Controller
    {
        private IRoundOfGolf _roundOfGolf;
        private UserManager<User> _userManager;

        private const int NINE      = 9;
        private const int EIGHTTEEN = 18;

        public StatisticsController(IRoundOfGolf roundOfGolf, UserManager<User> userManager)
        {
            _roundOfGolf = roundOfGolf;
            _userManager = userManager;
        }

        [HttpGet]
        public JsonResult GetScores(int holes, int mostRecentScores = 0)
        {            
            var rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id).ToList();            
            var scores = new List<int>();

            switch (holes)
            {
                case NINE:
                    scores = GetNineHoleScores(rounds);
                    break;
                case EIGHTTEEN:
                    scores = GetEightteenHoleScores(rounds);
                    break;
                default:
                    scores = null;
                    break;
            }

            if (scores.Count > mostRecentScores)
            {
                return Json(scores.Skip(scores.Count - mostRecentScores));
            }

            return Json(scores);
        }

        private List<int> GetNineHoleScores(IEnumerable<RoundOfGolf> rounds)
        {
            var scores = new List<int>();

            foreach (var round in rounds)
            {
                if (round.Scores.Count == NINE)
                {
                    scores.Add(round.Scores.Select(x => x.HoleScore).Sum());
                }
                else if (round.Scores.Count == EIGHTTEEN)
                {
                    scores.Add(round.Scores.Select(x => x.HoleScore).Take(NINE).Sum());
                    scores.Add(round.Scores.Select(x => x.HoleScore).Skip(NINE).Sum());
                }               
            }

            return scores;
        }

        private List<int> GetEightteenHoleScores(IEnumerable<RoundOfGolf> rounds)
        {
            var scores = new List<int>();

            foreach (var round in rounds)
            {
                if (round.Scores.Count == EIGHTTEEN)
                {
                    scores.Add(round.Scores.Select(x => x.HoleScore).Sum());
                }
            }

            return scores;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }


    }
}
