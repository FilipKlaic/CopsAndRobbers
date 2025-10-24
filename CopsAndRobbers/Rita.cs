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





        public static string[,] RedrawDrawing(string[,] drawingImport, List<Person> populatedIndex)  // den uppdaterade teckningen
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
            //p går igenom listan if p == police true
            bool hasPolice = populatedIndex.Any(p => p is Police);
            bool hasThief = populatedIndex.Any(p => p is Thief);
            bool hasCivilian = populatedIndex.Any(p => p is Civilian);

            // Police vs Thief
            if (hasPolice && hasThief)
            {
                var policeNames = string.Join(", ", populatedIndex.Where(p => p is Police).Select(p => p.Name));
                var thiefNames = string.Join(", ", populatedIndex.Where(p => p is Thief).Select(p => p.Name));
                Console.WriteLine($"{policeNames} met {thiefNames}! Justice served!");
            }
            // Civilian vs Thief (only if no Police)
            else if (!hasPolice && hasCivilian && hasThief)
            {
                var civNames = string.Join(", ", populatedIndex.Where(p => p is Civilian).Select(p => p.Name));
                var thiefNames = string.Join(", ", populatedIndex.Where(p => p is Thief).Select(p => p.Name));
                Console.WriteLine($"{civNames} met {thiefNames}! Uh oh!");
            }
            // Civilian vs Police (only if no Thief)
            else if (hasPolice && hasCivilian && !hasThief)
            {
                var civNames = string.Join(", ", populatedIndex.Where(p => p is Civilian).Select(p => p.Name));
                var policeNames = string.Join(", ", populatedIndex.Where(p => p is Police).Select(p => p.Name));
                Console.WriteLine($"{civNames} met {policeNames}! Hopefully just a friendly chat.");
            }
            // Thief vs Thief
            else if (populatedIndex.Count(p => p is Thief) > 1)
            {
                var thiefNames = string.Join(" and ", populatedIndex.Where(p => p is Thief).Select(p => p.Name));
                Console.WriteLine($"{thiefNames} met each other — plotting mischief!");
            }
            // Police vs Police
            else if (populatedIndex.Count(p => p is Police) > 1)
            {
                var policeNames = string.Join(" and ", populatedIndex.Where(p => p is Police).Select(p => p.Name));
                Console.WriteLine($"{policeNames} met each other — team coordination!");
            }
            // Civilian vs Civilian
            else if (populatedIndex.Count(p => p is Civilian) > 1)
            {
                var civNames = string.Join(" and ", populatedIndex.Where(p => p is Civilian).Select(p => p.Name));
                Console.WriteLine($"{civNames} met for a friendly chat.");
            }

            return drawingImport; //skicka tillbaka färdig ritning
        }
    }
}
