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

        public Person(int x, int y, string name, string roleName)
        {
            X = x;
            Y = y;
            Name = name;
            RoleName = roleName;

        }

        public void ShowPersonsInfo()
        {
            Console.WriteLine($"{RoleName} \'{Name}\' at position ({X}, {Y}) | Inventory: {string.Join(", ", Inventory.Items)}");
        }
    }
   
}
