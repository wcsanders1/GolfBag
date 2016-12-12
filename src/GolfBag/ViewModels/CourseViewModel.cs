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

        public List<ViewTeeBox> TeeBoxes { get; set; }

        public List<int> Pars { get; set; }

        public List<int> Handicaps { get; set; }

        public List<int> ListOfDeletedTeeBoxes { get; set; }

        public CourseViewModel()
        {
            ListOfDeletedTeeBoxes = new List<int>();
            TeeBoxes = new List<ViewTeeBox>();
        }
    }

    public class ViewTeeBox
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ViewTee> Tees { get; set; }

        public ViewTeeBox()
        {
            Tees = new List<ViewTee>();
        }
    }

    public class ViewTee
    {
        public int Yardage { get; set; }
    }
}
