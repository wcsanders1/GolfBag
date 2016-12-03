using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class RoundOfGolf
    {
        public int Id { get; set; }

        [Required]
        public List<Score> Scores { get; set; }

        public string PlayerName { get; set; }

        public int CourseId { get; set; }

        public int TeeBoxPlayed { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{MMMM d, yyyy}")]
        public DateTime Date { get; set; }

        public string Comment { get; set; }
    }
}
