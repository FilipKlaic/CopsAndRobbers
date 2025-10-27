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

        public static string GetRandomLastName()
        {

            Random rnd = new Random();
            int index = rnd.Next(Names.LastNames.Count);
            string name = Names.LastNames[index];  // store first
            Names.LastNames.RemoveAt(index);       // then remove it
            return name;
        }

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
            Name = "Thief " + GetRandomLastName();
            Charactercolor = ConsoleColor.Red;
        }
    }

    class Police : Person
    {
        public Police(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "P";
            Name = "Police " + GetRandomLastName();
            Charactercolor = ConsoleColor.Blue;
        }
    }

    class Civilian : Person
    {
        public Civilian(int randomX, int randomY) : base(randomX, randomY)
        {
            Character = "C";
            Name = "Civilian " + GetRandomLastName(); ;
            Charactercolor = ConsoleColor.Green;
        }
    }

    static class Names
    {
        public static List<string> LastNames = new List<string>        {
            "Baggins",
            "Stark",
            "Lupin",
            "Robin",
            "Fox",
            "Carter",
            "Vimes",
            "Montoya",
            "Marple",
            "Bonnie",
            "Clyde",
            "Archer",
            "Gambit",
            "Parker",
            "Addams"
        };
    }
}
