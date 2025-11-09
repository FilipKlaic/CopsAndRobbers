namespace CopsAndRobbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
   
            Random rnd = new Random();

            // Create two new objects: City and Prison
            City city = new City();
            Prison prison = new Prison();

            // Create characters without coordinates
            List<Person> characters = new List<Person>
            {
        new Civilian(0,0,"Jonsson"),
        new Civilian(0,0,"Svensson"),
        new Civilian(0,0,"Olofsson"),
        new Thief(0,0,"Silverstedt"),
        new Thief(0,0,"Brorson"),
        new Thief(0,0,"Olsson"),
        new Police(0,0,"Johansson"),
        new Police(0,0,"Karlsson"),
        new Police(0,0,"Nilsson"),
         new Civilian(0,0,"Jonsson2"),
        new Civilian(0,0,"Svensson2"),
        new Civilian(0,0,"Olofsson2"),
        new Thief(0,0,"Silverstedt2"),
        new Thief(0,0,"Brorson2"),
        new Thief(0,0,"Olsson2"),
        new Police(0,0,"Johansson2"),
        new Police(0,0,"Karlsson2"),
        new Police(0,0,"Nilsson2"),
        new Civilian(0,0,"Jonsson3"),
        new Civilian(0,0,"Svensson3"),
        new Civilian(0,0,"Olofsson3"),
        new Thief(0,0,"Silverstedt3"),
        new Thief(0,0,"Brorson3"),
        new Thief(0,0,"Olsson3"),
        new Police(0,0,"Johansson3"),
        new Police(0,0,"Karlsson3"),
        new Police(0,0,"Nilsson3")
    };
            // Assign them to places
            city.Persons = new List<Person>(characters); // all begin in city
            prison.Persons = new List<Person>();


            // Assign random positions inside the City
            foreach (var p in characters)
            {
                p.X = rnd.Next(1, city.Height - 1); // rows, avoid borders
                p.Y = rnd.Next(1, city.Width - 1);  // columns, avoid borders
            }

            // Determine total canvas size
            int totalHeight = city.Height + prison.Height + 1;   // calculate total screen size
            int totalWidth = Math.Max(city.Width, prison.Width); // use the wider place as width

            // Create a "canvas" (2D array) to draw the places 
            char[,] canvas = new char[totalHeight, totalWidth];


            city.DrawingClass(canvas, 0, 0);                       // draw the City 
            prison.DrawingClass(canvas, city.Height + 0, 0);      // draw Prison with a 1-line gap

            Console.WriteLine();

            // Draw everything on the console
            Place.UpdateDrawing(canvas);

            Helpers.MoveCharactersRandomly(characters, city, prison, canvas);
           
        }
    }
}