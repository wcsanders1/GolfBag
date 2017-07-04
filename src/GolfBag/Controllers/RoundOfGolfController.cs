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
            Course course   = model.MapViewModelToCourse(User.Identity.Name);
            course.PlayerId = GetCurrentUserAsync().Result.Id;

            if (course == null)
            {
                ViewBag.Message = "Failed to save course";
                return View();
            }

            try
            {
                _roundOfGolf.AddCourse(course);
            }
            catch
            {
                ViewBag.Message = "Error saving course.";
                return View();
            }
            
            var newCourseInList = new List<Course>();
            newCourseInList.Add(course);
            return RedirectToAction("EnterScore");
        }

        [HttpGet]
        public IActionResult EnterScore()
        {
            IEnumerable<Course> courses = null;
            try
            {
                courses = _roundOfGolf.GetAllCourses(GetCurrentUserAsync().Result.Id);
            }
            catch
            {
                ViewBag.Message = "There was an error retrieving your courses.";
                return View("EnterCourse");
            }

            if (courses == null || courses.Count() < 1)
            {
                ViewBag.Message = "You have no courses saved. Please enter a course before entering a score.";
                return View("EnterCourse");
            }

            var courseDictionary = new Dictionary<int, string>();

            foreach (var course in courses)
            {
                courseDictionary.Add(course.Id, course.CourseName);
            }

            return View(courseDictionary);            
        }

        [HttpPost]
        public IActionResult EnterScore(RoundOfGolfViewModel model, string courseName)
        {
            if (!model.IsValid())
            {
                ViewBag.Message = "Data Invalid";
                return View("Error");
            }

            int courseId = 0;
            try
            {
                courseId = _roundOfGolf.GetCourseId(courseName);
            }
            catch
            {
                ViewBag.Message = "Error retrieving course";
                return View("Error");
            }

            RoundOfGolf roundOfGolf = model.MapViewModelToRoundOfGolf(courseName, courseId, User.Identity.Name);
            roundOfGolf.PlayerId = GetCurrentUserAsync().Result.Id;

            if (roundOfGolf == null)
            {
                ViewBag.Message = "There was an error";
                return View("Error");
            }

            try
            {
                _roundOfGolf.AddRound(roundOfGolf);
            }
            catch
            {
                ViewBag.Message = "Error saving round";
                return View("Error");
            }
            
            return RedirectToAction("Index", "Home", "");

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
            RoundOfGolf round;
            try
            {
                round = _roundOfGolf.GetRound(id);
                if (round.PlayerId != GetCurrentUserAsync().Result.Id)
                {
                    ViewBag.Message = "An Error Occurred";
                    return PartialView("~/Views/Shared/Error.cshtml");
                }

                RoundOfGolfViewModel roundOfGolfViewModel = GetRoundOfGolfViewModel(round);
                return PartialView("_DisplayRound", roundOfGolfViewModel);
            }
            catch
            {
                ViewBag.Message = "An Error Occurred";
                return PartialView("~/Views/Shared/Error.cshtml");
            }
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
                courseToSave.TeeBoxes[i].CourseRating = Convert.ToDecimal(model.TeeBoxes[i].CourseRating);
                courseToSave.TeeBoxes[i].SlopeRating = model.TeeBoxes[i].SlopeRating;
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
                    teebox.CourseRating = Convert.ToDecimal(newTeebox.CourseRating);
                    teebox.SlopeRating = newTeebox.SlopeRating;

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
            RoundOfGolf round;
            try
            {
                round = _roundOfGolf.GetRound(id);
                if (round.PlayerId != GetCurrentUserAsync().Result.Id)
                {
                    ViewBag.Message = "An Error Occurred";
                    return PartialView("~/Views/Shared/Error.cshtml");
                }
                RoundOfGolfViewModel roundOfGolfViewModel = GetRoundOfGolfViewModel(round);
                return View("DisplayRoundToEdit", roundOfGolfViewModel);
            }
            catch
            {
                ViewBag.Message = "An Error Occurred";
                return PartialView("~/Views/Shared/Error.cshtml");
            }
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
                    score.HolePutt  = round.FrontNinePutts[i];
                    i++;
                }
            }

            i = 0;
            foreach (var score in roundToSave.Scores)
            {
                if (score.HoleNumber >= 10)
                {
                    score.HoleScore = round.BackNineScores[i];
                    score.HolePutt  = round.BackNinePutts[i];
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
        
        private RoundOfGolfViewModel GetRoundOfGolfViewModel(RoundOfGolf roundOfGolf)
        {
            List<RoundOfGolf> roundsOfGolf              = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id).ToList();
            Course course                               = _roundOfGolf.GetCourse(roundOfGolf.CourseId);
            RoundOfGolfViewModel roundOfGolfViewModel   = RoundOfGolfViewModel.MapRoundOfGolfToRoundOfGolfViewModel(
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
