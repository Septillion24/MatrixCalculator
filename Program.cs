using System.Diagnostics;
using System.Text.RegularExpressions;

class Program
{
    static char nextLetter = 'A';
    static void Main(string[] args)
    {
        Dictionary<string, Matrix> matricies = new() { };
        Console.Clear();


        while (true)
        {
            Console.WriteLine("Current matricies: [" + string.Join(", ", matricies.Keys) + "]");
            Console.WriteLine("Input command: ");
            Console.Write(">");

            string userInput = Console.ReadLine() ?? "";
            getUserAction(userInput, matricies);
        }

    }

    static void getUserAction(string userInput, Dictionary<string, Matrix> matricies)
    {
        Regex regex = new Regex(@"(\d*)?([A-Za-z])\s*([\+\-\*])\s*(\d*)?([A-Za-z])");
        Match match = regex.Match(userInput);
        if (match.Success)
        {
            doUserOperation(match, matricies);
            return;
        }
        string[] parts = userInput.Split(' ');
        string command = parts[0];
        if (command == "add" || command == "new")
        {
            Matrix result = promptNewUserMatrix(parts[1].ToUpper());
            matricies[parts[1].ToUpper()] = result;
        }
        if (command == "det")
        {
            if (matricies.ContainsKey(parts[1].ToUpper()))
            {
                Console.WriteLine(matricies[parts[1].ToUpper()].getDeterminant());
            }
            else
            {
                Console.WriteLine($"Could not find matrix {parts[1].ToUpper()} in list.");
            }
        }
        if (command == "change" || command == "edit")
        {
            if (matricies.ContainsKey(parts[1].ToUpper()))
            {
                //TODO
            }
            else
            {
                Console.WriteLine($"Could not find matrix {parts[1].ToUpper()} in list.");
            }
        }


        static void doUserOperation(Match match, Dictionary<string, Matrix> matricies) // TODO
        {
            string num1 = match.Groups[1].Value;
            // Matrix matrix1 = matricies[match.Groups[2].Value];
            string operatorSymbol = match.Groups[3].Value;
            string num2 = match.Groups[4].Value;
            string char2 = match.Groups[5].Value;
            if (matricies.TryGetValue(match.Groups[2].Value.ToUpper(), out Matrix matrix1) && matricies.TryGetValue(match.Groups[5].Value.ToUpper(), out Matrix matrix2))
            {
        
                if (operatorSymbol == "+")
                {
                    Console.WriteLine(matrix1.add(matrix2));
                }
                else if (operatorSymbol == "-")
                {
                    Console.WriteLine(matrix1.subtract(matrix2));
                }
                else if (operatorSymbol == "*")
                {
                    Console.WriteLine(matrix1.multiply(matrix2));
                }
                else
                {
                    Console.WriteLine("Somehow this happened");
                }
            }
            else if (matricies.Keys.Contains(match.Groups[2].Value))
            {
                Console.WriteLine($"Matrix {match.Groups[2].Value.ToUpper()} was not found in list. Perhaps use \"add {match.Groups[2].Value.ToUpper()}\" to create it.");
                return;
            }
            else
            {
                Console.WriteLine($"Matrix {match.Groups[5].Value.ToUpper()} was not found in list. Perhaps use \"add {match.Groups[5].Value.ToUpper()}\" to create it.");
                return;
            }
        }
    }


    static Matrix promptNewUserMatrix(string matrixName, bool force = true)
    {
        Console.Clear();

        string userInput;

        promptUserCommandOrDimensions(matrixName, force, out int rows, out int columns, out userInput);

        Matrix userMatrix = new Matrix(rows, columns);

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
        //TODO: refactor
        static void promptUserCommandOrDimensions(string matrixName, bool force, out int rows, out int columns, out string userInput)
        {
            Match match;
            //TODO: doesn't work lol
            do
            {
                rows = 0;
                columns = 0;
                Console.WriteLine($"Input dimensions of matrix {matrixName} (format: \"rows x columns\")\n Alternately, use : to specify a command and type the matrix command after(eg, :3i for a 3x3 identity matrix)");
                Console.Write("> ");
                userInput = Console.ReadLine() ?? "";
                if (userInput[0] == ':')
                {
                    (string letters, string numbers) userResult = parseUserResultMatrixEntry(userInput);
                    if (userResult.letters.Contains("i"))
                    {
                        Matrix.getIdentityMatrix(int.Parse(userResult.numbers));
                        force = false;
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
            userInput = Console.ReadLine() ?? "";
            userInput = ReplaceWhitespace(userInput);
            (string letters, string numbers) userResult = parseUserResultMatrixEntry(userInput);
            if (float.TryParse(userInput, out userValue))
            {
                Expression expression = new Expression('x', 0, 1, userValue);
                userMatrix.setRowColumn(row, column, expression);
                Console.Clear();
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
    public static string ReplaceWhitespace(string input, string replacement)
    {
        Regex sWhitespace = new Regex(@"\s+");
        return sWhitespace.Replace(input, replacement);
    }
    public static string ReplaceWhitespace(string input)
    {
        Regex sWhitespace = new Regex(@"\s+");
        return sWhitespace.Replace(input, "");
    }
    public static (string, string) parseUserResultMatrixEntry(string userInput)
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