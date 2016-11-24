using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string PlayerName { get; set; }
        public int NumberOfHoles { get; set; }
        public ICollection<CourseHole> CourseHoles { get; set; }
    }
}
