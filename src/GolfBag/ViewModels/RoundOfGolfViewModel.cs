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

        public string Comment { get; set; }

        public int NumberOfHoles { get; set; }

        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{M/dd/yyyy}")]
        public DateTime DateOfRound { get; set; }

        public DateTime DateOfPriorRound { get; set; }

        public DateTime DateOfSubsequentRound { get; set; }

        public List<int> Yardages { get; set; }
        public List<int> Pars { get; set; }
    }
}
