using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class ScoreCardEditViewModel
    {
        [Required, MaxLength(80)]
        public string PlayerName { get; set; }
        public string CourseName { get; set; }
        public CourseType Course { get; set; }

    }
}
