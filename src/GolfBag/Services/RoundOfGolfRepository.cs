using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfBag.Entities;
using Microsoft.EntityFrameworkCore;

namespace GolfBag.Services
{
    public class RoundOfGolfRepository : IRoundOfGolf
    {
        private ScoreCardDbContext _context;

        public RoundOfGolfRepository(ScoreCardDbContext context)
        {
            _context = context;
        }
        public void AddRound(RoundOfGolf newRoundOfGolf)
        {
            _context.Add(newRoundOfGolf);
            Commit();
        }

        public void AddCourse(Course newCourse)
        {
            _context.Add(newCourse);
            Commit();
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public IEnumerable<RoundOfGolf> GetAllRounds(string playerName)
        {
            IEnumerable<RoundOfGolf> rounds = _context.RoundsOfGolf
                .Where(r => r.PlayerName == playerName)
                .Include(r => r.Scores)
                .OrderBy(r => r.Date)
                .ToList();

            foreach (var round in rounds)
            {
                round.Scores = round.Scores.OrderBy(r => r.HoleNumber).ToList();
            }

            return rounds;
        }

        public IEnumerable<Course> GetAllCourses(string playerName)
        {
            IEnumerable<Course> courses = _context.Courses
                .Where(r => r.PlayerName == playerName)
                .Include(r => r.CourseHoles)
                .Include(r => r.TeeBoxes)
                .ThenInclude(m => m.Tees)
                .OrderBy(r => r.CourseName)
                .ToList();

            foreach (var course in courses)
            {
                course.CourseHoles = course.CourseHoles.OrderBy(r => r.HoleNumber).ToList();
                course.TeeBoxes = course.TeeBoxes.OrderByDescending(r => r.Tees.Sum(m => m.Yardage)).ToList();

                for (int i = 0; i < course.TeeBoxes.Count; i++)
                {
                    course.TeeBoxes[i].Tees = course.TeeBoxes[i].Tees.OrderBy(r => r.HoleNumber).ToList();
                }
            }

            return courses;
        }

        public Course GetCourse(string courseName)
        {
            Course course = _context.Courses
                .Where(r => r.CourseName == courseName)
                .Include(r => r.CourseHoles)                
                .Include(r => r.TeeBoxes)
                .ThenInclude(m => m.Tees)
                .FirstOrDefault();

            course.CourseHoles = course.CourseHoles.OrderBy(r => r.HoleNumber).ToList();
            course.TeeBoxes = course.TeeBoxes.OrderByDescending(r => r.Tees.Sum(m => m.Yardage)).ToList();

            for (int i = 0; i < course.TeeBoxes.Count; i++)
            {
                course.TeeBoxes[i].Tees = course.TeeBoxes[i].Tees.OrderBy(r => r.HoleNumber).ToList();
            }

            return course;
        }

        public Course GetCourse(int id)
        {
            Course course = _context.Courses
                .Where(r => r.Id == id)
                .Include(r => r.CourseHoles)
                .Include(r => r.TeeBoxes)
                .ThenInclude(m => m.Tees)
                .FirstOrDefault();

            course.CourseHoles = course.CourseHoles.OrderBy(r => r.HoleNumber).ToList();
            course.TeeBoxes = course.TeeBoxes.OrderByDescending(r => r.Tees.Sum(m => m.Yardage)).ToList();

            for (int i = 0; i < course.TeeBoxes.Count; i++)
            {
                course.TeeBoxes[i].Tees = course.TeeBoxes[i].Tees.OrderBy(r => r.HoleNumber).ToList();
            }

            return course;
        }

        public int GetCourseId(string courseName)
        {
            var course = new Course();
            course = _context.Courses
                .Where(r => r.CourseName == courseName)
                .FirstOrDefault();

            return course.Id;
        }

        public RoundOfGolf GetRound(int id)
        {
            RoundOfGolf round = _context.RoundsOfGolf
                .Where(r => r.Id == id)
                .Include(r => r.Scores)
                .FirstOrDefault();

            round.Scores = round.Scores.OrderBy(r => r.HoleNumber).ToList();

            return round;
        }

        public void SaveCourseEdits(Course course)
        {
            _context.Entry(course).State = EntityState.Modified;
            Commit();
        }

        public void SaveRoundEdits(RoundOfGolf round)
        {
            _context.Entry(round).State = EntityState.Modified;
            Commit();
        }
        public void DeleteRound(RoundOfGolf roundOfGolf)
        {
            foreach (var score in roundOfGolf.Scores)
            {
                _context.Remove(score);
            }

            _context.Remove(roundOfGolf);
            Commit();
        }

        public bool DeleteCourse(Course course)
        {
            if (_context.RoundsOfGolf.Any(r => r.CourseId == course.Id))
            {
                return false;
            }

            foreach (var courseHole in course.CourseHoles)
            {
                _context.Remove(courseHole);
            }

            foreach (var teeBox in course.TeeBoxes)
            {
                foreach (var tee in teeBox.Tees)
                {
                    _context.Remove(tee);
                }
                _context.Remove(teeBox);
            }
            _context.Remove(course);
            Commit();
            return true;
        }
    }
}
