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

        public List<ViewTeeBox> NewTeeBoxes { get; set; }

        public List<int> Pars { get; set; }

        public List<int> Handicaps { get; set; }

        public List<int> ListOfDeletedTeeBoxes { get; set; }

        public CourseViewModel()
        {
            ListOfDeletedTeeBoxes = new List<int>();
            TeeBoxes = new List<ViewTeeBox>();
            NewTeeBoxes = new List<ViewTeeBox>();
        }

        public void ProduceListOfNewTeeBoxes(Course course)
        {
            for (int i = 0; i < (6 - course.TeeBoxes.Count); i++)
            {
                var newViewTeeBox = new ViewTeeBox();

                for (int x = 0; x < course.NumberOfHoles; x++)
                {
                    var newViewTee = new ViewTee();
                    newViewTeeBox.Tees.Add(newViewTee);
                }
                NewTeeBoxes.Add(newViewTeeBox);
            }
        }

        public void ProduceListOfDeletedTeeBoxes(Course course)
        {
            for (int i = 0; i < course.TeeBoxes.Count; i++)
            {
                ListOfDeletedTeeBoxes.Add(0);
            }
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
