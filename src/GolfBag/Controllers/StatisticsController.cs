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
        private const int EIGHTEEN = 18;

        public StatisticsController(IRoundOfGolf roundOfGolf, UserManager<User> userManager)
        {
            _roundOfGolf = roundOfGolf;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetScores(int holes, int mostRecentScores = 0)
        {
            var rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id).ToList();

            var scores = new List<BarChartRound>();

            switch (holes)
            {
                case NINE:
                    scores = GetNineHoleScores(rounds);
                    break;
                case EIGHTEEN:
                    scores = GetEighteenHoleScores(rounds);
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

        [HttpGet]
        public IActionResult GetScoresToPar(int mostRecentScores = 0)
        {
            IEnumerable<RoundOfGolf> rounds;

            if (mostRecentScores > 0)
            {
                var roundList = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id)
                    .ToList();
                rounds = roundList.Skip(roundList.Count - mostRecentScores);
            }
            else
            {
                rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id).ToList();
            }

            return Json(CalculateScoresToPar(rounds));
        }

        [HttpGet]
        public IActionResult GetPutts(int holes, int mostRecentScores = 0)
        {
            var rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id).ToList();

            var putts = new List<LineGraphPutts>();

            switch (holes)
            {
                case NINE:
                    putts = GetNineHolePutts(rounds);
                    break;
                case EIGHTEEN:
                    putts = GetEighteenHolePutts(rounds);
                    break;
                default:
                    putts = null;
                    break;
            }

            if (putts.Count > mostRecentScores)
            {
                return Json(putts.Skip(putts.Count - mostRecentScores));
            }

            return Json(putts);
        }

        [HttpGet]
        public IActionResult GetPuttsToTwo(int mostRecentScores)
        {
            IEnumerable<RoundOfGolf> rounds;

            if (mostRecentScores > 0)
            {
                var roundList = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id)
                    .ToList();
                rounds = roundList.Skip(roundList.Count - mostRecentScores);
            }
            else
            {
                rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id).ToList();
            }

            return Json(CalculatePuttsToTwo(rounds));
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
                    barChartRound.RoundDate  = round.Date.ToString("MMMM d, yyyy");
                    barChartRound.CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName;
                    scores.Add(barChartRound);
                }
                else if (round.Scores.Count == EIGHTEEN)
                {
                    var frontNineBarChartRound = new BarChartRound();
                    var backNineBarChartRound  = new BarChartRound();

                    frontNineBarChartRound.RoundScore = round.Scores.Select(x => x.HoleScore).Take(NINE).Sum();
                    frontNineBarChartRound.RoundId    = round.Id;
                    frontNineBarChartRound.RoundDate  = round.Date.ToString("MMMM d, yyyy");
                    frontNineBarChartRound.CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName;
                    backNineBarChartRound.RoundScore  = round.Scores.Select(x => x.HoleScore).Skip(NINE).Sum();
                    backNineBarChartRound.RoundId     = round.Id;
                    backNineBarChartRound.RoundDate   = round.Date.ToString("MMMM d, yyyy");
                    backNineBarChartRound.CourseName  = _roundOfGolf.GetCourse(round.CourseId).CourseName;
                    scores.Add(frontNineBarChartRound);
                    scores.Add(backNineBarChartRound);
                }               
            }

            return scores;
        }

        private List<BarChartRound> GetEighteenHoleScores(IEnumerable<RoundOfGolf> rounds)
        {
            var scores = new List<BarChartRound>();

            foreach (var round in rounds)
            {
                if (round.Scores.Count == EIGHTEEN)
                {
                    var barChartRound = new BarChartRound();
                    barChartRound.RoundScore = round.Scores.Select(x => x.HoleScore).Sum();
                    barChartRound.RoundId    = round.Id;
                    barChartRound.RoundDate  = round.Date.ToString("MMMM d, yyyy");
                    barChartRound.CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName;
                    scores.Add(barChartRound);
                }
            }

            return scores;
        }

        private List<LineGraphPutts> GetNineHolePutts(IEnumerable<RoundOfGolf> rounds)
        {
            var putts = new List<LineGraphPutts>();

            foreach (var round in rounds)
            {
                if (round.Scores.Count == NINE)
                {
                    var lineGraphPutts = new LineGraphPutts
                    {
                        Putts      = round.Scores.Select(x => x.HolePutt).Sum(),
                        RoundId    = round.Id,
                        RoundDate  = round.Date.ToString("MMMM d, yyyy"),
                        CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName,
                    };
                    putts.Add(lineGraphPutts);
                }
                else if (round.Scores.Count == EIGHTEEN)
                {
                    var frontNineLineGraphPutts = new LineGraphPutts
                    {
                        Putts      = round.Scores.Select(x => x.HolePutt).Take(NINE).Sum(),
                        RoundId    = round.Id,
                        RoundDate  = round.Date.ToString("MMMM d, yyyy"),
                        CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName,
                    };

                    var backNineLineGraphPutts = new LineGraphPutts
                    {
                        Putts      = round.Scores.Select(x => x.HolePutt).Skip(NINE).Sum(),
                        RoundId    = round.Id,
                        RoundDate  = round.Date.ToString("MMMM d, yyyy"),
                        CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName,
                    };
                    putts.Add(frontNineLineGraphPutts);
                    putts.Add(backNineLineGraphPutts);
                }
            }
            return putts;
        }

        private List<LineGraphPutts> GetEighteenHolePutts(IEnumerable<RoundOfGolf> rounds)
        {
            var putts = new List<LineGraphPutts>();

            foreach (var round in rounds)
            {
                if (round.Scores.Count == EIGHTEEN)
                {
                    var lineGraphPutts = new LineGraphPutts
                    {
                        Putts      = round.Scores.Select(x => x.HolePutt).Sum(),
                        RoundId    = round.Id,
                        RoundDate  = round.Date.ToString("MMMM d, yyyy"),
                        CourseName = _roundOfGolf.GetCourse(round.CourseId).CourseName,
                    };
                    putts.Add(lineGraphPutts);
                }
            }
            return putts;
        }

        private List<ScoreToPar> CalculateScoresToPar(IEnumerable<RoundOfGolf> rounds)
        {
            float eagles       = 0;
            float birdies      = 0;
            float pars         = 0;
            float bogies       = 0;
            float doubleBogies = 0;
            float others       = 0;
            float totalHoles   = 0;

            foreach (var round in rounds)
            {
                var course = _roundOfGolf.GetCourse(round.CourseId);

                foreach (var score in round.Scores)
                {
                    int par = course.CourseHoles
                        .Where(x => x.HoleNumber == score.HoleNumber)
                        .FirstOrDefault()
                        .Par;

                    int difference = score.HoleScore - par;

                    switch (difference)
                    {
                        case -2:
                            eagles++;
                            break;
                        case -1:
                            birdies++;
                            break;
                        case 0:
                            pars++;
                            break;
                        case 1:
                            bogies++;
                            break;
                        case 2:
                            doubleBogies++;
                            break;
                        default:
                            others++;
                            break;
                    }
                    totalHoles++;
                }
            }

            var scoresToPar = new List<ScoreToPar>();
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName  = "eagle",
                Percentage = eagles / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName  = "birdie",
                Percentage = birdies / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName  = "par",
                Percentage = pars / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName  = "bogie",
                Percentage = bogies / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName  = "double-bogie",
                Percentage = doubleBogies / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName  = "other",
                Percentage = others / totalHoles * 100
            });

            return scoresToPar;
        }

        private List<PuttToTwo> CalculatePuttsToTwo(IEnumerable<RoundOfGolf> rounds)
        {
            float chipIns    = 0;
            float onePutts   = 0;
            float twoPutts   = 0;
            float threePutts = 0;
            float fourPutts  = 0;
            float otherPutts = 0;
            float totalHoles = 0;

            foreach (var round in rounds)
            {
                foreach (var score in round.Scores)
                {
                    switch (score.HolePutt)
                    {
                        case 0:
                            chipIns++;
                            break;
                        case 1:
                            onePutts++;
                            break;
                        case 2:
                            twoPutts++;
                            break;
                        case 3:
                            threePutts++;
                            break;
                        case 4:
                            fourPutts++;
                            break;
                        default:
                            otherPutts++;
                            break;
                    }
                    totalHoles++;
                }
            }

            var puttsToTwo = new List<PuttToTwo>();
            puttsToTwo.Add(new PuttToTwo
            {
                PuttName = "chip-in",
                Percentage = chipIns / totalHoles * 100
            });
            puttsToTwo.Add(new PuttToTwo
            {
                PuttName = "one-putt",
                Percentage = onePutts / totalHoles * 100
            });
            puttsToTwo.Add(new PuttToTwo
            {
                PuttName = "two-putt",
                Percentage = twoPutts / totalHoles * 100
            });
            puttsToTwo.Add(new PuttToTwo
            {
                PuttName = "three-putt",
                Percentage = threePutts / totalHoles * 100
            });
            puttsToTwo.Add(new PuttToTwo
            {
                PuttName = "four-putt",
                Percentage = fourPutts/ totalHoles * 100
            });
            puttsToTwo.Add(new PuttToTwo
            {
                PuttName = "other",
                Percentage = otherPutts / totalHoles * 100
            });

            return puttsToTwo;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }


    }
}
