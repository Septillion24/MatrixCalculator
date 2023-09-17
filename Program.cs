class Program
{
    static void Main(string[] args)
    {
        promptUserMatrix("A");
    }


    static Matrix? promptUserMatrix(string matrixName)
    {
        Console.Clear();
        Console.WriteLine("Input dimensions of matrix:");
        Console.Write("Rows: ");
        String userInput = Console.ReadLine();
        if (!int.TryParse(userInput, out int rows))
        {
            return null;
        }
        Console.Write("Columns: ");
        userInput = Console.ReadLine();
        if (!int.TryParse(userInput, out int columns))
        {
            return null;
        }
        Matrix userMatrix = new Matrix(rows, columns);

        float userValue;
        bool success = false;
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Console.Clear();
                do
                {
                    
                    Console.WriteLine(userMatrix.ToString());
                    Console.Write($"\n Input value for value at {row},{column}: ");
                    userInput = Console.ReadLine();
                    if (float.TryParse(userInput, out userValue))
                    {
                        userMatrix.setRowColumn(row, column, userValue);
                    }
                    else
                    {
                        Console.WriteLine($"Failed, could not convert \"{userInput}\" to a float. Please try again.");
                    }

                } while (!float.TryParse(userInput, out userValue));
            }
        }
        
        return userMatrix;
    }



}