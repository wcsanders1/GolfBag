using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int NumberOfHoles { get; set; }
        public string NumberOfTeeBoxes { get; set; }
        public List<TeeBox> TeeBoxes { get; set; }
        public List<int> Pars { get; set; }
        public List<int> Handicaps { get; set; }
    }
}
