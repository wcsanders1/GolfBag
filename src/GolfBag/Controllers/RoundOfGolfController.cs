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
        public IActionResult EnterCourse(CourseViewModel model)
        {
            model.TeeBoxes.RemoveRange(int.Parse(model.NumberOfTeeBoxes), model.TeeBoxes.Count - int.Parse(model.NumberOfTeeBoxes));

           // if (ModelState.IsValid)
           // {
                var course = new Course();
                var courseHoles = new List<CourseHole>();
                var teeBoxes = new List<TeeBox>();
                

                course.PlayerName = User.Identity.Name;
                course.CourseName = model.CourseName;
                course.NumberOfHoles = model.NumberOfHoles;

                for (int i = 0; i < model.NumberOfHoles; i++)
                {
                    var courseHole = new CourseHole();
                    courseHole.HoleNumber = i + 1;
                    courseHole.Par = model.Pars[i];
                    courseHole.Handicap = model.Handicaps[i];
                    courseHoles.Add(courseHole);
                }

                for (int i = 0; i < int.Parse(model.NumberOfTeeBoxes); i++)
                {
                    var teeBox = new TeeBox();
                    var tees = new List<Tee>();
                    teeBox.Name = model.TeeBoxes[i].Name;

                    for (int x = 0; x < model.NumberOfHoles; x++)
                    {
                        var tee = new Tee();
                        tee.HoleNumber = x + 1;
                        tee.Yardage = model.TeeBoxes[i].Tees[x].Yardage;
                        tees.Add(tee);
                    }

                    teeBox.Tees = tees;
                    teeBoxes.Add(teeBox);
                }

                course.CourseHoles = courseHoles;
                course.TeeBoxes = teeBoxes;
                _roundOfGolf.AddCourse(course);
                var newCourseInList = new List<Course>();
                newCourseInList.Add(course);
                return RedirectToAction("EnterScore");
            //}
            //return View();
        }

        [HttpGet]
        public IActionResult EnterScore()
        {
            IEnumerable<Course> courses = _roundOfGolf.GetAllCourses(User.Identity.Name);

            if (courses.Count() > 0)
            {
                var courseNames = new List<string>();

                foreach (var course in courses)
                {
                    courseNames.Add(course.CourseName);
                }

                return View(courseNames);
            }
            ViewBag.Message = "You have no courses saved. Please enter a course before entering a score.";
            return View("EnterCourse");
        }

        [HttpPost]
        public IActionResult EnterScore(RoundOfGolfViewModel model, string courseName)
        {
            if (ModelState.IsValid)
            {
                var roundOfGolf = new RoundOfGolf();
                var scores = new List<Score>();

                roundOfGolf.PlayerName = User.Identity.Name;

                if (model.FrontNineScores != null)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        var score = new Score();
                        score.HoleScore = model.FrontNineScores[i];
                        score.HoleNumber = i + 1;
                        scores.Add(score);                       
                    }
                }

                if (model.BackNineScores != null)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        var score = new Score();
                        score.HoleScore = model.BackNineScores[i];
                        score.HoleNumber = i + 10;
                        scores.Add(score);
                    }
                }

                roundOfGolf.Comment = model.Comment;
                roundOfGolf.Date = model.DateOfRound;
                roundOfGolf.Scores = scores;
                roundOfGolf.CourseId = _roundOfGolf.GetCourseId(courseName);

                _roundOfGolf.AddRound(roundOfGolf);
                return RedirectToAction("Index", "Home", "");
            }
            return View();
        }

        public IActionResult DisplayCourse(string courseName)
        {
            var course = new Course();
            var roundOfGolfViewModel = new RoundOfGolfViewModel();
            var pars = new List<int>();
            var handicaps = new List<int>();
            var yardages = new List<int>();
            var teeBoxes = new List<TeeBox>();

            course = _roundOfGolf.GetCourse(courseName);
            course.CourseHoles = course.CourseHoles.OrderBy(r => r.HoleNumber).ToList();

            for (int i = 0; i < course.TeeBoxes.Count; i++)
            {
                course.TeeBoxes[i].Tees = course.TeeBoxes[i].Tees.OrderBy(r => r.HoleNumber).ToList();
            }

            foreach (var courseHole in course.CourseHoles)
            {
                pars.Add(courseHole.Par);
                handicaps.Add(courseHole.Handicap);
            }

            roundOfGolfViewModel.CourseName = course.CourseName;
            roundOfGolfViewModel.NumberOfHoles = course.NumberOfHoles;
            roundOfGolfViewModel.Pars = pars;
            roundOfGolfViewModel.Handicaps = handicaps;
            roundOfGolfViewModel.TeeBoxes = course.TeeBoxes;

            return PartialView("_DisplayCourse", roundOfGolfViewModel);
        }

        public IActionResult DisplayBackNine()
        {
            return PartialView("_BackNine");
        }

        public IActionResult DisplayFrontNineEnterScore(RoundOfGolfViewModel model)
        {
            return PartialView("_FrontNineEnterScore", model);
        }

        public IActionResult DisplayBackNineEnterScore(RoundOfGolfViewModel model)
        {
            return PartialView("_BackNineEnterScore", model);
        }

        public IActionResult ViewRounds()
        {
            var roundsOfGolfViewModel = new List<ViewRoundsViewModel>();
            IEnumerable<RoundOfGolf> roundsOfGolf = _roundOfGolf.GetAllRounds(User.Identity.Name)
                .OrderBy(m => m.Date);

            if (roundsOfGolf.Count() > 0)
            {
                foreach (var round in roundsOfGolf)
                {
                    Course course = _roundOfGolf.GetCourse(round.CourseId);

                    var roundOfGolfViewModel = new ViewRoundsViewModel();
                    roundOfGolfViewModel.CourseName = course.CourseName;
                    roundOfGolfViewModel.RoundId = round.Id;
                    roundOfGolfViewModel.RoundDate = round.Date;
                    roundsOfGolfViewModel.Add(roundOfGolfViewModel);
                }

                return View(roundsOfGolfViewModel);
            }
            ViewBag.Message = "You have no rounds saved.";

            return View();
        }

        public IActionResult DisplayRound(int id)
        {
            var roundOfGolfViewModel = new RoundOfGolfViewModel();
            var round = new RoundOfGolf();
            var course = new Course();
            var scores = new List<Score>();
            var holes = new List<CourseHole>();
            var viewFrontNineScores = new List<int>();
            var viewBackNineScores = new List<int>();
            var viewPars = new List<int>();
            var viewYardages = new List<int>();

            round = _roundOfGolf.GetRound(id);
            course = _roundOfGolf.GetCourse(round.CourseId);

            scores = round.Scores.OrderBy(r => r.HoleNumber).ToList();
            holes = course.CourseHoles.OrderBy(r => r.HoleNumber).ToList();

            for (int i = 0; i < scores.Count; i++)
            {
                if (scores[i].HoleNumber < 10)
                {
                    viewFrontNineScores.Add(scores[i].HoleScore);
                    viewPars.Add(holes[i].Par);
                }
                else if (scores[i].HoleNumber >= 10)
                {
                    viewBackNineScores.Add(scores[i].HoleScore);

                    if (scores.Count < 10)
                    {
                        viewPars.Add(holes[i + 9].Par);
                    }
                    else
                    {
                        viewPars.Add(holes[i].Par);
                    }
                }
            }

            roundOfGolfViewModel.TeeBoxes = course.TeeBoxes.OrderBy(r => r.Tees.OrderBy(m => m.HoleNumber)).ToList();
            roundOfGolfViewModel.FrontNineScores = viewFrontNineScores;
            roundOfGolfViewModel.BackNineScores = viewBackNineScores;
            roundOfGolfViewModel.Pars = viewPars;
            roundOfGolfViewModel.CourseName = course.CourseName;
            roundOfGolfViewModel.NumberOfHoles = scores.Count();
            roundOfGolfViewModel.DateOfRound = round.Date;
            roundOfGolfViewModel.Comment = round.Comment;
            roundOfGolfViewModel.Id = id;

            var rounds = _roundOfGolf.GetAllRounds(User.Identity.Name)
                            .OrderBy(m => m.Date)
                            .ToList();

            for (int i = 0; i < rounds.Count; i++)
            {
                if (rounds[i].Date == round.Date)
                {
                    if (i == 0)
                    {
                        roundOfGolfViewModel.IdOfPriorRound = -1;
                    }
                    else
                    {
                        roundOfGolfViewModel.IdOfPriorRound = rounds[i - 1].Id;
                        roundOfGolfViewModel.DateOfPriorRound = rounds[i - 1].Date;
                    }

                    if ((i + 1) == rounds.Count)
                    {
                        roundOfGolfViewModel.IdOfSubsequentRound = -1;
                    }
                    else
                    {
                        roundOfGolfViewModel.IdOfSubsequentRound = rounds[i + 1].Id;
                        roundOfGolfViewModel.DateOfSubsequentRound = rounds[i + 1].Date;
                    }
                }
            } 

            return PartialView("_DisplayRound", roundOfGolfViewModel);
        }

        public IActionResult EditCourses()
        {
            IEnumerable<Course> courses = _roundOfGolf.GetAllCourses(User.Identity.Name);

            if (courses.Count() > 0)
            {
                var courseNames = new List<string>();

                foreach (var course in courses)
                {
                    courseNames.Add(course.CourseName);
                }

                return View(courseNames);
            }
            ViewBag.Message = "You have no courses saved. Please enter a course before entering a score.";
            return View("EnterCourse");
        }

        [HttpGet]
        public IActionResult EditCourse(string courseName)
        {
            var course = _roundOfGolf.GetCourse(courseName);
            var courseViewModel = new CourseViewModel();
            var pars = new List<int>();
            var handicaps = new List<int>();

            course.CourseHoles = course.CourseHoles.OrderBy(r => r.HoleNumber).ToList();

            courseViewModel.CourseName = course.CourseName;
            courseViewModel.NumberOfHoles = course.NumberOfHoles;
            courseViewModel.Id = course.Id;

            foreach (var courseHole in course.CourseHoles)
            {
                pars.Add(courseHole.Par);
                handicaps.Add(courseHole.Handicap);
            }
            courseViewModel.Pars = pars;
            courseViewModel.TeeBoxes = course.TeeBoxes.OrderBy(r => r.Tees.OrderBy(m => m.HoleNumber)).ToList();

            return PartialView("_DisplayCourseToEdit", courseViewModel);
        }

        public IActionResult SaveCourseChanges(CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                var courseToSave = _roundOfGolf.GetCourse(course.CourseName);

                courseToSave.CourseHoles = courseToSave.CourseHoles.OrderBy(r => r.HoleNumber).ToList();

                int i = 0;
                foreach (var courseHole in courseToSave.CourseHoles)
                {
                    courseHole.Par = course.Pars[i];
                    //courseHole.Yardage = course.Yardages[i];
                    i++;
                }
                _roundOfGolf.SaveCourseEdits(courseToSave);
            }
            return RedirectToAction("EditCourses");
        }

        public IActionResult EditRound(int id)
        {
            var roundOfGolfViewModel = new RoundOfGolfViewModel();
            var round = new RoundOfGolf();
            var course = new Course();
            var scores = new List<Score>();
            var holes = new List<CourseHole>();
            var viewFrontNineScores = new List<int>();
            var viewBackNineScores = new List<int>();
            var viewPars = new List<int>();
            var viewYardages = new List<int>();

            round = _roundOfGolf.GetRound(id);
            course = _roundOfGolf.GetCourse(round.CourseId);

            scores = round.Scores.OrderBy(r => r.HoleNumber).ToList();
            holes = course.CourseHoles.OrderBy(r => r.HoleNumber).ToList();

            for (int i = 0; i < scores.Count; i++)
            {
                if (scores[i].HoleNumber < 10)
                {
                    viewFrontNineScores.Add(scores[i].HoleScore);
                    viewPars.Add(holes[i].Par);
                }
                else if (scores[i].HoleNumber >= 10)
                {
                    viewBackNineScores.Add(scores[i].HoleScore);

                    if (scores.Count < 10)
                    {
                        viewPars.Add(holes[i + 9].Par);
                    }
                    else
                    {
                        viewPars.Add(holes[i].Par);
                    }
                }
            }

            roundOfGolfViewModel.FrontNineScores = viewFrontNineScores;
            roundOfGolfViewModel.BackNineScores = viewBackNineScores;
            roundOfGolfViewModel.Pars = viewPars;
            roundOfGolfViewModel.TeeBoxes = course.TeeBoxes.OrderBy(r => r.Tees.OrderBy(m => m.HoleNumber)).ToList();
            roundOfGolfViewModel.CourseName = course.CourseName;
            roundOfGolfViewModel.NumberOfHoles = scores.Count();
            roundOfGolfViewModel.DateOfRound = round.Date;
            roundOfGolfViewModel.Comment = round.Comment;
            roundOfGolfViewModel.Id = id;

            return View("DisplayRoundToEdit", roundOfGolfViewModel);
        }

        public IActionResult SaveRoundChanges(RoundOfGolfViewModel round)
        {
            if (ModelState.IsValid)
            {
                var roundToSave = _roundOfGolf.GetRound(round.Id);

                roundToSave.Scores = roundToSave.Scores.OrderBy(r => r.HoleNumber).ToList();
                roundToSave.Comment = round.Comment;
                roundToSave.Date = round.DateOfRound;

                int i = 0;
                foreach (var score in roundToSave.Scores)
                {
                    if (score.HoleNumber < 10)
                    {
                        score.HoleScore = round.FrontNineScores[i];
                        i++;
                    }
                }

                i = 0;
                foreach (var score in roundToSave.Scores)
                {
                    if (score.HoleNumber >= 10)
                    {
                        score.HoleScore = round.BackNineScores[i];
                        i++;
                    }
                }
                _roundOfGolf.SaveRoundEdits(roundToSave);
            }
            return RedirectToAction("ViewRounds");
        }

        public IActionResult DeleteRound(int id)
        {
            var round = _roundOfGolf.GetRound(id);
            _roundOfGolf.DeleteRound(round);
            return RedirectToAction("ViewRounds");
        }

        public IActionResult DeleteCourse(int id)
        {
            var course = _roundOfGolf.GetCourse(id);

            if (!_roundOfGolf.DeleteCourse(course))
            {
                IEnumerable<Course> courses = _roundOfGolf.GetAllCourses(User.Identity.Name);

                var courseNames = new List<string>();

                foreach (var courseName in courses)
                {
                    courseNames.Add(courseName.CourseName);
                }
                ViewBag.Message = "You must delete all rounds associated with this course before you can delete it.";
                return View("EditCourses", courseNames);
            }
            return RedirectToAction("EditCourses");
        }
    }
}
