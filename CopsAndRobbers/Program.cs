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

            while (true)
            {
                ObjectMovementAndDrawing.MovementDrawing(drawing, list, rnd);
                Console.ReadKey();
            }
        }
    }
}