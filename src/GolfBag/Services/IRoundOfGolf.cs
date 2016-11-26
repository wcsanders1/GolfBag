﻿using GolfBag.Entities;
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
        void DeleteRound(RoundOfGolf roundOfGolf);
        void AddCourse(Course newCourse);
        Course GetCourse(string courseName);
        Course GetCourse(int id);
        int GetCourseId(string courseName);
        int Commit();
    }
}
