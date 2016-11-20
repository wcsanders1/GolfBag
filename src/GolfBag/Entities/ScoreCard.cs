using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GolfBag.Entities
{
    public enum CourseType
    {
        Easy,
        Medium,
        Hard
    }
    public class ScoreCard
    {

        //public int[] Score { get; set; }
        public int Id { get; set; }

        [Required, MaxLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        public string CourseName { get; set; }
        public string Scores { get; set; }
        public CourseType Course { get; set; }
    }
}
