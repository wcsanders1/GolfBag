using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GolfBag.ViewModels;
using GolfBag.Entities;
using GolfBag.Services;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GolfBag.Controllers
{
    public class RoundOfGolfController : Controller
    {
        private IRoundOfGolf _roundOfGolf;
        private UserManager<User> _userManager;

        public RoundOfGolfController(IRoundOfGolf roundOfGolf, UserManager<User> userManager)
        {
            _roundOfGolf = roundOfGolf;             
            _userManager = userManager;    
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
            course.PlayerId = GetCurrentUserAsync().Result.Id;

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
            IEnumerable<Course> courses = _roundOfGolf.GetAllCourses(GetCurrentUserAsync().Result.Id);

            if (courses.Count() > 0)
            {
                var courseDictionary = new Dictionary<int, string>();

                foreach (var course in courses)
                {
                    courseDictionary.Add(course.Id, course.CourseName);
                }

                return View(courseDictionary);
            }
            ViewBag.Message = "You have no courses saved. Please enter a course before entering a score.";
            return View("EnterCourse");
        }

        [HttpPost]
        public IActionResult EnterScore(RoundOfGolfViewModel model, string courseName)
        {
            int courseId = _roundOfGolf.GetCourseId(courseName);
            
            RoundOfGolf roundOfGolf = model.MapViewModelToRoundOfGolf(courseName, courseId, User.Identity.Name);
            roundOfGolf.PlayerId = GetCurrentUserAsync().Result.Id;

            if (roundOfGolf != null)
            { 
                _roundOfGolf.AddRound(roundOfGolf);
                return RedirectToAction("Index", "Home", "");
            }
            return View();
        }

        public IActionResult DisplayCourse(int courseId)
        {
            Course course = _roundOfGolf.GetCourse(courseId);
            RoundOfGolfViewModel roundOfGolfViewModel = RoundOfGolfViewModel.MapCourseToRoundOfGolfViewModel(course);

            return PartialView("_DisplayCourse", roundOfGolfViewModel);
        }

        public IActionResult DisplayBackNine()
        {
            return PartialView("_BackNine");
        }

        public IActionResult DisplayFrontNineEnterScore(RoundOfGolfViewModel model)
        {
            Course course = _roundOfGolf.GetCourse(model.IdOfCourse);
            RoundOfGolfViewModel roundOfGolfViewModel = RoundOfGolfViewModel.MapCourseToRoundOfGolfViewModel(course);

            return PartialView("_FrontNineEnterScore", roundOfGolfViewModel);
        }

        public IActionResult DisplayBackNineEnterScore(RoundOfGolfViewModel model)
        {
            Course course = _roundOfGolf.GetCourse(model.IdOfCourse);
            RoundOfGolfViewModel roundOfGolfViewModel = RoundOfGolfViewModel.MapCourseToRoundOfGolfViewModel(course);

            return PartialView("_BackNineEnterScore", roundOfGolfViewModel);
        }

        public IActionResult ViewRounds(int selectedRound = -1)
        {
            var r = _userManager.GetUserId(User);
            
            var roundsOfGolfViewModel = new ViewRoundsViewModel();
            roundsOfGolfViewModel.SelectedRound = selectedRound;

            IEnumerable<RoundOfGolf> roundsOfGolf = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id);

            if (roundsOfGolf.Count() > 0)
            {
                foreach (var round in roundsOfGolf)
                {
                    Course course = _roundOfGolf.GetCourse(round.CourseId);
                    roundsOfGolfViewModel.MapViewRounds(round, course);
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
            IEnumerable<Course> courses = _roundOfGolf.GetAllCourses(GetCurrentUserAsync().Result.Id);

            if (courses.Count() > 0)
            {
                var courseDictionary = new Dictionary<int, string>();

                foreach (var course in courses)
                {
                    courseDictionary.Add(course.Id, course.CourseName);
                }

                return View(courseDictionary);
            }
            ViewBag.Message = "You have no courses saved. Please enter a course before entering a score.";
            return View("EnterCourse");
        }

        [HttpGet]
        public IActionResult EditCourse(int courseId)
        {
            var course = _roundOfGolf.GetCourse(courseId);
            CourseViewModel courseViewModel = CourseViewModel.MapCourseToCourseViewModel(course);
            courseViewModel.ProduceListOfNewTeeBoxes(course);
            courseViewModel.ProduceListOfDeletedTeeBoxes(course);

            return PartialView("_DisplayCourseToEdit", courseViewModel);
        }

        [HttpPost]
        public IActionResult SaveCourseChanges(CourseViewModel model)
        {
            var courseToSave = _roundOfGolf.GetCourse(model.Id);

            courseToSave.CourseName = model.CourseName;
            
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

        //[HttpPost]
        public IActionResult DeleteCourse(int courseId)
        {
            var course = _roundOfGolf.GetCourse(courseId);
            _roundOfGolf.DeleteCourse(course);
            return RedirectToAction("EditCourses");
        }

        public string DatesPlayedTeebox(int id)
        {
            var rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id);
            string datesPlayedTeebox = "";

            foreach (var round in rounds)
            {
                if (round.TeeBoxPlayed == id)
                    datesPlayedTeebox += round.Date.ToString("MMMM d, yyyy") + ":";
            }

            return datesPlayedTeebox;
        }

        public string DatesPlayedCourse(int id)
        {
            var rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id);
            string datesPlayedCourse = "";

            foreach (var round in rounds)
            {
                if (round.CourseId == id)
                    datesPlayedCourse += round.Date.ToString("MMMM d, yyyy") + ":";
            }

            return datesPlayedCourse;
        }


/**********************************************************************************************************************************
                                            PRIVATE METHODS
**********************************************************************************************************************************/
        
        private RoundOfGolfViewModel GetRoundOfGolfViewModel(int id)
        {
            RoundOfGolf roundOfGolf = _roundOfGolf.GetRound(id);
            List<RoundOfGolf> roundsOfGolf = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id).ToList();
            Course course = _roundOfGolf.GetCourse(roundOfGolf.CourseId);
            RoundOfGolfViewModel roundOfGolfViewModel = RoundOfGolfViewModel.MapRoundOfGolfToRoundOfGolfViewModel(
                                                            roundOfGolf,
                                                            roundsOfGolf,
                                                            course,
                                                            User.Identity.Name);
            return roundOfGolfViewModel;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
