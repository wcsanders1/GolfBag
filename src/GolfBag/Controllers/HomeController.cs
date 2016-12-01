using GolfBag.ViewModels;
using GolfBag.Services;
using GolfBag.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GolfBag.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IRoundOfGolf _roundOfGolf;
        public HomeController(IRoundOfGolf roundOfGolf)
        {
            _roundOfGolf = roundOfGolf;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var homePageViewModel = new HomePageViewModel();

            if (User.Identity.IsAuthenticated &&
                _roundOfGolf.GetAllRounds(User.Identity.Name).ToList().Count > 0)
            {
                var mostRecentRound = _roundOfGolf.GetAllRounds(User.Identity.Name)
                                        .OrderBy(m => m.Date)
                                        .Last();

                var mostRecentCoursePlayed = _roundOfGolf
                                        .GetCourse(mostRecentRound.CourseId);
                var mostRecentScores = mostRecentRound.Scores.ToList();

                homePageViewModel.CourseLastPlayed = mostRecentCoursePlayed
                                        .CourseName;
                homePageViewModel.DateOfLastRound = mostRecentRound.Date;
                homePageViewModel.NumberOfHolesPlayedInLastRound = mostRecentScores
                                        .Count;
                homePageViewModel.ScoreOfLastRound = mostRecentScores
                                        .Sum(m => m.HoleScore);
                homePageViewModel.DaysSinceLastRound = DateTime
                                        .Now
                                        .Subtract(mostRecentRound.Date)
                                        .Days;

                return View(homePageViewModel);
            }
            else
            {
                homePageViewModel.NumberOfHolesPlayedInLastRound = 0;
                return View(homePageViewModel);
            }
        }
    }
}
