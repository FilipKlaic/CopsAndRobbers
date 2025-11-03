namespace CopsAndRobbers
{
    internal class Person : Inventory
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Character { get; set; }
        public string Name { get; set; }
        public ConsoleColor Charactercolor { get; set; }
        public Inventory Inventory { get; set; } = new Inventory();

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
            Inventory = new Inventory(new List<string> { "Keys", "Phone", "Cash", "Watch" });

        }
    }

    static class Names
    {
        public static List<string> LastNames = new List<string>
        {
            // Original names
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
            "Addams",
            
            // Additional names (45 more to reach 4x = 60 total)
            "Holmes",
            "Watson",
            "Poirot",
            "Bond",
            "Bourne",
            "McClane",
            "Ripley",
            "Connor",
            "Wick",
            "Hunt",
            "Drake",
            "Croft",
            "Freeman",
            "Shepard",
            "Price",
            "Soap",
            "Ghost",
            "Roach",
            "Woods",
            "Mason",
            "Reznov",
            "Menendez",
            "Makarov",
            "Zakhaev",
            "Shepherd",
            "Ramirez",
            "Morgan",
            "Cross",
            "Fisher",
            "Lambert",
            "Grimsdottir",
            "Redding",
            "Bishop",
            "Torres",
            "Reed",
            "Kane",
            "Lynch",
            "Stone",
            "Black",
            "White",
            "Gray",
            "Green",
            "Brown",
            "Silver",
            "Gold"
        };
    }
}