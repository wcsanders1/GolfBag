using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class EnterCourseViewModel
    {
        public string CourseName { get; set; }
        public int NumberOfHoles { get; set; }
        public List<int> Yardage { get; set; }
        public List<int> Par { get; set; }
    }
}
