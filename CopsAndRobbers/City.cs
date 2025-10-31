using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class City : Place
    {
        public City() : base("CITY", 100, 25)  // Reduced from 100x25 to 80x20
        {
        }

        // City's methods can be added
    }
}
