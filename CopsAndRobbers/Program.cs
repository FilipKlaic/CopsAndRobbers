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
                new Police(5, 5)
            };

            List<Thief> JailedThieves = new List<Thief>();


            while (true)
            {

                var collisions = ObjectMovementAndDrawing.MovementDrawing(drawing, list, rnd);


                var caughtThisTurn = Interact.CaughtThieves(collisions);


                JailedThieves.AddRange(caughtThisTurn);  //fängslade tjuvar

                foreach (var thief in caughtThisTurn)
                {
                    list.Remove(thief);
                    Console.WriteLine($"{thief.Name} has been sent to Gulag!");

                }

                Console.ReadKey();

            }
        }
    }
}