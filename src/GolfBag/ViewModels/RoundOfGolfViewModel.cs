using GolfBag.Entities;
using System;
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

        public string CourseName { get; set; }

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

        public int IdOfPriorRound { get; set; }

        public int IdOfSubsequentRound { get; set; }

        public List<TeeBox> TeeBoxes { get; set; }

        public List<int> Pars { get; set; }

        public List<int> Handicaps { get; set; }

        public static RoundOfGolfViewModel MapCourseToRoundOfGolfViewModel(Course course)
        {
            var roundOfGolfViewModel = new RoundOfGolfViewModel();

            roundOfGolfViewModel.Pars = MapPars(course);
            roundOfGolfViewModel.Handicaps = MapHandicaps(course);
            roundOfGolfViewModel.CourseName = course.CourseName;
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
            
            var roundOfGolfViewModel = new RoundOfGolfViewModel();
            var priorAndSubsequentRounds = new Dictionary<string, Tuple<int, DateTime>>();
            Dictionary<string, List<int>> frontAndBackNineScores = GetFrontAndBackNineScores(roundOfGolf); 
            

            roundOfGolfViewModel.Pars = MapPars(roundOfGolf, course.CourseHoles);
            roundOfGolfViewModel.TeeBoxes = course.TeeBoxes;
            roundOfGolfViewModel.FrontNineScores = frontAndBackNineScores["frontNineScores"];
            roundOfGolfViewModel.BackNineScores = frontAndBackNineScores["backNineScores"];
            roundOfGolfViewModel.CourseName = course.CourseName;
            roundOfGolfViewModel.NumberOfHoles = roundOfGolf.Scores.Count();
            roundOfGolfViewModel.DateOfRound = roundOfGolf.Date;
            roundOfGolfViewModel.Comment = roundOfGolf.Comment;
            roundOfGolfViewModel.Id = roundOfGolf.Id;
            roundOfGolfViewModel.IdOfTeeBoxPlayed = roundOfGolf.TeeBoxPlayed;

            for (int i = 0; i < roundsOfGolf.Count; i++)
            {
                if (roundsOfGolf[i].Date == roundOfGolf.Date)
                {
                    if (i == 0)
                    {
                        roundOfGolfViewModel.IdOfPriorRound = -1;
                    }
                    else
                    {
                        roundOfGolfViewModel.IdOfPriorRound = roundsOfGolf[i - 1].Id;
                        roundOfGolfViewModel.DateOfPriorRound = roundsOfGolf[i - 1].Date;
                    }

                    if ((i + 1) == roundsOfGolf.Count)
                    {
                        roundOfGolfViewModel.IdOfSubsequentRound = -1;
                    }
                    else
                    {
                        roundOfGolfViewModel.IdOfSubsequentRound = roundsOfGolf[i + 1].Id;
                        roundOfGolfViewModel.DateOfSubsequentRound = roundsOfGolf[i + 1].Date;
                    }
                }
            }
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
    }
}
