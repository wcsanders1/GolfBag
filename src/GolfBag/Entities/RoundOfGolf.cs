using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class RoundOfGolf
    {
        public int Id { get; set; }
        public ICollection<Score> Scores { get; set; }
        public string PlayerName { get; set; }
        public int CourseId { get; set; }
        //public DateTime Date { get; set; }
    }
}
