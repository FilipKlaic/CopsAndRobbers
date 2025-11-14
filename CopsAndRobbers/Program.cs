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
        new Civilian(5,5,"Jonsson"),
        new Civilian(5,5,"Svensson"),
        new Civilian(5,5,"Olofsson"),
        new Civilian(5,5,"Jonsson2"),
        new Civilian(5,5,"Svensson2"),
        new Civilian(5,5,"Olofsson2"),
        new Civilian(5,5,"Jonsson3"),
        new Civilian(5,5,"Sixtesson"),
        new Civilian(5,5,"Svedberg"),
        new Civilian(5,5,"Bergman"),
        new Civilian(5,5,"Bilsson"),
        new Civilian(5,5,"Nilsson3"),
        new Civilian(5,5,"Johanesson"),
        new Civilian(5,5,"Andersson"),
        new Civilian(5,5,"Sturesson"),
        new Thief(10,10,"Silverstedt2"),
        new Thief(10,10,"Brorson2"),
        new Thief(10,10,"Olsson2"),
        new Thief(10,10,"Silverstedt"),
        new Thief(10,10,"Brorson"),
        new Thief(10,10,"Olsson"),
        new Thief(10,10,"Silverstedt3"),
        new Thief(10,10,"Brorson3"),
        new Thief(10,10,"Olsson3"),
        new Police(20,20,"Johansson2"),
        new Police(20,20,"Karlsson2"),
        new Police(20,20,"Nilsson2"),
        new Police(20,20,"Johansson"),
        new Police(20,20,"Karlsson"),
        new Police(20,20,"Nilsson")
        
       
    };
            // Assign them to places
            city.Persons = new List<Person>(characters); // all begin in city
            prison.Persons = new List<Person>();


            // Assign random positions inside the City
            foreach (var p in characters)
            {
                p.X = rnd.Next(2, city.Height - 1); // rows, avoid borders
                p.Y = rnd.Next(2, city.Width - 1);  // columns, avoid borders
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