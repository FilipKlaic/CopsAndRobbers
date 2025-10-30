namespace CopsAndRobbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            // Create a "canvas" (2D array) to draw the places 
            char[,] canvas = new char[totalHeight, totalWidth];


            city.DrawingClass(canvas, 0, 0);                       // draw the City 
            prison.DrawingClass(canvas, city.Height + 0, 0);      // draw Prison with a 1-line gap

            Console.WriteLine();

            // Draw everything on the console
            Place.UpdateDrawing(canvas);

            //MoveCharactersRandomly(characters, city);

            // Draw characters with color on top of the field
            foreach (var p in characters)
            {
                // Ensure character stays inside City borders (avoid frame)
                if (p.X > 0 && p.X < city.Height - 1 && p.Y > 0 && p.Y < city.Width - 1)
                {
                    p.Draw(); // Use Charactercolor
                }
            }


            //// Leave one empty line after field
            //Console.SetCursorPosition(0, city.Height + prison.Height + 2);

            //// Print all characters info to console
            //Console.WriteLine("Characters and their inventories:");
            //foreach (var p in characters)
            //{
            //    //Console.WriteLine($"{p.Character} at ({p.X}, {p.Y}) ; Inventory: {string.Join(", ", p.Inventory.Items)}");

            //    //Console.WriteLine($"{p.Character} at ({p.X}, {p.Y})");
            //    p.ShowPersonsInfo();  // Display inventory
            //}

            //Console.WriteLine("\nPress any key to start simulation...");
            //Console.ReadKey();
            MoveCharactersRandomly(characters, city, prison, canvas);


        }

        // === Function to move all characters randomly ===
        static void MoveCharactersRandomly(List<Person> characters, City city, Prison prison, char[,] canvas)
        {
            Random rnd = new Random();

            // Total canvas offsets: city is drawn at startRow=0,startCol=0; prison at startRow = city.Height + 0
            int cityStartRow = 0;
            int cityStartCol = 0;
            int prisonStartRow = city.Height + 0;
            int prisonStartCol = 0;

            // Calculate safe area inside city (avoid title row and borders)
            int safeTop = cityStartRow + 2;             // skip title line and top border
            int safeBottom = cityStartRow + city.Height - 2; // skip bottom border
            int safeLeft = cityStartCol + 1;            // skip left border
            int safeRight = cityStartCol + city.Width - 2; // skip right border

            // Store previous positions so we can restore underlying canvas chars
            var previousPositions = new Dictionary<Person, (int X, int Y)>();
            foreach (var p in characters)
                previousPositions[p] = (p.X, p.Y);

            // Draw initial characters (on top of already drawn canvas)
            foreach (var p in characters)
            {
                if (p.X >= safeTop && p.X <= safeBottom && p.Y >= safeLeft && p.Y <= safeRight)
                {
                    Console.SetCursorPosition(p.Y, p.X);
                    Console.ForegroundColor = p.Charactercolor;
                    Console.Write(p.Character);
                    Console.ResetColor();
                }
            }

            // Main loop
            while (true)
            {
                foreach (var p in characters)
                {
                    // restore underlying symbol at previous position from canvas (avoid erasing frame)
                    var prev = previousPositions[p];
                    if (prev.X >= 0 && prev.X < canvas.GetLength(0) && prev.Y >= 0 && prev.Y < canvas.GetLength(1))
                    {
                        // restore the canvas char (this will put '=' or 'X' or ' ' back)
                        Console.SetCursorPosition(prev.Y, prev.X);
                        Console.Write(canvas[prev.X, prev.Y]);
                    }

                    // choose random step (-1, 0, 1 each)
                    int dx = rnd.Next(-1, 2);
                    int dy = rnd.Next(-1, 2);

                    int candidateX = p.X + dx;
                    int candidateY = p.Y + dy;

                    // clamp to safe area (so characters never go on borders)
                    candidateX = Math.Max(safeTop, Math.Min(candidateX, safeBottom));
                    candidateY = Math.Max(safeLeft, Math.Min(candidateY, safeRight));

                    // update model position
                    p.X = candidateX;
                    p.Y = candidateY;

                    // draw character in new position (color)
                    Console.SetCursorPosition(p.Y, p.X);
                    Console.ForegroundColor = p.Charactercolor;
                    Console.Write(p.Character);
                    Console.ResetColor();

                    // save previous position for next loop
                    previousPositions[p] = (p.X, p.Y);
                }

                // show logs below the field
                int logStartRow = city.Height + prison.Height + 2;
                Console.SetCursorPosition(0, logStartRow);

                // Clear previous logs
                for (int i = 0; i < characters.Count + 1; i++)
                {
                    Console.SetCursorPosition(0, logStartRow + i);
                    Console.Write(new string(' ', Console.WindowWidth));
                }

                Console.SetCursorPosition(0, logStartRow);
                Console.WriteLine("Characters and their positions:");
                foreach (var p in characters)
                {
                    p.ShowPersonsInfo();
                }

                // small pause so we can see movement
                Thread.Sleep(700);
            }
        }
}
}