namespace CopsAndRobbers
{
    internal class Person : Inventory
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Character { get; set; }
        public string Name { get; set; }
        public ConsoleColor Charactercolor { get; set; }

        public Person() { }

        public Person(int x, int y)
        {
            X = x;
            Y = y;
        }

        public virtual void Draw()
        {
            Console.ForegroundColor = Charactercolor;
            Console.SetCursorPosition(Y, X);
            Console.Write(Character);
            Console.ResetColor();
        }
    }

    class Thief : Person
    {
        public Thief(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "T";
            Name = "Thief";
            Charactercolor = ConsoleColor.Red;
        }
    }

    class Police : Person
    {
        public Police(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "P";
            Name = "Police";
            Charactercolor = ConsoleColor.Blue;
        }

    }

    class Civilian : Person
    {
        public Civilian(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "C";
            Name = "Civilian";

            Charactercolor = ConsoleColor.Green;
        }
    }
}
