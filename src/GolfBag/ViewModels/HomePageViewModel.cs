using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public enum PlayLevel
    {
        NotGood = 1,
        AlmostDecent = 2,
        Decent = 3,
        Good = 4
    }

    public class HomePageViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{MMMM d, yyyy}")]
        public DateTime DateOfLastRound { get; set; }

        public string CourseLastPlayed { get; set; }

        public int NumberOfHolesPlayedInLastRound { get; set; }

        public int ScoreOfLastRound { get; set; }

        public int DaysSinceLastRound { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Handicap { get; set; }

        public PlayLevel PlayLevel { get; set; }
    }
}
