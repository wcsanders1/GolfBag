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

        public static CourseViewModel MapCourseToCourseViewModel(Course course)
        {
            var courseViewModel = new CourseViewModel();
            Dictionary<string, List<int>> parsAndHandicaps = MapViewParsAndHandicaps(course);

            courseViewModel.CourseName = course.CourseName;
            courseViewModel.NumberOfHoles = course.NumberOfHoles;
            courseViewModel.NumberOfTeeBoxes = course.TeeBoxes.Count.ToString();
            courseViewModel.Id = course.Id;
            courseViewModel.Pars = parsAndHandicaps["pars"];
            courseViewModel.Handicaps = parsAndHandicaps["handicaps"];
            courseViewModel.TeeBoxes = MapViewTeeBoxes(course);

            return courseViewModel;
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
                teeBox.CourseRating = Convert.ToDecimal(TeeBoxes[i].CourseRating);
                teeBox.SlopeRating = TeeBoxes[i].SlopeRating;

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

        private static Dictionary<string, List<int>> MapViewParsAndHandicaps(Course course)
        {
            var parsAndHandicaps = new Dictionary<string, List<int>>();
            var pars = new List<int>();
            var handicaps = new List<int>();

            foreach (var courseHole in course.CourseHoles)
            {
                pars.Add(courseHole.Par);
                handicaps.Add(courseHole.Handicap);
            }

            parsAndHandicaps.Add("pars", pars);
            parsAndHandicaps.Add("handicaps", handicaps);
            return parsAndHandicaps;
        }

        private static List<ViewTeeBox> MapViewTeeBoxes(Course course)
        {
            var viewTeeBoxes = new List<ViewTeeBox>();
            for (int i = 0; i < course.TeeBoxes.Count; i++)
            {
                var viewTeebox = new ViewTeeBox();

                viewTeebox.Id = course.TeeBoxes[i].Id;
                viewTeebox.Name = course.TeeBoxes[i].Name;
                viewTeebox.CourseRating = course.TeeBoxes[i].CourseRating.ToString("N1") ?? "0";
                viewTeebox.SlopeRating = course.TeeBoxes[i].SlopeRating;

                for (int x = 0; x < course.TeeBoxes[i].Tees.Count; x++)
                {
                    var viewTee = new ViewTee();
                    viewTee.Yardage = course.TeeBoxes[i].Tees[x].Yardage;
                    viewTeebox.Tees.Add(viewTee);
                }
                viewTeeBoxes.Add(viewTeebox);
            }
            return viewTeeBoxes;
        }
    }
}
