using GolfBag.ViewModels;
using GolfBag.Services;
using GolfBag.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GolfBag.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IRoundOfGolf _roundOfGolf;
        private UserManager<User> _userManager;

        public HomeController(IRoundOfGolf roundOfGolf, UserManager<User> userManager)
        {
            _roundOfGolf = roundOfGolf;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var homePageViewModel = new HomePageViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var currentUser = GetCurrentUserAsync().Result;
                homePageViewModel.FirstName = currentUser.FirstName;
                homePageViewModel.LastName = currentUser.LastName;
            }

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


/**********************************************************************************************************************************
                                            PRIVATE METHODS
**********************************************************************************************************************************/

        private async Task<User> GetCurrentUserAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return currentUser;
        }
    }
}
