﻿using GolfBag.Entities;
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
        public IActionResult GetScores(int holes, int mostRecentScores = 0)
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
                else if (round.Scores.Count == EIGHTTEEN)
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

        private List<BarChartRound> GetEightteenHoleScores(IEnumerable<RoundOfGolf> rounds)
        {
            var scores = new List<BarChartRound>();

            foreach (var round in rounds)
            {
                if (round.Scores.Count == EIGHTTEEN)
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

        private List<ScoreToPar> CalculateScoresToPar(IEnumerable<RoundOfGolf> rounds)
        {
            float eagles = 0;
            float birdies = 0;
            float pars = 0;
            float bogies = 0;
            float doubleBogies = 0;
            float others = 0;
            float totalHoles = 0;

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
                ScoreName = "eagle",
                Percentage = eagles / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName = "birdie",
                Percentage = birdies / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName = "par",
                Percentage = pars / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName = "bogie",
                Percentage = bogies / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName = "double-bogie",
                Percentage = doubleBogies / totalHoles * 100
            });
            scoresToPar.Add(new ScoreToPar
            {
                ScoreName = "other",
                Percentage = others / totalHoles * 100
            });

            return scoresToPar;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }


    }
}
