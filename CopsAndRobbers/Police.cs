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

        // New method to arrest thieves
        public bool ArrestThief(Thief thief, Random prisonRnd, City city, Prison prison)
        {
            if (thief.IsImprisoned)
            {
                Helpers.DrawLog($"{Name} tried to arrest {thief.Name}, but they are already in prison!", city, prison);
                return false;
            }


            //If innocent
            if (thief.Inventory.Items.Count > 0)
            {
                // Police have a high success rate for arrests (90%)

                if (prisonRnd.Next(100) < 90)
                {

                    // First confiscate any stolen items
                    StealFrom(thief, prisonRnd, city, prison);


                    // Then send thief to prison
                    thief.SendToPrison(prison);

                    // Position thief in prison (random position within prison bounds)
                    thief.X = city.Height + 1 + prisonRnd.Next(1, prison.Height - 2); // Prison starts after city
                    thief.Y = prisonRnd.Next(1, prison.Width - 2);

                    Helpers.DrawLog($"{Name} successfully arrested {thief.Name} and sent them to prison!", city, prison);
                    return true;
                }
                else
                {
                    Helpers.DrawLog($"{Name} tried to arrest {thief.Name} but they escaped!", city, prison);
                    return false;
                }
            }
            else
            {
                Helpers.DrawLog($"{Name} Looked through {thief.Name}'s  pockets! 'Twas empty indeed!", city, prison);
                return false;
            }
        }
    }
}
