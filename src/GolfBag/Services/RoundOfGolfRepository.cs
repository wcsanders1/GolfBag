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
            return _context.RoundsOfGolf
                .Where(r => r.PlayerName == playerName)
                .Include(r => r.Scores)
                .ToList();
        }

        public IEnumerable<Course> GetAllCourses(string playerName)
        {
            return _context.Courses
                .Where(r => r.PlayerName == playerName)
                .Include(r => r.CourseHoles)
                .ToList();
        }

        public Course GetCourse(string courseName)
        {
            return _context.Courses
                .Where(r => r.CourseName == courseName)
                .Include(r => r.CourseHoles)
                .FirstOrDefault();
        }

        public Course GetCourse(int id)
        {
            return _context.Courses
                .Where(r => r.Id == id)
                .Include(r => r.CourseHoles)
                .FirstOrDefault();
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
            return _context.RoundsOfGolf
                .Where(r => r.Id == id)
                .Include(r => r.Scores)
                .FirstOrDefault();
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
            _context.Remove(course);
            Commit();
            return true;

        }
    }
}
