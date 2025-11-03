namespace CopsAndRobbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[,] drawing = Rita.DrawingClassInitilize();
            Random rnd = new Random();

            List<Person> list = new List<Person>
            {
                new Civilian(5, 5),
                new Thief(5, 5),
                new Police(5, 5),
                new Civilian(5, 5),
                new Thief(5, 5),
                new Police(5, 5),
                new Civilian(7, 7),
                new Thief(7, 7),
                new Police(7, 7),
                new Civilian(9, 9),
                new Thief(9, 9),
                new Police(9, 9),
                new Civilian(11, 11),
                new Thief(11, 11),
                new Police(11, 11),
                new Civilian(13, 13),
                new Thief(13, 13),
                new Police(13, 13),
                new Civilian(15, 15),
                new Thief(15, 15),
                new Police(15, 15),
                new Civilian(17, 17),
                new Thief(17, 17),
                new Police(17, 17),
                new Civilian(19, 19),
                new Thief(19, 19),
                new Police(19, 19),
                new Civilian(21, 21),
                new Thief(21, 21),
                new Police(21, 21),
                new Civilian(3, 10),
                new Thief(3, 10),
                new Police(3, 10),
                new Civilian(6, 15),
                new Thief(6, 15),
                new Police(6, 15),
                new Civilian(8, 20),
                new Thief(8, 20),
                new Police(8, 20),
                new Civilian(10, 25),
                new Thief(10, 25),
                new Police(10, 25),
                new Civilian(12, 30),
            };

            List<Thief> JailedThieves = new List<Thief>();

            while (true)
            {
                // Clear console at the START of each frame
                Console.Clear();

                var collisions = ObjectMovementAndDrawing.MovementDrawing(drawing, list, rnd);

                Interact.StolenGoods(collisions);
                var caughtThisTurn = Interact.CaughtThieves(collisions);

                JailedThieves.AddRange(caughtThisTurn);

                foreach (var thief in caughtThisTurn)
                {
                    list.Remove(thief);
                    Console.WriteLine($"{thief.Name} has been sent to Gulag!");
                }

                foreach (Person person in list)
                {
                    Console.Write($"{person.Name} ({person.GetType().Name}): ");
                    person.Inventory.ShowItems();
                }

                // Clear buffered keystrokes
                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                Console.WriteLine("\n=== Press ANY key to continue ===");
                Console.ReadKey(true);
            }
        }
    }
}