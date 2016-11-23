using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Services
{
    public interface IRoundOfGolf
    {
        IEnumerable<RoundOfGolf> GetAll(string playerName);

        //RoundOfGolf Get(int id);
        void Add(RoundOfGolf newRoundOfGolf);
        void Delete(RoundOfGolf roundOfGolf);
        int Commit();
    }
}
