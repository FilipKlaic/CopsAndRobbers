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
