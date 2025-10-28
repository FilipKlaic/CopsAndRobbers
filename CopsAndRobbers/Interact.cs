namespace CopsAndRobbers
{
    internal class Interact
    {

        public static List<Thief> CaughtThieves(List<List<Person>> collisions)
        {

            List<Thief> caughtThieves = new List<Thief>(); // adds a new list for the caught theives

            foreach (var populatedIndex in collisions)
            {
                bool hasPolice = populatedIndex.Any(p => p is Police); //checks if the cop is in the same index
                if (!hasPolice) continue;



                caughtThieves.AddRange(populatedIndex.OfType<Thief>()); //adds all theifs in the index
            }

            return caughtThieves;



        }



    }
}
