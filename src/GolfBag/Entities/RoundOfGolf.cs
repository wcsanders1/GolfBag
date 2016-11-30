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
        public ICollection<Score> Scores { get; set; }

        public string PlayerName { get; set; }

        public int CourseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{M/dd/yy}")]
        public DateTime Date { get; set; }

        public string Comment { get; set; }
    }
}
