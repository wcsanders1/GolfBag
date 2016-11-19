using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class ScoreCardEditViewModel
    {
        public string PlayerName { get; set; }
        public string CourseName { get; set; }
        public CourseType Course { get; set; }

    }
}
