using GolfBag.Entities;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class RoundOfGolfViewModel
    {
        public List<int> FrontNineScores { get; set; }

        public List<int> BackNineScores { get; set; }

        public List<string> FrontNineScoreNames { get; set; }

        public List<string> BackNineScoreNames { get; set; }

        public string CourseName { get; set; }

        public int IdOfCourse { get; set; }

        public int IdOfTeeBoxPlayed { get; set; }

        public string Comment { get; set; }

        public int NumberOfHoles { get; set; }

        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{MMMM d, yyyy}")]
        public DateTime DateOfRound { get; set; }

        public DateTime DateOfPriorRound { get; set; }

        public DateTime DateOfSubsequentRound { get; set; }

        public string CurrentDate { get; set; }

        public int IdOfPriorRound { get; set; }

        public int IdOfSubsequentRound { get; set; }

        public List<TeeBox> TeeBoxes { get; set; }

        public List<int> Pars { get; set; }

        public List<int> Handicaps { get; set; }

        public RoundOfGolfViewModel()
        {
            CurrentDate = DateTime.Today.Date.ToString("D");
        }

        public static RoundOfGolfViewModel MapCourseToRoundOfGolfViewModel(Course course)
        {
            var roundOfGolfViewModel = new RoundOfGolfViewModel();

            roundOfGolfViewModel.Pars = MapPars(course);
            roundOfGolfViewModel.Handicaps = MapHandicaps(course);
            roundOfGolfViewModel.CourseName = course.CourseName;
            roundOfGolfViewModel.IdOfCourse = course.Id;
            roundOfGolfViewModel.NumberOfHoles = course.NumberOfHoles;
            roundOfGolfViewModel.TeeBoxes = course.TeeBoxes;

            return roundOfGolfViewModel;
        }

        public static RoundOfGolfViewModel MapRoundOfGolfToRoundOfGolfViewModel(
            RoundOfGolf roundOfGolf, 
            List<RoundOfGolf> roundsOfGolf, 
            Course course, 
            string playerName)
        {         
            var roundOfGolfViewModel       = new RoundOfGolfViewModel();
            var frontAndBackNineScores     = GetFrontAndBackNineScores(roundOfGolf);
            var frontAndBackNineScoreNames = GetFrontAndBackNineScoreNames(roundOfGolf.Scores, course.CourseHoles);
            var priorAndSubsequentRounds   = GetPriorAndSubsequentRounds(roundsOfGolf, roundOfGolf);
            var priorRound                 = priorAndSubsequentRounds["priorRound"];
            var subsequentRound            = priorAndSubsequentRounds["subsequentRound"];
            var priorRoundType             = priorRound.GetType();
            var subsequentRoundType        = subsequentRound.GetType();

            roundOfGolfViewModel.Pars = MapPars(roundOfGolf, course.CourseHoles);
            roundOfGolfViewModel.Handicaps = MapHandicaps(course);
            roundOfGolfViewModel.TeeBoxes = course.TeeBoxes;
            roundOfGolfViewModel.FrontNineScores = frontAndBackNineScores["frontNineScores"];
            roundOfGolfViewModel.BackNineScores = frontAndBackNineScores["backNineScores"];
            roundOfGolfViewModel.FrontNineScoreNames = frontAndBackNineScoreNames["frontNineScoreNames"];
            roundOfGolfViewModel.BackNineScoreNames = frontAndBackNineScoreNames["backNineScoreNames"];
            roundOfGolfViewModel.IdOfPriorRound = (int)priorRoundType.GetProperty("id").GetValue(priorRound, null);
            roundOfGolfViewModel.IdOfSubsequentRound = (int)subsequentRoundType.GetProperty("id").GetValue(subsequentRound, null);
            roundOfGolfViewModel.DateOfPriorRound = (DateTime)priorRoundType.GetProperty("date").GetValue(priorRound, null);
            roundOfGolfViewModel.DateOfSubsequentRound = (DateTime)subsequentRoundType.GetProperty("date").GetValue(subsequentRound, null);
            roundOfGolfViewModel.CourseName = course.CourseName;
            roundOfGolfViewModel.IdOfCourse = course.Id;
            roundOfGolfViewModel.NumberOfHoles = roundOfGolf.Scores.Count();
            roundOfGolfViewModel.DateOfRound = roundOfGolf.Date;
            roundOfGolfViewModel.Comment = roundOfGolf.Comment;
            roundOfGolfViewModel.Id = roundOfGolf.Id;
            roundOfGolfViewModel.IdOfTeeBoxPlayed = roundOfGolf.TeeBoxPlayed;

            return roundOfGolfViewModel;
        }

        public RoundOfGolf MapViewModelToRoundOfGolf(string courseName, int courseId, string playerName)
        {
            var roundOfGolf = new RoundOfGolf();

            roundOfGolf.PlayerName = playerName;
            roundOfGolf.Comment = Comment;
            roundOfGolf.Date = DateOfRound;
            roundOfGolf.CourseId = courseId;
            roundOfGolf.TeeBoxPlayed = IdOfTeeBoxPlayed;
            roundOfGolf.Scores = MapScores();

            return roundOfGolf;
        }

        public bool IsValid()
        {
            if (DateOfRound == null)
            {
                return false;
            }

            if (FrontNineScores == null && BackNineScores == null)
            {
                return false;
            }

            if (FrontNineScores != null)
            {
                foreach (var score in FrontNineScores)
                {
                    if (score < 1 || score > 99)
                    {
                        return false;
                    }
                }
            }

            if (BackNineScoreNames != null)
            {
                foreach (var score in BackNineScores)
                {
                    if (score < 1 || score > 99)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<Score> MapScores()
        {
            var scores = new List<Score>();

            if (FrontNineScores != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    var score = new Score();
                    score.HoleScore = FrontNineScores[i];
                    score.HoleNumber = i + 1;
                    scores.Add(score);
                }
            }

            if (BackNineScores != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    var score = new Score();
                    score.HoleScore = BackNineScores[i];
                    score.HoleNumber = i + 10;
                    scores.Add(score);
                }
            }
            return scores;
        }

        private static List<int> MapPars(Course course)
        {
            var pars = new List<int>();
            foreach (var courseHole in course.CourseHoles)
            {
                pars.Add(courseHole.Par);
            }
            return pars;
        }

        private static List<int> MapHandicaps(Course course)
        {
            var handicaps = new List<int>();
            foreach (var courseHole in course.CourseHoles)
            {
                handicaps.Add(courseHole.Handicap);
            }
            return handicaps;
        }

        private static List<int> MapPars(RoundOfGolf roundOfGolf, List<CourseHole> courseHoles)
        {
            var pars = new List<int>();
            for (int i = 0; i < roundOfGolf.Scores.Count; i++)
            {
                if (roundOfGolf.Scores[i].HoleNumber < 10)
                {                   
                    pars.Add(courseHoles[i].Par);
                }
                else if (roundOfGolf.Scores[i].HoleNumber >= 10)
                {
                    if (roundOfGolf.Scores.Count < 10)
                    {
                        pars.Add(courseHoles[i + 9].Par);
                    }
                    else
                    {
                        pars.Add(courseHoles[i].Par);
                    }
                }
            }
            return pars;
        }

        private static Dictionary<string, List<int>> GetFrontAndBackNineScores(RoundOfGolf roundOfGolf)
        {
            var frontAndBackNineScores = new Dictionary<string, List<int>>();
            var frontNineScores = new List<int>();
            var backNineScores = new List<int>();
            for (int i = 0; i < roundOfGolf.Scores.Count; i++)
            {
                if (roundOfGolf.Scores[i].HoleNumber < 10)
                {
                    frontNineScores.Add(roundOfGolf.Scores[i].HoleScore);
                }
                else if (roundOfGolf.Scores[i].HoleNumber >= 10)
                {
                    backNineScores.Add(roundOfGolf.Scores[i].HoleScore);
                }
            }
            frontAndBackNineScores.Add("frontNineScores", frontNineScores);
            frontAndBackNineScores.Add("backNineScores", backNineScores);
            return frontAndBackNineScores;
        }

        private static Dictionary<string, List<string>> GetFrontAndBackNineScoreNames(List<Score> scores, List<CourseHole> courseHoles)
        {
            var frontAndBackNineScoreNames = new Dictionary<string, List<string>>();
            var frontNineScoreNames = new List<string>();
            var backNineScoreNames = new List<string>();

            for (int i = 0; i < scores.Count; i++)
            {
                string name;
                int difference = scores[i].HoleScore - courseHoles[scores[i].HoleNumber - 1].Par;
                switch (difference)
                {
                    case -3:
                        name = "albatross-score";
                        break;
                    case -2:
                        name = "eagle-score";
                        break;
                    case -1:
                        name = "birdie-score";
                        break;
                    case 0:
                        name = "par-score";
                        break;
                    case 1:
                        name = "bogie-score";
                        break;
                    case 2:
                        name = "double-bogie-score";
                        break;
                    default:
                        name = "other-score";
                        break;
                }

                if (scores[i].HoleNumber < 10)
                {
                    frontNineScoreNames.Add(name);
                }
                else if (scores[i].HoleNumber >= 10)
                {
                    backNineScoreNames.Add(name);
                }
            }
            frontAndBackNineScoreNames.Add("frontNineScoreNames", frontNineScoreNames);
            frontAndBackNineScoreNames.Add("backNineScoreNames", backNineScoreNames);
            return frontAndBackNineScoreNames;
        }

        private static Dictionary<string, object> GetPriorAndSubsequentRounds(List<RoundOfGolf> roundsOfGolf, RoundOfGolf roundOfGolf)
        {
            int idOfPriorRound           = 0;
            int idOfSubsequentRound      = 0;
            var dateOfPriorRound         = new DateTime();
            var dateOfSubsequentRound    = new DateTime();
            var priorAndSubsequentRounds = new Dictionary<string, object>();

            for (int i = 0; i < roundsOfGolf.Count; i++)
            {
                if (roundsOfGolf[i].Id == roundOfGolf.Id)
                {
                    if (i == 0)
                    {
                        idOfPriorRound = -1;
                    }
                    else
                    {
                        idOfPriorRound = roundsOfGolf[i - 1].Id;
                        dateOfPriorRound = roundsOfGolf[i - 1].Date;
                    }

                    if ((i + 1) == roundsOfGolf.Count)
                    {
                        idOfSubsequentRound = -1;
                    }
                    else
                    {
                        idOfSubsequentRound = roundsOfGolf[i + 1].Id;
                        dateOfSubsequentRound = roundsOfGolf[i + 1].Date;
                    }
                }
            }
            priorAndSubsequentRounds.Add("priorRound", new { id = idOfPriorRound, date = dateOfPriorRound });
            priorAndSubsequentRounds.Add("subsequentRound", new { id = idOfSubsequentRound, date = dateOfSubsequentRound });
            return priorAndSubsequentRounds;
        }
    }
}
