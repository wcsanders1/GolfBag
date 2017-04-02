using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class TeeBox
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Tee> Tees { get; set; }

        //[NotMapped]
        //public int SumFrontYardage
        //{
        //    get
        //    {
        //        if (Tees.Count >= 9)
        //        {
        //            return Tees.Take(9).Sum(x => x.Yardage);
        //        }
        //        return 0;
        //    }
        //}

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
