namespace CopsAndRobbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[,] drawing = Helpers.DrawingClass();
            
            Random rnd = new Random();

            List<Person> list = new List<Person>
            {
                new Civilian(5, 5),
                new Thief(5, 5),
                new Police(5, 5)
            };

            while (true)
            {
                // Create a 2D array of lists to track which characters are at each grid position
                // This allows multiple characters to occupy the same cell
                List<Person>[,] positions = new List<Person>[drawing.GetLength(0), drawing.GetLength(1)];
                
                // Initialize every cell with an empty list
                for (int i = 0; i < positions.GetLength(0); i++)
                {
                    for (int j = 0; j < positions.GetLength(1); j++)
                    {
                        positions[i, j] = new List<Person>();
                    }
                }

                // Move each character randomly
                foreach (var p in list)
                {
                    // Generate random movement: -1, 0, or 1 for both X and Y
                    int dx = rnd.Next(-1, 2); // horizontal movement
                    int dy = rnd.Next(-1, 2); // vertical movement
                    
                    // Calculate new position and clamp to stay inside borders
                    // Math.Clamp prevents moving into border (row/col 0 or last row/col)
                    int newX = Math.Clamp(p.X + dx, 1, drawing.GetLength(0) - 2);
                    int newY = Math.Clamp(p.Y + dy, 1, drawing.GetLength(1) - 2);

                    // Update character's position (overlapping is allowed)
                    p.X = newX;
                    p.Y = newY;

                    // Add this character to the position tracker at their new location
                    positions[p.X, p.Y].Add(p);
                }

                // Reset the entire grid to borders (#) and empty spaces ( )
                for (int row = 0; row < drawing.GetLength(0); row++)
                {
                    for (int col = 0; col < drawing.GetLength(1); col++)
                    {
                        // Check if current cell is on the border (top, bottom, left, or right edge)
                        if (row == 0 || row == drawing.GetLength(0) - 1 ||
                            col == 0 || col == drawing.GetLength(1) - 1)
                        {
                            drawing[row, col] = "#"; // Border
                        }
                        else
                        {
                            drawing[row, col] = " "; // Empty interior space
                        }
                    }
                }

                // Draw characters onto the grid based on position tracker
                for (int row = 0; row < positions.GetLength(0); row++)
                {
                    for (int col = 0; col < positions.GetLength(1); col++)
                    {
                        // Check if any characters are at this position
                        if (positions[row, col].Count > 0)
                        {
                            if (positions[row, col].Count == 1)
                            {
                                // Only one character here: show their letter (C, T, or P)
                                drawing[row, col] = positions[row, col][0].Character;
                            }
                            else
                            {
                                // Multiple characters here: show the count as a number (2, 3, etc.)
                                drawing[row, col] = positions[row, col].Count.ToString();
                            }
                        }
                        // If Count is 0, the cell remains empty (" ") from the reset step
                    }
                }

                // Clear the console and display the updated grid
                Helpers.UpdateDrawing(drawing);

                // Wait for user to press any key before next frame (manual step-through)
                Console.ReadKey();
            }
        }
    }
}