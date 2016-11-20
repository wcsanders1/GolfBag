using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Services
{
    public interface IGreeter
    {
        string GetGreeting();
    }
    public class Greeter : IGreeter
    {
        private string _greeting;
        public Greeter(IConfigurationRoot configuration)
        {
            _greeting = configuration["greeting"].ToString();
        }
        public string GetGreeting()
        {
            return _greeting;
        }
    }
}
