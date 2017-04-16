using GolfBag.Entities;
using GolfBag.Entities.Statistics;
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
            var scores = new List<BarChartRound>();

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

        private List<BarChartRound> GetNineHoleScores(IEnumerable<RoundOfGolf> rounds)
        {
            var scores = new List<BarChartRound>();

            foreach (var round in rounds)
            {              
                if (round.Scores.Count == NINE)
                {
                    var barChartRound = new BarChartRound();

                    barChartRound.RoundScore = round.Scores.Select(x => x.HoleScore).Sum();
                    barChartRound.RoundId    = round.Id;
                    barChartRound.RoundDate  = round.Date;
                    barChartRound.CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName;
                    scores.Add(barChartRound);
                }
                else if (round.Scores.Count == EIGHTTEEN)
                {
                    var frontNineBarChartRound = new BarChartRound();
                    var backNineBarChartRound  = new BarChartRound();

                    frontNineBarChartRound.RoundScore = round.Scores.Select(x => x.HoleScore).Take(NINE).Sum();
                    frontNineBarChartRound.RoundId    = round.Id;
                    frontNineBarChartRound.RoundDate  = round.Date;
                    frontNineBarChartRound.CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName;
                    backNineBarChartRound.RoundScore  = round.Scores.Select(x => x.HoleScore).Skip(NINE).Sum();
                    backNineBarChartRound.RoundId     = round.Id;
                    backNineBarChartRound.RoundDate   = round.Date;
                    backNineBarChartRound.CourseName  = _roundOfGolf.GetCourse(round.CourseId).CourseName;
                    scores.Add(frontNineBarChartRound);
                    scores.Add(backNineBarChartRound);
                }               
            }

            return scores;
        }

        private List<BarChartRound> GetEightteenHoleScores(IEnumerable<RoundOfGolf> rounds)
        {
            var scores = new List<BarChartRound>();

            foreach (var round in rounds)
            {
                if (round.Scores.Count == EIGHTTEEN)
                {
                    var barChartRound = new BarChartRound();
                    barChartRound.RoundScore = round.Scores.Select(x => x.HoleScore).Sum();
                    scores.Add(barChartRound);
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
