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

        public void DeleteRound(RoundOfGolf roundOfGolf)
        {
            _context.Remove(roundOfGolf);
            Commit();
        }

        public IEnumerable<RoundOfGolf> GetAllRounds(string playerName)
        {
            return _context.RoundsOfGolf.Where(r => r.PlayerName == playerName);
        }

        public IEnumerable<Course> GetAllCourses(string playerName)
        {
            return _context.Courses
                .Where(r => r.PlayerName == playerName)
                .ToList();
        }

        public Course GetCourse(string courseName)
        {
            return _context.Courses
                .Where(r => r.CourseName == courseName)
                .Include(r => r.CourseHoles)
                .FirstOrDefault();
        }
    }
}
