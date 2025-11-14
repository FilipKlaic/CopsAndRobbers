using System.Drawing;
using System.Xml.Linq;

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
            int safeTop = cityStartRow + 2;             // skip title line and top border
            int safeBottom = cityStartRow + city.Height - 2; // skip bottom border
            int safeLeft = cityStartCol + 1;            // skip left border
            int safeRight = cityStartCol + city.Width - 2; // skip right border

            // Store previous positions so we can restore underlying canvas chars
            var previousPositions = new Dictionary<Person, (int X, int Y)>();
            foreach (var p in characters)
                previousPositions[p] = (p.X, p.Y);

            bool paused = false;   // pause flag
            bool running = true;   // exit control flag

            // Draw initial characters (on top of already drawn canvas)
            foreach (var p in characters)
            {
                if (p is Thief thief && thief.IsInPrison)
                    continue;
                else if (p.X >= safeTop && p.X <= safeBottom && p.Y >= safeLeft && p.Y <= safeRight)
                {
                    Console.SetCursorPosition(p.Y, p.X);
                    Console.ForegroundColor = p.Charactercolor;
                    Console.Write(p.Character);
                    Console.ResetColor();
                }
            }
            DrawCityStats(characters, city, prison);

            // Main loop
            while (true)
            {
                // check keyboard input without blocking
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.P)
                    {
                        paused = !paused; // toggle pause
                        
                    }
                    else if (key == ConsoleKey.Q)
                    {
                        break; // exit loop
                    }
                }

                if (paused)
                {
                    Thread.Sleep(200); // slow loop while paused
                    continue; // skip movement
                }
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

                    // wrap-around (teleport to other side if outside city)
                    if (candidateX < safeTop)
                        candidateX = safeBottom - 1;
                    else if (candidateX > safeBottom - 1)
                        candidateX = safeTop;

                    if (candidateY < safeLeft)
                        candidateY = safeRight - 1;
                    else if (candidateY > safeRight - 1)
                        candidateY = safeLeft;

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

                HandleCollisions(characters, city, prison, rnd);

                //Console.SetCursorPosition(0, logStartRow);
                //DrawLog("Characters and their positions:", city, prison);
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
            int prisonStartRow = city.Height + 0;
            int prisonStartCol = 0;
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

                        DrawLog($" Thief {thief.Name} meets Civilian{civilian.Name}", city, prison);
                        thief?.StealFrom(civilian, rnd, city, prison);
                        Thread.Sleep(300);
                    }
                
                    else if ((p1.RoleName == "Thief" && p2.RoleName == "Police officer") ||
                             (p1.RoleName == "Police officer" && p2.RoleName == "Thief"))
                    {
                        var thief = p1.RoleName == "Thief" ? (Thief)p1 : (Thief)p2;
                        var police = p1.RoleName == "Police officer" ? (Police)p1 : (Police)p2;

                        DrawLog($" Police {police.Name} catches Thief {thief.Name}!", city, prison);
                     

                        if (thief.Inventory.Items.Count > 0)
                        {
                            int jailTime = thief.Inventory.Items.Count; // 1 thing = 1 second
                            Console.ForegroundColor = ConsoleColor.Red;
                            DrawLog($"** Police {police.Name} catches Thief {thief.Name}! Sent to prison for {jailTime} seconds.", city, prison);
                            police?.StealFrom(thief, rnd, city, prison);

                            // move the thief to prison
                            city.Persons.Remove(thief);
                            prison.Persons.Add(thief);

                            thief.X = prisonStartRow + 2;
                            thief.Y = prisonStartCol + 3;

                            int prisonX = prisonStartRow + rnd.Next(1, prison.Height - 1);
                            int prisonY = prisonStartCol + rnd.Next(1, prison.Width - 1);
                            thief.X = prisonX;
                            thief.Y = prisonY;

                            // draw thief in the prison
                            Console.SetCursorPosition(thief.Y, thief.X);
                            Console.ForegroundColor = thief.Charactercolor;
                            Console.Write(thief.Character);
                            Console.ResetColor();

                            thief.PrisonReleaseTime = DateTime.Now.AddSeconds(jailTime);
                            DrawCityStats(characters, city, prison);


                            Thread.Sleep(jailTime * 1000); // wait 

                            Console.SetCursorPosition(thief.Y, thief.X);
                            Console.Write(' ');

                            // after prison return back to the city
                            Console.ForegroundColor = ConsoleColor.Green;
                            DrawLog($" <<<Thief {thief.Name} released from prison!", city, prison);

                            prison.Persons.Remove(thief);
                            city.Persons.Add(thief);
                            thief.X = rnd.Next(2, city.Height - 2);
                            thief.Y = rnd.Next(2, city.Width - 2);

                            DrawCityStats(characters, city, prison);
                        }
                        else
                        {
                            // if thief has nothing 
                            DrawLog($" Police {police.Name} stops {thief.Name}, but finds nothing.", city, prison);
                        }

                        Thread.Sleep(300);
                    }
                    
                    else if (p1.RoleName == "Civilian" && p2.RoleName == "Civilian")
                    {
                        // Nothing happens
                       DrawLog($" Civilian {p1.Name} meets Civilian {p2.Name}", city, prison);
                        Thread.Sleep(300);
                    }
                  
                    else if ((p1.RoleName == "Police officer" && p2.RoleName == "Civilian") ||
                             (p1.RoleName == "Civilian" && p2.RoleName == "Police officer"))
                    {
                        var police = p1.RoleName == "Police officer" ? (Police)p1 : (Police)p2;
                        var civilian = p1.RoleName == "Civilian" ? (Civilian)p1 : (Civilian)p2;
                        // Nothing happens

                        DrawLog($" Police {police.Name} greets Civilian {civilian.Name}", city, prison);
                        Thread.Sleep(1000);
                    }
                    
                    else if (p1.RoleName == "Police officer" && p2.RoleName == "Police officer")
                    {
                        // Nothing happens
      
                        DrawLog($" Police {p1.Name} meets Police {p2.Name}", city, prison);
                        Thread.Sleep(300);
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
            int logStartRow = city.Height + prison.Height + 8;

            // Add the new message to the queue
            logs.Enqueue(message);

            // Limit the queue size to 10 lines (remove the oldest if too many)
            if (logs.Count > 10)
                logs.Dequeue();

            // Redraw all log lines
            int i = 1;
            foreach (var log in logs)
            {
                Console.SetCursorPosition(0, logStartRow + i);
                Console.Write(new string(' ', Console.WindowWidth)); // clear line
                Console.SetCursorPosition(0, logStartRow + i);
                Console.WriteLine(log);
                i++;
            }
        }
        internal static void DrawCityStats(List<Person> characters, City city, Prison prison)
        {
            int logStartRow = city.Height + prison.Height + 2; // draw logs below prison 

            int thiefCount = city.Persons.Count(p => p.RoleName == "Thief");
            int policeCount = city.Persons.Count(p => p.RoleName == "Police officer");
            int civilianCount = city.Persons.Count(p => p.RoleName == "Civilian");

            int prisonCount = prison.Persons.Count;

            // clear the old lines
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(0, logStartRow + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, logStartRow);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, logStartRow);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"======= CITY STATS =========================================");
            Console.SetCursorPosition(0, logStartRow + 1);
            Console.WriteLine($"  Police: {policeCount}");
            Console.WriteLine($"  Thieves: {thiefCount}");
            Console.WriteLine($"  Civilians: {civilianCount}");
            Console.WriteLine($"  In Prison: {prisonCount}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("======= NEWS ================================================");
            Console.ResetColor();

        }

    }
}
