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





        public static string[,] RedrawDrawing(string[,] drawingImport, List<List<Person>> collisions)  // den uppdaterade teckningen
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

            //check for collsions again
            if (collisions == null || collisions.Count == 0)
                return drawingImport;

            foreach (var populatedIndex in collisions)    //kikar om det finns flera platser som det krockas på
            {

                //failsafe
                if (populatedIndex == null || populatedIndex.Count == 0)
                    continue;

                // Check who is present in this cell
                bool hasPolice = populatedIndex.Any(p => p is Police);          //charachter present     = true / false
                bool hasThief = populatedIndex.Any(t => t is Thief);
                bool hasCivilian = populatedIndex.Any(c => c is Civilian);

                // Collect names by type (using your preferred variable names)
                string policeNames = string.Join(", ", populatedIndex.OfType<Police>().Select(p => p.Name));  //name saved to string
                string thiefNames = string.Join(", ", populatedIndex.OfType<Thief>().Select(t => t.Name));
                string civNames = string.Join(", ", populatedIndex.OfType<Civilian>().Select(c => c.Name));


                // --- Meeting Logic ---
                if (hasPolice && hasThief && hasCivilian)
                {

                    Console.WriteLine($"{policeNames} , {thiefNames} and {civNames} Chaos is here!");

                }

                // Police vs Thief 
                if (hasPolice && hasThief)
                {
                    Console.WriteLine($"{policeNames} met {thiefNames}! Justice served!");


                    var caughtThieves = populatedIndex.OfType<Thief>().ToList();       // denna för att lägga ihop dom i samma string
                    foreach (var thief in caughtThieves)
                    {
                        Console.WriteLine($"{thief.Name} has been arrested and removed from the game!");

                    }

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
                else if (populatedIndex.Count(t => t is Thief) > 1)  // if more than 1 thief is present
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
            return drawingImport; // Ensure a return statement for all code paths
        }
    }
}

