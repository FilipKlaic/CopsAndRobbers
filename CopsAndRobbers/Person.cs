namespace CopsAndRobbers
{
    internal class Person 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Character { get; set; }
        public ConsoleColor Charactercolor { get; set; }
        public Inventory Inventory { get; set; } = new Inventory(); // Inventory is created automatically for all objects

        public Person() { }

        public Person(int x, int y)
        {
            X = x;
            Y = y;
            // X and Y are used when the character's position is set during creation.
            // If positions are assigned randomly later, this constructor can be omitted
            // and the simpler initialization can be used instead, for example:
            // var thief = new Thief();
            // thief.X = randomX;
            // thief.Y = randomY;

        }

        public virtual void Draw()
        {
            Console.ForegroundColor = Charactercolor;
            Console.SetCursorPosition(Y, X);
            Console.Write(Character);
            Console.ResetColor();
        }

        public void ShowInventory()
        {
            Console.Write($"{Character} inventory: ");
            Inventory.ShowItems();
        }
    }


    // Each subclass has two constructors now: one for creating with specific coordinates,
    // and one default constructor for random or later-assigned positions.


    class Thief : Person
    {
        public Thief(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "T";
            Charactercolor = ConsoleColor.Red;
        }

        public Thief()
        {
            Character = "T";
            Charactercolor = ConsoleColor.Red;
        }
    }

    class Police : Person
    {
        public Police(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "P";
            Charactercolor = ConsoleColor.Blue;
        }

        public Police()
        {
            Character = "P";
            Charactercolor = ConsoleColor.Blue;
        }
    }

    class Civilian : Person
    {
        public Civilian(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "C";
            Charactercolor = ConsoleColor.Green;
            Inventory = new Inventory(new List<string> { "Keys", "Phone", "Cash", "Watch" });
        }

        public Civilian()
        {
            Character = "C";
            Charactercolor = ConsoleColor.Green;
            Inventory = new Inventory(new List<string> { "Keys", "Phone", "Cash", "Watch" });
        }
    }
}
