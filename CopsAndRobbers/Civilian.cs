using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    internal class Civilian : Person
    {
        public Civilian(int randomX, int randomY, string name = "Unknown Civilian") : base(randomX, randomY, name, "Civilian")
        {
            Character = "C";
            Charactercolor = ConsoleColor.Green;
            Inventory = new Inventory(new List<string> { "Keys", "Phone", "Cash", "Watch" });
        }

    }
}
