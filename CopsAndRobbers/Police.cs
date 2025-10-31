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

        public bool StealFrom(Thief thief, Random rnd, City city, Prison prison)
        {
            if (thief.Inventory.Items.Count == 0)
            {
                Helpers.DrawLog($"{Name} tried to confiscate items from {thief.Name}, but they have nothing to confiscate!", city, prison);
                return false;
            }

            // Police have 100% success rate and take ALL items
            List<string> stolenItems = new List<string>(thief.Inventory.Items); // Create a copy of all items

            // Transfer all items from thief to police
            foreach (string item in stolenItems)
            {
                thief.Inventory.RemoveItem(item);
                this.Inventory.AddItem(item);
            }

            Helpers.DrawLog($"{Name} successfully confiscated all items ({string.Join(", ", stolenItems)}) from {thief.Name}!", city, prison);

            return true;
        }
    }

}
