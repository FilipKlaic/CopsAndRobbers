namespace CopsAndRobbers
{
    internal class Helpers
    {

        // Function to move all characters randomly
     internal static void MoveCharactersRandomly(List<Person> characters, City city, Prison prison, char[,] canvas)
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

                HandleCollisions(characters, city, prison, rnd);

                //Console.SetCursorPosition(0, logStartRow);
                //Console.WriteLine("Characters and their positions:");
                //foreach (var p in characters)
                //{
                //    p.ShowPersonsInfo();
                //}

                // small pause so we can see movement
                Thread.Sleep(400);
            }
            }

        public static void HandleCollisions(List<Person> characters, City city, Prison prison, Random rnd)
        {
            int startLogRow = city.Height + prison.Height + 2;
            // Iterate through all pairs of characters
            for (int i = 0; i < characters.Count; i++)
            {
                for (int j = i + 1; j < characters.Count; j++)
                {
                    var p1 = characters[i];
                    var p2 = characters[j];

                    // Skip if characters are not on the same position
                    if (p1.X != p2.X || p1.Y != p2.Y)
                        continue;

                    if ((p1.RoleName == "Thief" && p2.RoleName == "Civilian") ||
                        (p1.RoleName == "Civilian" && p2.RoleName == "Thief"))
                    {
                        var thief = p1.RoleName == "Thief" ? p1 as Thief : p2 as Thief;
                        var civilian = p1.RoleName == "Civilian" ? p1 as Civilian : p2 as Civilian;

                        thief?.StealFrom(civilian, rnd);
                        //Console.SetCursorPosition(0, startLogRow);
                        //Console.WriteLine("Thief meets Civilian");
                        Thread.Sleep(1000);
                    }
                
                    else if ((p1.RoleName == "Thief" && p2.RoleName == "Police officer") ||
                             (p1.RoleName == "Police officer" && p2.RoleName == "Thief"))
                    {
                        // Something happens
                        Console.SetCursorPosition(0, startLogRow);
                        Console.WriteLine("Thief meets Police");
                        Thread.Sleep(1000);
                    }
                    // Civilian meets Civilian
                    else if (p1.RoleName == "Civilian" && p2.RoleName == "Civilian")
                    {
                        // Nothing happens
                        Console.SetCursorPosition(0, startLogRow);
                        Console.WriteLine("Civilian meets Civilian");
                        Thread.Sleep(1000);
                    }
                  
                    else if ((p1.RoleName == "Police officer" && p2.RoleName == "Civilian") ||
                             (p1.RoleName == "Civilian" && p2.RoleName == "Police officer"))
                    {
                        // Something happens
                        Console.SetCursorPosition(0, startLogRow);
                        Console.WriteLine("Police meets Civilian");
                        Thread.Sleep(1000);
                    }
                    
                    else if (p1.RoleName == "Police officer" && p2.RoleName == "Police officer")
                    {
                        // Nothing happens
                        Console.SetCursorPosition(0, startLogRow);
                        Console.WriteLine("Police meets Police");
                        Thread.Sleep(1000);
                    }
                   
                    else
                    {
                        // Could log or ignore
                    }
                }
            }
        }
    }
}
