using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class CourseHole
    {
        public int Id { get; set; }
        public int Yardage { get; set; }
        public int HoleNumber { get; set; }
        public int Par { get; set; }
    }
}
