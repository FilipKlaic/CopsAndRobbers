namespace CopsAndRobbers
{
    internal class Helpers
    {
        // Queue to store log messages for display at bottom of console
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
            {
                previousPositions[p] = (p.X, p.Y);

            }


            // Main loop
            while (true)
            {
                // Erase all characters at their old positions
                foreach (var p in characters)
                {
                    // Wipe canvas
                    var prev = previousPositions[p];
                    if (prev.X >= 0 && prev.X < canvas.GetLength(0) && prev.Y >= 0 && prev.Y < canvas.GetLength(1))
                    {
                        // Restore the canvas char (this will put '=' or 'X' or ' ' back)
                        Console.SetCursorPosition(prev.Y, prev.X);
                        Console.Write(canvas[prev.X, prev.Y]);
                    }
                }

                //  Move all characters to new positions
                foreach (var p in characters)
                {
                    // Move character randomly within their current location (city or prison)
                    MoveCharacter(p, rnd, citySafeTop, citySafeBottom, citySafeLeft, citySafeRight,
                                 prisonSafeTop, prisonSafeBottom, prisonSafeLeft, prisonSafeRight);

                    // Save previous position for next loop iteration (forMousePos)
                    previousPositions[p] = (p.X, p.Y);
                }

                //Detect collision positions before drawing
                var collisionPositions = GetCollisionPositions(characters);

                //  Draw characters
                foreach (var p in characters)
                {
                    // Only draw if NOT in a collision ( X )
                    if (!collisionPositions.Any(cPos => cPos.X == p.X && cPos.Y == p.Y))
                    {
                        DrawCharacterAtPosition(p, citySafeTop, citySafeBottom, citySafeLeft, citySafeRight,
                                              prisonSafeTop, prisonSafeBottom, prisonSafeLeft, prisonSafeRight);
                    }
                }
                // pass collision positions
                HandleCollisions(characters, collisionPositions, city, prison, rnd);



            }
        }

        // Helper method to move a single character randomly within bounds
        private static void MoveCharacter(Person p, Random rnd, int citySafeTop, int citySafeBottom, int citySafeLeft, int citySafeRight,
                                        int prisonSafeTop, int prisonSafeBottom, int prisonSafeLeft, int prisonSafeRight)
        {
            // Choose random step (-1, 0, 1 each) for X and Y
            int dx = rnd.Next(-1, 2);
            int dy = rnd.Next(-1, 2);

            int candidateX = p.X + dx;
            int candidateY = p.Y + dy;

            // Determine movement bounds based on imprisonment status
            if (p.IsImprisoned)
            {
                // Imprisoned characters can only move within prison bounds
                candidateX = Math.Max(prisonSafeTop, Math.Min(candidateX, prisonSafeBottom));
                candidateY = Math.Max(prisonSafeLeft, Math.Min(candidateY, prisonSafeRight));
            }
            else
            {
                // Free characters can only move within city bounds
                candidateX = Math.Max(citySafeTop, Math.Min(candidateX, citySafeBottom));
                candidateY = Math.Max(citySafeLeft, Math.Min(candidateY, citySafeRight));
            }

            // Update model position
            p.X = candidateX;
            p.Y = candidateY;
        }


        // Create a list of Colliding indexes
        private static List<(int X, int Y)> GetCollisionPositions(List<Person> characters)
        {

            var positionCounts = new Dictionary<(int X, int Y), int>();

            // Count characters at each position
            foreach (var p in characters)
            {
                var pos = (p.X, p.Y);

                //if pos1 == pos2   add count
                if (positionCounts.ContainsKey(pos))
                {
                    positionCounts[pos]++;
                }
                else
                {
                    positionCounts[pos] = 1;
                }
            }

            // Create a list to store collision positions
            var collisionPositions = new List<(int X, int Y)>();

            foreach (var anomaly in positionCounts)
            {
                if (anomaly.Value > 1)  // More than 1 character at this position
                {
                    collisionPositions.Add(anomaly.Key);
                }
            }


            //  Write out number of people at pos
            foreach (var pos in collisionPositions)
            {
                Console.SetCursorPosition(pos.Y, pos.X);
                Console.ForegroundColor = ConsoleColor.Yellow;

                // Get the count from the dictionary
                int occupiedPeople = positionCounts[pos];

                Console.Write(occupiedPeople.ToString());
                Console.ResetColor();
            }

            // Return the list of collision positions for HandleCollisions
            return collisionPositions;
        }



        // Helper method to draw a character at their position (with bounds checking)
        private static void DrawCharacterAtPosition(Person p, int citySafeTop, int citySafeBottom, int citySafeLeft, int citySafeRight,
                                                   int prisonSafeTop, int prisonSafeBottom, int prisonSafeLeft, int prisonSafeRight)
        {
            // Check if character is within city bounds
            bool inCity = p.X >= citySafeTop && p.X <= citySafeBottom && p.Y >= citySafeLeft && p.Y <= citySafeRight;
            // Check if character is within prison bounds
            bool inPrison = p.X >= prisonSafeTop && p.X <= prisonSafeBottom && p.Y >= prisonSafeLeft && p.Y <= prisonSafeRight;

            // Only draw if character is within valid bounds
            if (inCity || inPrison)
            {
                Console.SetCursorPosition(p.Y, p.X);
                Console.ForegroundColor = p.Charactercolor;

                // Show imprisoned thieves with different character ('I' instead of 'T')
                if (p.IsImprisoned && p.RoleName == "Thief")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("I"); // 'I' for Imprisoned
                }
                else
                {
                    Console.Write(p.Character); // Normal character symbol
                }
                Console.ResetColor();
            }
        }


        //interaction
        public static void HandleCollisions(List<Person> characters, List<(int X, int Y)> collisionPositions, City city, Prison prison, Random rnd)
        {
            //        For evey charachter at a collision site
            foreach (var pos in collisionPositions)
            {
                //Filters them out
                var collided = characters.Where(p => p.X == pos.X && p.Y == pos.Y).ToList();

                if (collided.Count < 2)
                    continue;

                // Loop through all unique pairs of characters at this collision position
                for (int i = 0; i < collided.Count; i++)
                {
                    for (int j = i + 1; j < collided.Count; j++)
                    {
                        var p1 = collided[i];
                        var p2 = collided[j];

                        // Skip interactions involving imprisoned characters (except within prison)
                        if (p1.IsImprisoned && p2.IsImprisoned)
                        {
                            // Imprisoned characters can interact with each other
                            DrawLog($"Imprisoned {p1.Name} meets imprisoned {p2.Name} in prison", city, prison);

                            continue;
                        }
                        else if (p1.IsImprisoned || p2.IsImprisoned)
                        {
                            // Skip interactions between imprisoned and free characters
                            continue;
                        }

                        // Handle Thief + Civilian collision
                        if ((p1.RoleName == "Thief" && p2.RoleName == "Civilian") ||
                            (p1.RoleName == "Civilian" && p2.RoleName == "Thief"))
                        {
                            var thief = p1.RoleName == "Thief" ? p1 as Thief : p2 as Thief;
                            var civilian = p1.RoleName == "Civilian" ? p1 as Civilian : p2 as Civilian;

                            DrawLog($"Thief {thief.Name} meets Civilian {civilian.Name}", city, prison);
                            thief?.StealFrom(civilian, rnd, city, prison);

                        }
                        // Handle Police + Thief collision
                        else if ((p1.RoleName == "Thief" && p2.RoleName == "Police officer") ||
                                 (p1.RoleName == "Police officer" && p2.RoleName == "Thief"))
                        {
                            var thief = p1.RoleName == "Thief" ? (Thief)p1 : (Thief)p2;
                            var police = p1.RoleName == "Police officer" ? (Police)p1 : (Police)p2;

                            DrawLog($"Police {police.Name} encounters Thief {thief.Name}!", city, prison);

                            // Attempt to arrest the thief
                            police.ArrestThief(thief, rnd, city, prison);


                        }
                        // Handle Civilian + Civilian collision
                        else if (p1.RoleName == "Civilian" && p2.RoleName == "Civilian")
                        {
                            // Nothing happens, just a greeting
                            DrawLog($"Civilian {p1.Name} meets Civilian {p2.Name}", city, prison);

                        }
                        // Handle Police + Civilian collision
                        else if ((p1.RoleName == "Police officer" && p2.RoleName == "Civilian") ||
                                 (p1.RoleName == "Civilian" && p2.RoleName == "Police officer"))
                        {
                            var police = p1.RoleName == "Police officer" ? (Police)p1 : (Police)p2;
                            var civilian = p1.RoleName == "Civilian" ? (Civilian)p1 : (Civilian)p2;

                            // Nothing happens, just a greeting
                            DrawLog($"Police {police.Name} greets Civilian {civilian.Name}", city, prison);

                        }
                        // Handle Police + Police collision
                        else if (p1.RoleName == "Police officer" && p2.RoleName == "Police officer")
                        {
                            // Nothing happens, colleagues meet
                            DrawLog($"Police {p1.Name} meets Police {p2.Name}", city, prison);

                        }
                        else if (p1.RoleName == "Thief" && p2.RoleName == "Thief")
                        {
                            // Just log their meeting, similar to other cases
                            DrawLog($"Thief {p1.Name} meets Thief {p2.Name}", city, prison);

                        }
                        else
                        {

                        }
                        Thread.Sleep(2000);

                    }
                }
            }
        }

        // Draws messages in the bottom area of the console (scrolling log)
        internal static void DrawLog(string message, City city, Prison prison)
        {
            // Calculate where log area starts (below city and prison)
            int logStartRow = city.Height + prison.Height + 2;

            // Add the new message to the queue
            logs.Enqueue(message);

            // Limit the queue size to 5 lines (remove the oldest if too many)
            if (logs.Count > 5)
                logs.Dequeue();

            // Redraw all log lines
            int i = 0;
            foreach (var log in logs)
            {
                Console.SetCursorPosition(0, logStartRow + i);
                Console.Write(new string(' ', Console.WindowWidth)); // Clear line
                Console.SetCursorPosition(0, logStartRow + i);
                Console.WriteLine(log);
                i++;
            }
        }
    }
}
