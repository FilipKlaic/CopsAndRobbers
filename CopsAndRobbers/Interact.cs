namespace CopsAndRobbers
{
    internal class Interact
    {
        public static void StolenGoods(List<List<Person>> collisions)
        {
            foreach (var populatedIndex in collisions)
            {

                bool hasThief = populatedIndex.Any(t => t is Thief); //checks if the thief is in the same index


                if (!hasThief) continue;

                foreach (var civilian in populatedIndex.OfType<Civilian>())
                {
                    if (civilian.Inventory.Items.Count > 0)
                    {

                        var theif = populatedIndex.OfType<Thief>().FirstOrDefault();



                        // Transfer all items from civ to thief
                        foreach (var item in civilian.Inventory.Items)
                        {
                            theif.Inventory.AddItem(item);
                        }



                        // Clear civilians inventory
                        civilian.Inventory.Items.Clear();
                    }
                }

            }

        }



        public static List<Thief> CaughtThieves(List<List<Person>> collisions)
        {

            List<Thief> caughtThieves = new List<Thief>(); // adds a new list for the caught theives

            foreach (var populatedIndex in collisions)
            {
                bool hasPolice = populatedIndex.Any(p => p is Police); //checks if the cop is in the same index

                //add if thief has something in pocket

                if (!hasPolice) continue;


                foreach (var thief in populatedIndex.OfType<Thief>())
                {
                    // Only catch thieves who have stolen items (more than 0 items)
                    if (thief.Inventory != null && thief.Inventory.Items.Count > 0)
                    {
                        caughtThieves.Add(thief);

                        var police = populatedIndex.OfType<Police>().FirstOrDefault();



                        // Transfer all items from thief to police
                        foreach (var item in thief.Inventory.Items)
                        {
                            police.Inventory.AddItem(item);
                        }



                        // Clear thief's inventory
                        thief.Inventory.Items.Clear();
                    }
                }

            }

            return caughtThieves; // if thief not caught = caught thieves = empty



        }



    }
}
