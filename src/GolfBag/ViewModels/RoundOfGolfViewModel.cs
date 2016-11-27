using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class RoundOfGolfViewModel
    {
        public List<int> FrontNineScores { get; set; }
        public List<int> BackNineScores { get; set; }
        public string CourseName { get; set; }
        public int NumberOfHoles { get; set; }
        public int Id { get; set; }
        public List<int> Yardages { get; set; }
        public List<int> Pars { get; set; }
    }
}
