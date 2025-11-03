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
        public bool IsImprisoned { get; set; } = false; // New property to track imprisonment status
        public Place CurrentLocation { get; set; } // Track which location the person is in

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
            //if (Y >= 0 && Y < Console.WindowWidth && X >= 0 && X < Console.WindowHeight)
            //{
                Console.ForegroundColor = Charactercolor;
                Console.SetCursorPosition(Y, X);  // Note: SetCursorPosition takes (column, row) = (Y, X)
                Console.Write(Character);
                Console.ResetColor();
            //}
        }

        public void ShowPersonsInfo()
        {
            Console.WriteLine($"{RoleName} \'{Name}\' at position ({X}, {Y}) | Inventory: {string.Join(", ", Inventory.Items)} | Status: {(IsImprisoned ? "Imprisoned" : "Free")}");
        }

        // Method to move character to prison
        public virtual void SendToPrison(Prison prison)
        {
            IsImprisoned = true;
            CurrentLocation = prison;
            
            // Add the person to prison's person list
            if (!prison.Persons.Contains(this))
            {
                prison.Persons.Add(this);
            }
        }

        // Method to release character from prison (for future use)
        public virtual void ReleaseFromPrison(City city)
        {
            IsImprisoned = false;
            CurrentLocation = city;
            
            // Remove from prison and add back to city if needed
            if (city.Persons != null && !city.Persons.Contains(this))
            {
                city.Persons.Add(this);
            }
        }
    }


    // Each subclass has two constructors now: one for creating with specific coordinates,
    // and one default constructor for random or later-assigned positions.
   
}
