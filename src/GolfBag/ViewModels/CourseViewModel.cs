using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class CourseViewModel
    {
        public string CourseName { get; set; }
        public int NumberOfHoles { get; set; }
        public List<int> Yardages { get; set; }
        public List<int> Pars { get; set; }
    }
}
