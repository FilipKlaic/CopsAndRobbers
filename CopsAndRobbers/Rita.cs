namespace CopsAndRobbers
{
    internal class Rita
    {
        public static string[,] DrawingClassInitilize()
        {
            // Match Place dimensions: 25 rows (height) × 100 columns (width)
            string[,] drawingCreated = new string[25, 100];

            // Top border with title
            string title = "RobeCity";
            string topLine = "======" + title + "===";
            int remaining = drawingCreated.GetLength(1) - topLine.Length;
            if (remaining > 0)
                topLine += new string('=', remaining);

            // Draw top border
            for (int col = 0; col < Math.Min(topLine.Length, drawingCreated.GetLength(1)); col++)
            {
                drawingCreated[0, col] = topLine[col].ToString();
                Console.Write(drawingCreated[0, col]);
            }
            Console.WriteLine();

            // Draw field body with side borders
            for (int row = 1; row < drawingCreated.GetLength(0) - 1; row++)
            {
                drawingCreated[row, 0] = "X"; // left border
                Console.Write(drawingCreated[row, 0]);

                for (int col = 1; col < drawingCreated.GetLength(1) - 1; col++)
                {
                    drawingCreated[row, col] = " "; // inner field
                    Console.Write(drawingCreated[row, col]);
                }

                drawingCreated[row, drawingCreated.GetLength(1) - 1] = "X"; // right border
                Console.Write(drawingCreated[row, drawingCreated.GetLength(1) - 1]);
                Console.WriteLine();
            }

            // Draw bottom border
            for (int col = 0; col < drawingCreated.GetLength(1); col++)
            {
                drawingCreated[drawingCreated.GetLength(0) - 1, col] = "=";
                Console.Write(drawingCreated[drawingCreated.GetLength(0) - 1, col]);
            }
            Console.WriteLine();

            return drawingCreated;
        }

        // Wipes the drawing between character movements
        public static void WipeDrawing(string[,] drawingCreated)
        {
            // Top border - restore title
            string title = "Cops%Robbers";
            string topLine = "======" + title + "===";
            int remaining = drawingCreated.GetLength(1) - topLine.Length;
            if (remaining > 0)
                topLine += new string('=', remaining);

            for (int col = 0; col < Math.Min(topLine.Length, drawingCreated.GetLength(1)); col++)
            {
                drawingCreated[0, col] = topLine[col].ToString();
            }

            // Middle rows - restore borders and clear interior
            for (int row = 1; row < drawingCreated.GetLength(0) - 1; row++)
            {
                drawingCreated[row, 0] = "X"; // left border

                for (int col = 1; col < drawingCreated.GetLength(1) - 1; col++)
                {
                    drawingCreated[row, col] = " "; // empty space
                }

                drawingCreated[row, drawingCreated.GetLength(1) - 1] = "X"; // right border
            }

            // Bottom border
            for (int col = 0; col < drawingCreated.GetLength(1); col++)
            {
                drawingCreated[drawingCreated.GetLength(0) - 1, col] = "=";
            }
        }

        public static string[,] RedrawDrawing(string[,] drawingImport, List<List<Person>> collisions)
        {
            // Remove Console.Clear() from here - it's now in Program.cs

            for (int row = 0; row < drawingImport.GetLength(0); row++)
            {
                for (int col = 0; col < drawingImport.GetLength(1); col++)
                {
                    Console.Write(drawingImport[row, col]);
                }
                Console.WriteLine();
            }

            // Check for collisions
            if (collisions == null || collisions.Count == 0)
                return drawingImport;

            foreach (var populatedIndex in collisions)
            {
                if (populatedIndex == null || populatedIndex.Count == 0)
                    continue;

                // Check who is present in this cell
                bool hasPolice = populatedIndex.Any(p => p is Police);
                bool hasThief = populatedIndex.Any(t => t is Thief);
                bool hasCivilian = populatedIndex.Any(c => c is Civilian);

                // Collect names by type
                string policeNames = string.Join(", ", populatedIndex.OfType<Police>().Select(p => p.Name));
                string thiefNames = string.Join(", ", populatedIndex.OfType<Thief>().Select(t => t.Name));
                string civNames = string.Join(", ", populatedIndex.OfType<Civilian>().Select(c => c.Name));

                // --- Meeting Logic ---
                if (hasPolice && hasThief && hasCivilian)
                {
                    Console.WriteLine($"{policeNames}, {thiefNames} and {civNames} Chaos is here!");
                }

                // Police vs Thief 
                if (hasPolice && hasThief)
                {
                    Console.WriteLine($"{policeNames} met {thiefNames}!");
                }
                // Civilian vs Thief
                else if (hasCivilian && hasThief)
                {
                    Console.WriteLine($"{civNames} met {thiefNames}! Uh oh!");
                }
                // Civilian vs Police (no Thief)
                else if (hasPolice && hasCivilian)
                {
                    Console.WriteLine($"{civNames} met {policeNames}! Hopefully just a friendly chat.");
                }
                // Thief vs Thief
                else if (populatedIndex.Count(t => t is Thief) > 1)
                {
                    Console.WriteLine($"{thiefNames} met each other — plotting mischief!");
                }
                // Police vs Police
                else if (populatedIndex.Count(p => p is Police) > 1)
                {
                    Console.WriteLine($"{policeNames} met each other — team coordination!");
                }
                // Civilian vs Civilian
                else if (populatedIndex.Count(c => c is Civilian) > 1)
                {
                    Console.WriteLine($"{civNames} met for a friendly chat.");
                }
            }

            return drawingImport;
        }
    }
}