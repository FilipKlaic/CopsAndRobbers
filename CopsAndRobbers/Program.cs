namespace CopsAndRobbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set console window size to accommodate the game
            try
            {
                Console.SetWindowSize(120, 50);  // width, height
                Console.SetBufferSize(120, 50);
            }
            catch (Exception)
            {
                // If we can't resize, continue with current size
                Console.WriteLine("Could not resize console window. Game may not display properly.");
                Console.WriteLine("Please maximize your console window.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            Random rnd = new Random();

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
                new Police(0,0,"Nilsson")
            };

            // Create two new objects: City and Prison
            City city = new City();
            Prison prison = new Prison();

            // Assign random positions inside the City
            foreach (var p in characters)
            {
                p.X = rnd.Next(1, city.Height - 1); // rows, avoid borders
                p.Y = rnd.Next(1, city.Width - 1);  // columns, avoid borders
            }

            // Determine total canvas size
            int totalHeight = city.Height + prison.Height + 1;   // calculate total screen size
            int totalWidth = Math.Max(city.Width, prison.Width); // use the wider place as width

            // Check if the canvas fits in the console
            if (totalHeight > Console.WindowHeight - 5 || totalWidth > Console.WindowWidth)
            {
                Console.WriteLine($"Console window too small! Need at least {totalWidth}x{totalHeight + 5}");
                Console.WriteLine($"Current size: {Console.WindowWidth}x{Console.WindowHeight}");
                Console.WriteLine("Please resize your console window and restart the program.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            // Create a "canvas" (2D array) to draw the places 
            char[,] canvas = new char[totalHeight, totalWidth];

            city.DrawingClass(canvas, 0, 0);                       // draw the City 
            prison.DrawingClass(canvas, city.Height + 1, 0);      // draw Prison with a 1-line gap

            Console.Clear();
            Console.WriteLine();

            // Draw everything on the console
            Place.UpdateDrawing(canvas);

            // Draw characters with color on top of the field
            foreach (var p in characters)
            {
                // Ensure character stays inside City borders (avoid frame)
                if (p.X > 0 && p.X < city.Height - 1 && p.Y > 0 && p.Y < city.Width - 1)
                {
                    p.Draw(); // Use Charactercolor
                }
            }

            // Safe cursor positioning for character info
            int infoStartY = Math.Min(city.Height + prison.Height + 3, Console.WindowHeight - 12);
            Console.SetCursorPosition(0, infoStartY);

            // Print all characters info to console
            Console.WriteLine("Characters and their inventories:");
            foreach (var p in characters)
            {
                p.ShowPersonsInfo();  // Display inventory
            }

            // Start the game loop
            Console.WriteLine("\nPress any key to start the game...");
            Console.ReadKey();

            while (true)
            {
                // Move each character randomly
                foreach (var p in characters)
                {
                    int dx = rnd.Next(-1, 2);
                    int dy = rnd.Next(-1, 2);

                    int newX = Math.Clamp(p.X + dx, 1, city.Height - 2);
                    int newY = Math.Clamp(p.Y + dy, 1, city.Width - 2);

                    p.X = newX;
                    p.Y = newY;
                }

                // Handle thief-civilian interactions
                HandleInteractions(characters, rnd);

                // Clear and redraw the canvas
                canvas = new char[totalHeight, totalWidth];
                city.DrawingClass(canvas, 0, 0);
                prison.DrawingClass(canvas, city.Height + 1, 0);

                Console.Clear();
                Place.UpdateDrawing(canvas);

                // Draw characters with color - with safe bounds checking
                foreach (var p in characters)
                {
                    if (p.X > 0 && p.X < city.Height - 1 && p.Y > 0 && p.Y < city.Width - 1)
                    {
                        p.Draw();
                    }
                }

                // Display updated character info with safe positioning
                try
                {
                    int safeInfoY = Math.Min(city.Height + prison.Height + 3, Console.WindowHeight - 12);
                    Console.SetCursorPosition(0, safeInfoY);
                    Console.WriteLine("Characters and their inventories:");
                    foreach (var p in characters)
                    {
                        p.ShowPersonsInfo();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Fallback: just print without positioning
                    Console.WriteLine("\nCharacters and their inventories:");
                    foreach (var p in characters)
                    {
                        p.ShowPersonsInfo();
                    }
                }

                Console.WriteLine("\nPress any key for next turn (or Ctrl+C to exit)...");
                Console.ReadKey();
            }
        }

        static List<Person> GetCharactersAtPosition(List<Person> characters, int x, int y)
        {
            return characters.Where(p => p.X == x && p.Y == y).ToList();
        }

        static void HandleInteractions(List<Person> characters, Random rnd)
        {
            var thieves = characters.OfType<Thief>().ToList();

            foreach (var thief in thieves)
            {
                // Check for civilians at the same position
                var nearbyCharacters = GetCharactersAtPosition(characters, thief.X, thief.Y);
                var civiliansAtSameSpot = nearbyCharacters.OfType<Civilian>().ToList();

                if (civiliansAtSameSpot.Any())
                {
                    // If there are multiple civilians, pick one randomly
                    var targetCivilian = civiliansAtSameSpot[rnd.Next(civiliansAtSameSpot.Count)];
                    thief.StealFrom(targetCivilian, rnd);
                }
            }
        }
    }
}