using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Services
{
    public interface IRoundOfGolf
    {
        IEnumerable<RoundOfGolf> GetAllRounds(string playerName);
        IEnumerable<Course> GetAllCourses(string playerName);
        RoundOfGolf GetRound(int id);
        void AddRound(RoundOfGolf newRoundOfGolf);
        void AddCourse(Course newCourse);
        void SaveCourseEdits(Course course, List<int> teeboxesToDelete);
        void SaveRoundEdits(RoundOfGolf round);
        bool DeleteCourse(Course course);
        void DeleteRound(RoundOfGolf round);
        Course GetCourse(string courseName);
        Course GetCourse(int id);
        int GetCourseId(string courseName);
        int Commit();
    }
}
