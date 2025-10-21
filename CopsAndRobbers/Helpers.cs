namespace CopsAndRobbers
{
    internal class Helpers
    {

        public static string[,] DrawingClass()
        {
            string[,] drawingCreated = new string[10, 15];

            for (int row = 0; row < drawingCreated.GetLength(0); row++)
            {
                for (int col = 0; col < drawingCreated.GetLength(1); col++)
                {
                    if (row == 0 || row == drawingCreated.GetLength(0) - 1 || //top,mid,bot
                        col == 0 || col == drawingCreated.GetLength(1) - 1)
                    {
                        drawingCreated[row, col] = "#";

                    }
                    else
                    {
                        drawingCreated[row, col] = " ";
                    }

                    Console.Write(drawingCreated[row, col]);
                }
                Console.WriteLine();

            }
            return drawingCreated;
        }

        public static string[,] UpdateDrawing(string[,] drawingImport)  // den uppdaterade teckningen
        {
            Console.Clear();

            for (int row = 0; row < drawingImport.GetLength(0); row++)
            {
                for (int col = 0; col < drawingImport.GetLength(1); col++)  //rita upp allt
                {
                    Console.Write(drawingImport[row, col]); //rita ut den uppdaterade rad för rad , med den nya bokstaven

                }
                Console.WriteLine();

            }

            return drawingImport; //skicka tillbaka färdig ritning
        }
    }
}
