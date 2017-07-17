using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class Score
    {
        public int Id { get; set; }
        public int HoleScore { get; set; }
        public int HolePutt { get; set; }
        public int HoleNumber { get; set; }
    }
}
