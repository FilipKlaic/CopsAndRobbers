namespace CopsAndRobbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[,] drawing = Helpers.DrawingClassInitilize();
            
            Random rnd = new Random();

            List<Person> list = new List<Person>
            {
                new Civilian(5, 5),
                new Thief(5, 5),
                new Police(5, 5)
            };

            while (true)
            {
                //Creates a list for each square on the drawing to be able to display and hold multiple chcarachters without overwriting eachother
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
                    int dx = rnd.Next(-1, 2); 
                    int dy = rnd.Next(-1, 2); 
                    
                    // Calculate new position and clamp to stay inside borders
                    // Math.Clamp prevents moving into border (row/col 0 or last row/col)
                    int newX = Math.Clamp(p.X + dx, 1, drawing.GetLength(0) - 2);
                    int newY = Math.Clamp(p.Y + dy, 1, drawing.GetLength(1) - 2);

                    p.X = newX;
                    p.Y = newY;

                    // sparar personen på den motsvarande platsen i en lista.
                    positions[p.X, p.Y].Add(p); 
                }

                //Reset drawingför att bättre göra rörande karaktärer utan att överstryka dubbla posistioner när dom flyttar till samma ruta.
                //kan ej använda DrawingClassInitilize då den är kopplad till att göra en ny array för drawing.
                //Denna skriver inte heller ut till konsollen och ritar upp något. allt flyter på bättre. Den rensar endast rutorna mellan flytten.
                //kan beskrivas som bowling mellan varje plan rensing
                Helpers.WipeDrawing(drawing);

                // Draw characters onto the grid based on position tracker   move thorugh "array of lists"
                for (int row = 0; row < positions.GetLength(0); row++)
                {
                    for (int col = 0; col < positions.GetLength(1); col++)
                    {
                        // Check if any characters are at this position   count = person
                        if (positions[row, col].Count > 0)
                        {
                            if (positions[row, col].Count == 1)
                            {
                                drawing[row, col] = positions[row, col][0].Character;  //0 är personen i listan. det ända objected i listan.
                            }
                            else
                            {
                                drawing[row, col] = positions[row, col].Count.ToString();
                            }
                        }
                    }
                }
                
                Helpers.RedrawDrawing(drawing);
                Console.ReadKey();
            }
        }
    }
}
