namespace CopsAndRobbers
{
    internal class Inventory
    {
        public List<string> Items { get; set; }

        public Inventory()
        {
            Items = new List<string>();
        }

        public Inventory(List<string> items)
        {
            Items = items;
        }

        public void AddItem(string item)
        {
            Items.Add(item);
        }

        public void RemoveItem(string item)
        {
            Items.Remove(item);
        }

        public void ShowItems()
        {
            if (Items.Count == 0)
                Console.WriteLine("Inventory list is empty");
            else
                Console.WriteLine(string.Join(", ", Items));
        }
    }
}
