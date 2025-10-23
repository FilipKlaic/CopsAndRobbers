namespace CopsAndRobbers
{
    internal class Rita
    {

        public static string[,] DrawingClassInitilize()
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


        //rensar gamla drawing mellan att karaktärerna flyttar sig.
        public static void WipeDrawing(string[,] drawingCreated)
        {
            for (int row = 0; row < drawingCreated.GetLength(0); row++)
            {
                for (int col = 0; col < drawingCreated.GetLength(1); col++)
                {
                    if (row == 0 || row == drawingCreated.GetLength(0) - 1 ||  // top and bottom borders
                        col == 0 || col == drawingCreated.GetLength(1) - 1)   // left and right borders
                    {
                        drawingCreated[row, col] = "#";  // border
                    }
                    else
                    {
                        drawingCreated[row, col] = " ";  // empty space
                    }
                }
            }
        }





        public static string[,] RedrawDrawing(string[,] drawingImport, string meeting)  // den uppdaterade teckningen
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

            if (!string.IsNullOrEmpty(meeting))
            {
                Console.WriteLine("\n--- Meetings this turn ---");

                bool hasThief = meeting.Contains("T");
                bool hasPolice = meeting.Contains("P");
                bool hasCivilian = meeting.Contains("C");

                // Thief and Police meet (any number)
                if (hasThief && hasPolice && !hasCivilian)
                {
                    Console.WriteLine("Thief(s) met Police! Stand down!.");
                }

                // Civilian and Thief meet
                if (hasCivilian && hasThief && !hasPolice)
                {
                    Console.WriteLine("Civilian(s) met Thief(s)! Uh oh!");
                }

                // Civilian and Police meet
                if (hasCivilian && hasPolice && !hasThief)
                {
                    Console.WriteLine("Civilian(s) met Police! Hopefully just a friendly chat.");
                }

                // All three meet
                if (hasCivilian && hasPolice && hasThief)
                {
                    Console.WriteLine("All three groups met! Chaos or justice?");
                }

                // Just one type
                if ((hasCivilian && !hasPolice && !hasThief) ||
                    (hasPolice && !hasCivilian && !hasThief) ||
                    (hasThief && !hasCivilian && !hasPolice))
                {
                    Console.WriteLine("Just a group of same-type people hanging out.");
                }
            }

            return drawingImport; //skicka tillbaka färdig ritning
        }
    }
}
