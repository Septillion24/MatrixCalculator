using System.Text.RegularExpressions;

class Program
{

    static char nextLetter = 'A';
    static void Main(string[] args)
    {
        Dictionary<string, Matrix> matricies = new() { };



        promptUserMatrix("A");
        promptUserMatrix("B");
    }


    static Matrix? promptUserMatrix(string matrixName, bool force = true)
    {
        Console.Clear();
        Match match;
        int rows = 0;
        int columns = 0;
        string userInput;
        do
        {
            Console.WriteLine($"Input dimensions of matrix {matrixName} (format: \"rows x columns\")\n Alternately, use : to specify a command and type the matrix command after");
            Console.Write("> ");
            userInput = Console.ReadLine();
            if (userInput[1] == ':')
            {
                (string letters, string numbers) userResult = parseUserResult(userInput);
                if (userResult.letters.Contains("i"))
                {
                    Matrix.getIdentityMatrix(int.Parse(userResult.numbers));
                }
            }
            Regex regex = new Regex(@"(\d+)\s*x\s*(\d+)");
            match = regex.Match(userInput);
            if (match.Success)
            {
                rows = int.Parse(match.Groups[1].Value);
                columns = int.Parse(match.Groups[2].Value);
                Console.WriteLine($"Rows: {rows}, Columns: {columns}");
            }

        } while (!match.Success && force);

        // Console.Write("Rows: ");
        // String userInput = Console.ReadLine();
        // if (!int.TryParse(userInput, out int rows))
        // {
        //     return null;
        // }
        // Console.Write("Columns: ");
        // userInput = Console.ReadLine();
        // if (!int.TryParse(userInput, out int columns))
        // {
        //     return null;
        // }

        Matrix userMatrix = new Matrix(rows, columns);

        float userValue;
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
                    userInput = ReplaceWhitespace(userInput);
                    (string letters, string numbers) userResult = parseUserResult(userInput);
                    if (float.TryParse(userInput, out userValue))
                    {
                        userMatrix.setRowColumn(row, column, userValue);
                    }
                    else
                    {
                        Console.WriteLine($"Failed, could not convert \"{userInput}\" to a float or command. Please try again.");
                    }

                } while (!float.TryParse(userInput, out userValue));
            }
        }

        return userMatrix;
    }


    // regex junk
    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }
    public static string ReplaceWhitespace(string input)
    {
        return sWhitespace.Replace(input, "");
    }

    public static (string, string) parseUserResult(string userInput)
    {
        string _letters = Regex.Replace(userInput, "[^a-zA-Z]", "");
        string _numbers = Regex.Replace(userInput, "[^0-9]", "");

        var returnValue = (letters: _letters, numbers: _numbers);

        return returnValue;

    }


}