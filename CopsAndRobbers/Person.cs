namespace CopsAndRobbers
{
    internal class Person 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Character { get; set; }
        public ConsoleColor Charactercolor { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public Inventory Inventory { get; set; } = new Inventory(); // Inventory is created automatically for all objects

        //public Person() { }

        public Person(int x, int y, string name, string roleName)
        {
            X = x;
            Y = y;
            Name = name;
            RoleName = roleName;
            // X and Y are used when the character's position is set during creation.
            // If positions are assigned randomly later, this constructor can be omitted
            // and the simpler initialization can be used instead, for example:
            // var thief = new Thief();
            // thief.X = randomX;
            // thief.Y = randomY;

        }

        public virtual void Draw()
        {
            // Ensure the cursor position is within console bounds
            if (Y >= 0 && Y < Console.WindowWidth && X >= 0 && X < Console.WindowHeight)
            {
                Console.ForegroundColor = Charactercolor;
                Console.SetCursorPosition(Y, X);  // Note: SetCursorPosition takes (column, row) = (Y, X)
                Console.Write(Character);
                Console.ResetColor();
            }
        }

        public void ShowPersonsInfo()
        {
            Console.WriteLine($"{RoleName} \'{Name}\' at position ({X}, {Y}) | Inventory: {string.Join(", ", Inventory.Items)}");
        }
    }


    // Each subclass has two constructors now: one for creating with specific coordinates,
    // and one default constructor for random or later-assigned positions.


    class Thief : Person
    {
        public Thief(int randomX, int randomY, string name = "Unknown Thief") : base(randomX, randomY, name, "Thief")
        {
            Character = "T";
            Charactercolor = ConsoleColor.Red;
        }

        public bool StealFrom(Civilian civilian, Random rnd)
        {
            if (civilian.Inventory.Items.Count == 0)
            {
                Console.WriteLine($"{Name} tried to steal from {civilian.Name}, but they have nothing to steal!");
                return false;
            }

            // Add a success rate (e.g., 70% chance)
            if (rnd.Next(100) < 70)
            {
                int randomIndex = rnd.Next(civilian.Inventory.Items.Count);
                string stolenItem = civilian.Inventory.Items[randomIndex];
                
                civilian.Inventory.RemoveItem(stolenItem);
                this.Inventory.AddItem(stolenItem);
                
                Console.WriteLine($"{Name} successfully stole {stolenItem} from {civilian.Name}!");
                return true;
            }
            else
            {
                Console.WriteLine($"{Name} tried to steal from {civilian.Name} but failed!");
                return false;
            }
        }

    }

    class Police : Person
    {
        public Police(int randomX, int randomY, string name = "Unknown Police officer") : base(randomX, randomY, name, "Police officer")
        {
            Character = "P";
            Charactercolor = ConsoleColor.Blue;
        }

        //public Police()
        //{
        //    Character = "P";
        //    Charactercolor = ConsoleColor.Blue;
        //}
    }

    class Civilian : Person
    {
        public Civilian(int randomX, int randomY, string name = "Unknown Civilian") : base(randomX, randomY, name, "Civilian")
        {
            Character = "C";
            Charactercolor = ConsoleColor.Green;
            Inventory = new Inventory(new List<string> { "Keys", "Phone", "Cash", "Watch" });
        }

        //public Civilian()
        //{
        //    Character = "C";
        //    Charactercolor = ConsoleColor.Green;
        //    Inventory = new Inventory(new List<string> { "Keys", "Phone", "Cash", "Watch" });
        //}
    }
}
