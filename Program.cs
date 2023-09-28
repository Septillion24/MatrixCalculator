using System.Text.RegularExpressions;

class Program
{

    static char nextLetter = 'A';
    static void Main(string[] args)
    {
        Dictionary<string, Matrix> matricies = new() { };


        matricies["A"] = promptUserMatrix("A");
        Console.WriteLine(matricies["A"] + "\nDet:");
        Console.WriteLine(Matrix.getDeterminant(matricies["A"]));
        // matricies["B"] = promptUserMatrix("B");
        // Console.WriteLine($"{matricies["A"]} \n * \n {matricies["B"]} \n = ");
        // Console.WriteLine(matricies["A"].multiply(matricies["B"]));

    }


    static Matrix? promptUserMatrix(string matrixName, bool force = true)
    {
        Console.Clear();
        Match match;


        string userInput;

        promptUserCommandOrDimensions(matrixName, force, out match, out int rows, out int columns, out userInput);

        Matrix userMatrix = new Matrix(rows, columns);

        float userValue;
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Console.Clear();
                bool continuePrompting = true;
                while (continuePrompting)
                {
                    continuePrompting = promptRowColumn(out userInput, userMatrix, row, column);
                }

            }
        }

        return userMatrix;

        static void promptUserCommandOrDimensions(string matrixName, bool force, out Match match, out int rows, out int columns, out string userInput)
        {
            //TODO: doesn't work lol
            do
            {
                rows = 0;
                columns = 0;
                Console.WriteLine($"Input dimensions of matrix {matrixName} (format: \"rows x columns\")\n Alternately, use : to specify a command and type the matrix command after(eg, :3i for a 3x3 identity matrix)");
                Console.Write("> ");
                userInput = Console.ReadLine();
                if (userInput[0] == ':')
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

        }

        static bool promptRowColumn(out string userInput, Matrix userMatrix, int row, int column)
        {
            float userValue;
            Console.WriteLine(userMatrix.ToString());
            Console.Write($"\n Input value at {row},{column}: ");
            userInput = Console.ReadLine();
            userInput = ReplaceWhitespace(userInput);
            (string letters, string numbers) userResult = parseUserResult(userInput);
            if (float.TryParse(userInput, out userValue))
            {
                Expression expression = new Expression('x', 0, 1, userValue);
                userMatrix.setRowColumn(row, column, expression);
                return false;
            }
            else if (userResult.letters.Length == 1)
            {
                tryGetExpression(userInput, out Expression expression);
                userMatrix.setRowColumn(row, column, expression);
                return false;
            }
            else
            {
                Console.WriteLine($"Failed, could not convert \"{userInput}\" to an expression or number. Please try again.");
                return true;
            }
        }
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

    public static bool tryGetExpression(string input, out Expression? expression)
    {
        expression = null;
        Regex regex = new Regex(@"([\+\-]?\d+)?([a-zA-Z])?(\^[\+\-]?\d+)?([\+\-]\d+)?"); // an alien wrote this
        Match match = regex.Match(input);
        if (match.Success)
        {
            float coefficient = float.Parse(string.IsNullOrEmpty(match.Groups[1].Value) ? "1" : match.Groups[1].Value);
            char variable = match.Groups[2].Value[0];
            float exponent = float.Parse(string.IsNullOrEmpty(match.Groups[3].Value) ? "1" : match.Groups[3].Value.Substring(1));
            float constant = string.IsNullOrEmpty(match.Groups[4].Value) ? 0 : float.Parse(match.Groups[4].Value);

            expression = new Expression(variable, coefficient, exponent, constant);
            return true;
        }
        return false;

    }
}