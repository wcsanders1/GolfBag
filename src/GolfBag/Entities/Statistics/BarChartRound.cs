using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities.Statistics
{
    public class BarChartRound
    {
        public int RoundScore { get; set; }
        public int RoundId { get; set; }
        public string CourseName { get; set; }
        public string RoundDate { get; set; }
    }
}
