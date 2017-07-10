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
        public IActionResult Index()
        {
            var homePageViewModel = new HomePageViewModel();
            User currentUser;

            if (User.Identity.IsAuthenticated)
            {
                currentUser = GetCurrentUserAsync().Result;

                if (currentUser == null)
                {
                    // TODO1 account for when a cookie exists for a deleted user
                }

                homePageViewModel.FirstName = currentUser.FirstName;
                homePageViewModel.LastName = currentUser.LastName;

                IEnumerable<RoundOfGolf> rounds;
                try
                {
                    rounds = _roundOfGolf.GetAllRounds(currentUser.Id);
                }
                catch
                {
                    ViewBag.Message = "Sorry, there is an error retrieving your information.";
                    return View("Error");
                }

                if (rounds.ToList().Count > 0)
                {
                    var mostRecentRound = _roundOfGolf.GetAllRounds(currentUser.Id)
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
                    
                    homePageViewModel.Handicap = GetHandicap(rounds);
                }
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

        private string GetHandicap(IEnumerable<RoundOfGolf> rounds)
        {
            const decimal diffCalc = .96m;

            rounds = rounds.Skip(rounds.Count() - 20).ToList();
            if (rounds.Count() < 5)
            {
                return null;
            }

            var courseIds = rounds.Select(t => t.CourseId).ToList().Distinct();

            var courses = new List<Course>();
            foreach (var id in courseIds)
            {
                Course course;
                try
                {
                    course = _roundOfGolf.GetCourse(id);
                }
                catch
                {
                    return null;
                }

                courses.Add(course);
            }

            var differentials = new List<decimal>();
            foreach (var round in rounds)
            {
                var course = courses.Where(t => t.Id == round.CourseId).FirstOrDefault();
                var teebox = course.TeeBoxes.Where(t => t.Id == round.TeeBoxPlayed).FirstOrDefault();
                decimal slopeRating = 0;
                decimal courseRating = 0;
                if (round.Scores.Count == 9 && teebox.Tees.Count > 9)
                {
                    slopeRating = teebox.SlopeRating / 2;
                    courseRating = teebox.CourseRating / 2;
                }
                else
                {
                    slopeRating = teebox.SlopeRating;
                    courseRating = teebox.CourseRating;
                }

                if (slopeRating == 0 || courseRating == 0)
                {
                    return null;
                }

                var differential = (round.Scores.Sum(t => t.HoleScore) - courseRating) * (113 / slopeRating);
                differentials.Add(differential);
            }

            differentials.Sort();

            decimal handicap;
            if (rounds.Count() < 11)
            {
                handicap = differentials[0] * diffCalc;
            }
            else if (rounds.Count() < 20)
            {
                handicap = differentials.Take(5).Average() * diffCalc;
            }
            else
            {
                handicap = differentials.Take(10).Average() * diffCalc;
            }

            if (handicap < 0)
            {
                handicap = Math.Abs(handicap);
            }

            return handicap.ToString("N1");
        }
    }
}
