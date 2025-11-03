namespace CopsAndRobbers
{
    internal class Helpers
    {
        private static Queue<string> logs = new Queue<string>();


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
            int citySafeTop = cityStartRow + 2;             // skip title line and top border
            int citySafeBottom = cityStartRow + city.Height - 2; // skip bottom border
            int citySafeLeft = cityStartCol + 1;            // skip left border
            int citySafeRight = cityStartCol + city.Width - 2; // skip right border

            // Calculate safe area inside prison
            int prisonSafeTop = prisonStartRow + 2;             // skip title line and top border
            int prisonSafeBottom = prisonStartRow + prison.Height - 2; // skip bottom border
            int prisonSafeLeft = prisonStartCol + 1;            // skip left border
            int prisonSafeRight = prisonStartCol + prison.Width - 2; // skip right border

            // Store previous positions so we can restore underlying canvas chars
            var previousPositions = new Dictionary<Person, (int X, int Y)>();
            foreach (var p in characters)
                previousPositions[p] = (p.X, p.Y);

            // Draw initial characters (on top of already drawn canvas)
            foreach (var p in characters)
            {
                DrawCharacterAtPosition(p, citySafeTop, citySafeBottom, citySafeLeft, citySafeRight,
                                      prisonSafeTop, prisonSafeBottom, prisonSafeLeft, prisonSafeRight);
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

                    // Move character randomly within their current location (city or prison)
                    MoveCharacter(p, rnd, citySafeTop, citySafeBottom, citySafeLeft, citySafeRight,
                                 prisonSafeTop, prisonSafeBottom, prisonSafeLeft, prisonSafeRight);

                    // draw character in new position (color)
                    DrawCharacterAtPosition(p, citySafeTop, citySafeBottom, citySafeLeft, citySafeRight,
                                          prisonSafeTop, prisonSafeBottom, prisonSafeLeft, prisonSafeRight);

                    // save previous position for next loop
                    previousPositions[p] = (p.X, p.Y);
                }

                HandleCollisions(characters, city, prison, rnd);

                // small pause so we can see movement
                Thread.Sleep(400);
            }
        }

        private static void DrawCharacterAtPosition(Person p, int citySafeTop, int citySafeBottom, int citySafeLeft, int citySafeRight,
                                                   int prisonSafeTop, int prisonSafeBottom, int prisonSafeLeft, int prisonSafeRight)
        {
            bool inCity = p.X >= citySafeTop && p.X <= citySafeBottom && p.Y >= citySafeLeft && p.Y <= citySafeRight;
            bool inPrison = p.X >= prisonSafeTop && p.X <= prisonSafeBottom && p.Y >= prisonSafeLeft && p.Y <= prisonSafeRight;

            if (inCity || inPrison)
            {
                Console.SetCursorPosition(p.Y, p.X);
                Console.ForegroundColor = p.Charactercolor;

                // Show imprisoned thieves with different character
                if (p.IsImprisoned && p.RoleName == "Thief")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("I"); // 'I' for Imprisoned
                }
                else
                {
                    Console.Write(p.Character);
                }
                Console.ResetColor();
            }
        }

        private static void MoveCharacter(Person p, Random rnd, int citySafeTop, int citySafeBottom, int citySafeLeft, int citySafeRight,
                                        int prisonSafeTop, int prisonSafeBottom, int prisonSafeLeft, int prisonSafeRight)
        {
            // choose random step (-1, 0, 1 each)
            int dx = rnd.Next(-1, 2);
            int dy = rnd.Next(-1, 2);

            int candidateX = p.X + dx;
            int candidateY = p.Y + dy;

            // Determine movement bounds based on imprisonment status
            if (p.IsImprisoned)
            {
                // Imprisoned characters can only move within prison
                candidateX = Math.Max(prisonSafeTop, Math.Min(candidateX, prisonSafeBottom));
                candidateY = Math.Max(prisonSafeLeft, Math.Min(candidateY, prisonSafeRight));
            }
            else
            {
                // Free characters can only move within city
                candidateX = Math.Max(citySafeTop, Math.Min(candidateX, citySafeBottom));
                candidateY = Math.Max(citySafeLeft, Math.Min(candidateY, citySafeRight));
            }

            // update model position
            p.X = candidateX;
            p.Y = candidateY;
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

                    // Skip interactions involving imprisoned characters (except within prison)
                    if (p1.IsImprisoned && p2.IsImprisoned)
                    {
                        // Imprisoned characters can interact with each other
                        DrawLog($"Imprisoned {p1.Name} meets imprisoned {p2.Name} in prison", city, prison);
                        Thread.Sleep(1000);
                        continue;
                    }
                    else if (p1.IsImprisoned || p2.IsImprisoned)
                    {
                        // Skip interactions between imprisoned and free characters
                        continue;
                    }

                    if ((p1.RoleName == "Thief" && p2.RoleName == "Civilian") ||
                        (p1.RoleName == "Civilian" && p2.RoleName == "Thief"))
                    {
                        var thief = p1.RoleName == "Thief" ? p1 as Thief : p2 as Thief;
                        var civilian = p1.RoleName == "Civilian" ? p1 as Civilian : p2 as Civilian;

                        DrawLog($"Thief {thief.Name} meets Civilian {civilian.Name}", city, prison);
                        thief?.StealFrom(civilian, rnd, city, prison);
                        Thread.Sleep(1000);
                    }

                    else if ((p1.RoleName == "Thief" && p2.RoleName == "Police officer") ||
                             (p1.RoleName == "Police officer" && p2.RoleName == "Thief"))
                    {
                        var thief = p1.RoleName == "Thief" ? (Thief)p1 : (Thief)p2;
                        var police = p1.RoleName == "Police officer" ? (Police)p1 : (Police)p2;

                        DrawLog($"Police {police.Name} encounters Thief {thief.Name}!", city, prison);

                        // Attempt to arrest the thief
                        police.ArrestThief(thief, rnd, city, prison);

                        Thread.Sleep(1000);
                    }

                    else if (p1.RoleName == "Civilian" && p2.RoleName == "Civilian")
                    {
                        // Nothing happens
                        DrawLog($"Civilian {p1.Name} meets Civilian {p2.Name}", city, prison);
                        Thread.Sleep(1000);
                    }

                    else if ((p1.RoleName == "Police officer" && p2.RoleName == "Civilian") ||
                             (p1.RoleName == "Civilian" && p2.RoleName == "Police officer"))
                    {
                        var police = p1.RoleName == "Police officer" ? (Police)p1 : (Police)p2;
                        var civilian = p1.RoleName == "Civilian" ? (Civilian)p1 : (Civilian)p2;
                        // Nothing happens

                        DrawLog($"Police {police.Name} greets Civilian {civilian.Name}", city, prison);
                        Thread.Sleep(1000);
                    }

                    else if (p1.RoleName == "Police officer" && p2.RoleName == "Police officer")
                    {
                        // Nothing happens

                        DrawLog($"Police {p1.Name} meets Police {p2.Name}", city, prison);
                        Thread.Sleep(1000);
                    }

                    else
                    {
                        // Could log or ignore
                    }
                }
            }
        }

        // Draws messages in the bottom area of the console
        internal static void DrawLog(string message, City city, Prison prison)
        {
            int logStartRow = city.Height + prison.Height + 2;

            // Add the new message to the queue
            logs.Enqueue(message);

            // Limit the queue size to 10 lines (remove the oldest if too many)
            if (logs.Count > 5)
                logs.Dequeue();

            // Redraw all log lines
            int i = 0;
            foreach (var log in logs)
            {
                Console.SetCursorPosition(0, logStartRow + i);
                Console.Write(new string(' ', Console.WindowWidth)); // clear line
                Console.SetCursorPosition(0, logStartRow + i);
                Console.WriteLine(log);
                i++;
            }
        }
    }
}
