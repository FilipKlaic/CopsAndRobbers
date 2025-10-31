using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    class Thief : Person
    {
        public Thief(int randomX, int randomY, string name = "Unknown Thief") : base(randomX, randomY, name, "Thief")
        {
            Character = "T";
            Charactercolor = ConsoleColor.Red;
        }

        public bool StealFrom(Civilian civilian, Random rnd, City city, Prison prison)
        {
            if (civilian.Inventory.Items.Count == 0)
            {
                Helpers.DrawLog($"{Name} tried to steal from {civilian.Name}, but they have nothing to steal!", city, prison);
                return false;
            }

            // Add a success rate (e.g., 70% chance)
            if (rnd.Next(100) < 70)
            {
                int randomIndex = rnd.Next(civilian.Inventory.Items.Count);
                string stolenItem = civilian.Inventory.Items[randomIndex];

                civilian.Inventory.RemoveItem(stolenItem);
                this.Inventory.AddItem(stolenItem);

                Helpers.DrawLog($"{Name} successfully stole {stolenItem} from {civilian.Name}!", city, prison);

                return true;
            }
            else
            {
                Helpers.DrawLog($"{Name} tried to steal from {civilian.Name} but failed!", city, prison);
                return false;
            }
        }

    }
}
