using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GolfBag.Entities
{
    public class TeeBox
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal SlopeRating { get; set; }

        public decimal CourseRating { get; set; }

        public List<Tee> Tees { get; set; }

        [NotMapped]
        public int TotalYardage
        {
            get
            {
                if (Tees.Count == 18)
                {
                    return Tees.Sum(x => x.Yardage);
                }
                return 0;
            }
        }
    }
}
