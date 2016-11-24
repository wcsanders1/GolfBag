using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GolfBag.ViewModels;
using GolfBag.Entities;
using GolfBag.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GolfBag.Controllers
{
    public class RoundOfGolfController : Controller
    {
        private IRoundOfGolf _roundOfGolf;

        public RoundOfGolfController(IRoundOfGolf roundOfGolf)
        {
            _roundOfGolf = roundOfGolf;
        }

        [HttpGet]
        public IActionResult EnterCourse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnterCourse(EnterCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Course course = new Course();
                List<CourseHole> courseHoles = new List<CourseHole>();

                course.PlayerName = User.Identity.Name;
                course.CourseName = model.CourseName;
                course.NumberOfHoles = model.NumberOfHoles;

                for (int i = 0; i < model.NumberOfHoles; i++)
                {
                    CourseHole courseHole = new CourseHole();
                    courseHole.HoleNumber = i + 1;
                    courseHole.Par = model.Par[i];
                    courseHole.Yardage = model.Yardage[i];
                    courseHoles.Add(courseHole);
                }

                course.CourseHoles = courseHoles;
                _roundOfGolf.AddCourse(course);
                return View("EnterScore");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EnterScore()
        {
            IEnumerable<Course> courses = _roundOfGolf.GetAllCourses(User.Identity.Name);

            if (courses.Count() > 0)
            {
                return View(courses);
            }
            ViewBag.Message = "You have no courses saved. Please enter a course before entering a score.";
            return View("EnterCourse");
        }

        [HttpPost]
        public IActionResult EnterScore(RoundOfGolfViewModel model)
        {
            if (ModelState.IsValid)
            {
                RoundOfGolf roundOfGolf = new RoundOfGolf();
                List<Score> scores = new List<Score>();

                roundOfGolf.PlayerName = User.Identity.Name;

                for (int i = 0; i < model.Scores.Count; i++)
                {
                    Score score = new Score();
                    score.HoleScore = model.Scores[i];
                    scores.Add(score);
                }

                roundOfGolf.Scores = scores;

                _roundOfGolf.AddRound(roundOfGolf);
                return RedirectToAction("Index", "Home", "");
            }
            return View();
        }
    }
}
