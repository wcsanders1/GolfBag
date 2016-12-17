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

        public Course MapViewModelToCourse(string name)
        {
            var course = new Course();

            TeeBoxes.RemoveRange(int.Parse(NumberOfTeeBoxes), TeeBoxes.Count - int.Parse(NumberOfTeeBoxes));

            course.PlayerName = name;
            course.CourseName = CourseName;
            course.NumberOfHoles = NumberOfHoles;
            course.CourseHoles = MapCourseHoles();
            course.TeeBoxes = MapTeeBoxes();

            return course;
        }

        private List<CourseHole> MapCourseHoles()
        {
            var courseHoles = new List<CourseHole>();

            for (int i = 0; i < NumberOfHoles; i++)
            {
                var courseHole = new CourseHole();
                courseHole.HoleNumber = i + 1;
                courseHole.Par = Pars[i];
                courseHole.Handicap = Handicaps[i];
                courseHoles.Add(courseHole);
                courseHoles.Add(courseHole);
            }
            return courseHoles;
        }

        private List<TeeBox> MapTeeBoxes()
        {
            var teeBoxes = new List<TeeBox>();
            for (int i = 0; i < int.Parse(NumberOfTeeBoxes); i++)
            {
                var teeBox = new TeeBox();
                var tees = new List<Tee>();
                teeBox.Name = TeeBoxes[i].Name;

                for (int x = 0; x < NumberOfHoles; x++)
                {
                    var tee = new Tee();
                    tee.HoleNumber = x + 1;
                    tee.Yardage = TeeBoxes[i].Tees[x].Yardage;
                    tees.Add(tee);
                }

                teeBox.Tees = tees;
                teeBoxes.Add(teeBox);
            }
            return teeBoxes;
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
}
