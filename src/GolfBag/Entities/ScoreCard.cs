using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string PlayerName { get; set; }
        public string CourseName { get; set; }
        public CourseType Course { get; set; }
    }
}
