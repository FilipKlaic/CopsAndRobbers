namespace CopsAndRobbers
{
    internal class Place
    {
        public string Title { get; }
        public int Width { get; }
        public int Height { get; }

        public List<Person> Persons { get; set; }

        public Place(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            Persons = new List<Person>();
        }

        // Creates the drawing (similar to DrawingClass)
        public virtual void DrawingClass(char[,] drawingCreated, int startRow, int startCol)
        {
            //Console.Clear();

            // Top border with title
            string topLine = "======" + Title + "===";
            int remaining = Width - topLine.Length;
            if (remaining > 0)
                topLine += new string('=', remaining);

            for (int col = 0; col < Math.Min(topLine.Length, Width); col++)
            {
                drawingCreated[startRow, startCol + col] = topLine[col];
            }

            // Field body (with side borders)
            for (int row = 1; row < Height - 1; row++)
            {
                drawingCreated[startRow + row, startCol] = 'X'; // left border

                for (int col = 1; col < Width - 1; col++)
                    drawingCreated[startRow + row, startCol + col] = ' '; // inner field

                drawingCreated[startRow + row, startCol + Width - 1] = 'X'; // right border
            }

            // Bottom border
            for (int col = 0; col < Width; col++)
                drawingCreated[startRow + Height - 1, startCol + col] = '=';
        }

        // Draws the prision and city borders
        public static void DrawDrawing(char[,] drawingImport)
        {
            //Console.Clear();

            for (int row = 0; row < drawingImport.GetLength(0); row++)
            {
                for (int col = 0; col < drawingImport.GetLength(1); col++)
                {
                    Console.Write(drawingImport[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
