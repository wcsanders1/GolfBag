using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class TeeBox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tee> Tees { get; set; }
    }
}
