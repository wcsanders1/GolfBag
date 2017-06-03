using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities.Statistics
{
    public class PuttToTwo
    {
        [JsonProperty(PropertyName = "scoreName")]
        public string PuttName { get; set; }
        public float Percentage { get; set; }
    }
}
