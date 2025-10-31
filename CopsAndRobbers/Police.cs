using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Police : Person
    {
        public Police(int randomX, int randomY, string name = "Unknown Police officer") : base(randomX, randomY, name, "Police officer")
        {
            Character = "P";
            Charactercolor = ConsoleColor.Blue;
        }


    }

}
