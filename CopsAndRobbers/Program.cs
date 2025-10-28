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

            // Draw characters with color on top of the field
            foreach (var p in characters)
            {
                // Ensure character stays inside City borders (avoid frame)
                if (p.X > 0 && p.X < city.Height - 1 && p.Y > 0 && p.Y < city.Width - 1)
                {
                    p.Draw(); // Use Charactercolor
                }
            }


            // Leave one empty line after field
            Console.SetCursorPosition(0, city.Height + prison.Height + 2);

            // Print all characters info to console
            Console.WriteLine("Characters and their inventories:");
            foreach (var p in characters)
            {
                //Console.WriteLine($"{p.Character} at ({p.X}, {p.Y}) ; Inventory: {string.Join(", ", p.Inventory.Items)}");

                //Console.WriteLine($"{p.Character} at ({p.X}, {p.Y})");
                p.ShowPersonsInfo();  // Display inventory
            }

            // Pause to keep console open
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        




        //        while (true)
        //        {
        //            // Create a 2D array of lists to track which characters are at each grid position
        //            // This allows multiple characters to occupy the same cell
        //            List<Person>[,] positions = new List<Person>[drawing.GetLength(0), drawing.GetLength(1)];

        //            // Initialize every cell with an empty list
        //            for (int i = 0; i < positions.GetLength(0); i++)
        //            {
        //                for (int j = 0; j < positions.GetLength(1); j++)
        //                {
        //                    positions[i, j] = new List<Person>();
        //                }
        //            }

        //            // Move each character randomly
        //            foreach (var p in list)
        //            {
        //                // Generate random movement: -1, 0, or 1 for both X and Y
        //                int dx = rnd.Next(-1, 2); // horizontal movement
        //                int dy = rnd.Next(-1, 2); // vertical movement

        //                // Calculate new position and clamp to stay inside borders
        //                // Math.Clamp prevents moving into border (row/col 0 or last row/col)
        //                int newX = Math.Clamp(p.X + dx, 1, drawing.GetLength(0) - 2);
        //                int newY = Math.Clamp(p.Y + dy, 1, drawing.GetLength(1) - 2);

        //                // Update character's position (overlapping is allowed)
        //                p.X = newX;
        //                p.Y = newY;

        //                // Add this character to the position tracker at their new location
        //                positions[p.X, p.Y].Add(p);
        //            }

        //            // Reset the entire grid to borders (#) and empty spaces ( )
        //            for (int row = 0; row < drawing.GetLength(0); row++)
        //            {
        //                for (int col = 0; col < drawing.GetLength(1); col++)
        //                {
        //                    // Check if current cell is on the border (top, bottom, left, or right edge)
        //                    if (row == 0 || row == drawing.GetLength(0) - 1 ||
        //                        col == 0 || col == drawing.GetLength(1) - 1)
        //                    {
        //                        drawing[row, col] = "#"; // Border
        //                    }
        //                    else
        //                    {
        //                        drawing[row, col] = " "; // Empty interior space
        //                    }
        //                }
        //            }

        //            // Draw characters onto the grid based on position tracker
        //            for (int row = 0; row < positions.GetLength(0); row++)
        //            {
        //                for (int col = 0; col < positions.GetLength(1); col++)
        //                {
        //                    // Check if any characters are at this position
        //                    if (positions[row, col].Count > 0)
        //                    {
        //                        if (positions[row, col].Count == 1)
        //                        {
        //                            // Only one character here: show their letter (C, T, or P)
        //                            drawing[row, col] = positions[row, col][0].Character;
        //                        }
        //                        else
        //                        {
        //                            // Multiple characters here: show the count as a number (2, 3, etc.)
        //                            drawing[row, col] = positions[row, col].Count.ToString();
        //                        }
        //                    }
        //                    // If Count is 0, the cell remains empty (" ") from the reset step
        //                }
        //            }

        //            // Clear the console and display the updated grid
        //            Helpers.UpdateDrawing(drawing);

        //            // Wait for user to press any key before next frame (manual step-through)
        //            Console.ReadKey();
        //        }
    }
}
}