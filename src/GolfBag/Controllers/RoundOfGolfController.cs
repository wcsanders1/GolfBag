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
            Course course = model.MapViewModelToCourse(User.Identity.Name);

            if (course != null)
            {
                _roundOfGolf.AddCourse(course);
                var newCourseInList = new List<Course>();
                newCourseInList.Add(course);
                return RedirectToAction("EnterScore");
            }
            ViewBag.Message = "Failed to save course";
            return View();
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
            int courseId = _roundOfGolf.GetCourseId(courseName);

            RoundOfGolf roundOfGolf = model.MapViewModelToRoundOfGolf(courseName, courseId, User.Identity.Name);

            if (roundOfGolf != null)
            { 
                _roundOfGolf.AddRound(roundOfGolf);
                return RedirectToAction("Index", "Home", "");
            }
            return View();
        }

        public IActionResult DisplayCourse(string courseName)
        {
            Course course = _roundOfGolf.GetCourse(courseName);
            RoundOfGolfViewModel roundOfGolfViewModel = RoundOfGolfViewModel.MapCourseToRoundOfGolfViewModel(course);

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

        public IActionResult ViewRounds(int selectedRound = -1)
        {
            var roundsOfGolfViewModel = new ViewRoundsViewModel();
            roundsOfGolfViewModel.SelectedRound = selectedRound;

            IEnumerable<RoundOfGolf> roundsOfGolf = _roundOfGolf.GetAllRounds(User.Identity.Name);

            if (roundsOfGolf.Count() > 0)
            {
                foreach (var round in roundsOfGolf)
                {
                    Course course = _roundOfGolf.GetCourse(round.CourseId);

                    var viewRound = new ViewRound();
                    viewRound.CourseName = course.CourseName;
                    viewRound.RoundId = round.Id;
                    viewRound.RoundDate = round.Date;
                    roundsOfGolfViewModel.ViewRounds.Add(viewRound);
                }

                return View(roundsOfGolfViewModel);
            }
            ViewBag.Message = "You have no rounds saved.";

            return View();
        }

        public IActionResult DisplayRound(int id)
        {
            RoundOfGolfViewModel roundOfGolfViewModel = GetRoundOfGolfViewModel(id);
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
            CourseViewModel courseViewModel = MapCourseToCourseViewModel(course);
            courseViewModel.ProduceListOfNewTeeBoxes(course);
            courseViewModel.ProduceListOfDeletedTeeBoxes(course);

            return PartialView("_DisplayCourseToEdit", courseViewModel);
        }

        [HttpPost]
        public IActionResult SaveCourseChanges(CourseViewModel model)
        {
            var courseToSave = _roundOfGolf.GetCourse(model.CourseName);
            
            for (int i = 0; i < courseToSave.CourseHoles.Count; i++)
            {
                courseToSave.CourseHoles[i].Par = model.Pars[i];
                courseToSave.CourseHoles[i].Handicap = model.Handicaps[i];
            }

            for (int i = 0; i < courseToSave.TeeBoxes.Count; i++)
            {
                courseToSave.TeeBoxes[i].Name = model.TeeBoxes[i].Name;
                for (int x = 0; x < courseToSave.TeeBoxes[i].Tees.Count; x++)
                {
                    courseToSave.TeeBoxes[i].Tees[x].Yardage = model.TeeBoxes[i].Tees[x].Yardage;
                }
            }

            foreach (var newTeebox in model.NewTeeBoxes)
            {
                if (newTeebox.Name != null)
                {
                    var teebox = new TeeBox();
                    var tees = new List<Tee>();
                    teebox.Name = newTeebox.Name;

                    for (int i = 0; i < newTeebox.Tees.Count; i++)
                    {
                        var tee = new Tee();
                        tee.HoleNumber = i + 1;
                        tee.Yardage = newTeebox.Tees[i].Yardage;
                        tees.Add(tee);
                    }
                    teebox.Tees = tees;
                    courseToSave.TeeBoxes.Add(teebox);
                }
            }

            _roundOfGolf.SaveCourseEdits(courseToSave, model.ListOfDeletedTeeBoxes);
            return RedirectToAction("EditCourses");

        }

        public IActionResult EditRound(int id)
        {
            RoundOfGolfViewModel roundOfGolfViewModel = GetRoundOfGolfViewModel(id);
            return View("DisplayRoundToEdit", roundOfGolfViewModel);
        }

        public IActionResult SaveRoundChanges(RoundOfGolfViewModel round)
        {
            var roundToSave = _roundOfGolf.GetRound(round.Id);

            roundToSave.Comment = round.Comment;
            roundToSave.Date = round.DateOfRound;
            roundToSave.TeeBoxPlayed = round.IdOfTeeBoxPlayed;

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
            
            return RedirectToAction("ViewRounds", new { selectedRound = roundToSave.Id });
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

        public string DatesPlayedTeebox(int id)
        {
            var rounds = _roundOfGolf.GetAllRounds(User.Identity.Name);
            string datesPlayedTeebox = "";

            foreach (var round in rounds)
            {
                if (round.TeeBoxPlayed == id)
                    datesPlayedTeebox += round.Date.ToString("MMMM d, yyyy") + ":";
            }

            return datesPlayedTeebox;
        }


        //****************  PRIVATE METHODS  ***********************************************************
        


        private RoundOfGolfViewModel GetRoundOfGolfViewModel(int id)
        {
            RoundOfGolf roundOfGolf = _roundOfGolf.GetRound(id);
            List<RoundOfGolf> roundsOfGolf = _roundOfGolf.GetAllRounds(User.Identity.Name).ToList();
            Course course = _roundOfGolf.GetCourse(roundOfGolf.CourseId);
            RoundOfGolfViewModel roundOfGolfViewModel = RoundOfGolfViewModel.MapRoundOfGolfToRoundOfGolfViewModel(
                                                            roundOfGolf,
                                                            roundsOfGolf,
                                                            course,
                                                            User.Identity.Name);
            return roundOfGolfViewModel;
        }
        
        
        private CourseViewModel MapCourseToCourseViewModel(Course course)
        {
            var courseViewModel = new CourseViewModel();
            var pars = new List<int>();
            var handicaps = new List<int>();

            courseViewModel.CourseName = course.CourseName;
            courseViewModel.NumberOfHoles = course.NumberOfHoles;
            courseViewModel.NumberOfTeeBoxes = course.TeeBoxes.Count.ToString();
            courseViewModel.Id = course.Id;

            foreach (var courseHole in course.CourseHoles)
            {
                pars.Add(courseHole.Par);
                handicaps.Add(courseHole.Handicap);
            }
            courseViewModel.Pars = pars;
            courseViewModel.Handicaps = handicaps;

            for (int i = 0; i < course.TeeBoxes.Count; i++)
            {
                var viewTeebox = new ViewTeeBox();

                viewTeebox.Id = course.TeeBoxes[i].Id;
                viewTeebox.Name = course.TeeBoxes[i].Name;

                for (int x = 0; x < course.TeeBoxes[i].Tees.Count; x++)
                {
                    var viewTee = new ViewTee();
                    viewTee.Yardage = course.TeeBoxes[i].Tees[x].Yardage;
                    viewTeebox.Tees.Add(viewTee);
                }
                courseViewModel.TeeBoxes.Add(viewTeebox);
            }

            return courseViewModel;
        }   
    }
}
