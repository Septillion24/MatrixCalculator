using System.Diagnostics.Contracts;
using System.Security.Claims;
using System.Text;

class Matrix
{

    // list of rows
    // [[1,2,3],
    //  [1,2,3]]
    int rows;
    int columns;
    List<List<Expression>> values;


    public Matrix(List<List<Expression>> values)
    {
        this.values = values;
        this.rows = values.Count;
        this.columns = values[0].Count;
    }
    public Matrix(int rows, int columns)
    {
        List<List<Expression>> values = createEmptyMatrix(rows, columns);
        this.values = values;
        this.rows = values.Count;
        this.columns = values[0].Count;
    }


    public static List<List<Expression>> createEmptyMatrix(int rows, int columns)
    {
        List<List<Expression>> returnValue = new List<List<Expression>>() { };

        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            returnValue.Add(new List<Expression>() { });
            for (int currentColumn = 0; currentColumn < columns; currentColumn++)
            {
                returnValue[currentRow].Add(new Expression());
            }
        }

        return returnValue;
    }
    public static Expression calculateDotProductVector(List<Expression> vectorA, List<Expression> vectorB)
    {
        Expression sum = new Expression();
        for (int i = 0; i < vectorA.Count; i++)
        {
            sum += vectorA[i] * vectorB[i];
        }
        return sum;
    }

    public List<Expression> getColumnList(int column)
    {
        // columns start at 0
        List<Expression> returnValue = new List<Expression>() { };
        foreach (List<Expression> row in this.values)
        {
            returnValue.Add(row[column]);
        }
        return returnValue;
    }
    public List<Expression> getRowList(int row)
    {
        // row starts at 0
        List<Expression> returnValue = new List<Expression>() { };
        return this.values[row];
    }

    public void setRowColumn(int row, int column, Expression value)
    {
        this.values[row][column] = value;
    }
    public Expression getRowColumn(int row, int column)
    {
        return this.values[row][column];
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var row in values)
        {
            sb.Append("[");
            sb.Append(string.Join(", ", row));
            sb.Append("]\n");
        }
        return sb.ToString();
    }
    public Matrix getSubmatrix(int row, int column)
    {
        Matrix returnMatrix = new Matrix(rows - 1, columns - 1);
        int rowToAdd = 0;
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            if (currentRow == row) continue;
            int columnToAdd = 0;
            for (int currentColumn = 0; currentColumn < columns; currentColumn++)
            {
                if (currentColumn == column) continue;
                returnMatrix.setRowColumn(rowToAdd, columnToAdd, getRowColumn(currentRow, currentColumn));
                columnToAdd++;
            }
            rowToAdd++;
        }
        return returnMatrix;
    }



    public Matrix? multiply(Matrix other)
    {
        // this.rows, this.columns x other.rows, other.columns
        if (this.columns != other.rows)
        {
            return null;
        }


        Matrix returnMatrix = new Matrix(this.rows, other.columns);
        List<Expression> columnToProcess;
        List<Expression> rowToProcess;
        for (int rows = 0; rows < returnMatrix.rows; rows++)
        {
            for (int columns = 0; columns < returnMatrix.columns; columns++)
            {
                columnToProcess = other.getColumnList(columns);
                rowToProcess = this.getRowList(rows);
                returnMatrix.setRowColumn(rows, columns, calculateDotProductVector(rowToProcess, columnToProcess));
            }
        }

        return returnMatrix;

    }

    public static Matrix getIdentityMatrix(int rows)
    {
        Matrix returnMatrix = new Matrix(rows, rows);
        for (int i = 0; i < rows; i++)
        {
            returnMatrix.setRowColumn(i, i, new(1));
        }
        return returnMatrix;
    }
    public static Expression getDeterminant(Matrix matrix)
    {
        if (matrix.rows != matrix.columns)
        {
            throw new InvalidOperationException("Cannot calculate determinant of a non-square matrix.");
        }

        if (matrix.rows == 1)
        {
            return matrix.getRowColumn(0, 0);
        }
        if (matrix.rows == 2)
        {
            return (matrix.getRowColumn(0, 0) * matrix.getRowColumn(1, 1)) - (matrix.getRowColumn(1, 0) * matrix.getRowColumn(0, 1));
        }
        if (matrix.rows == 3)
        {
            Expression det;
            det = matrix.getRowColumn(0, 0) * (matrix.getRowColumn(1, 1) * matrix.getRowColumn(2, 2) - matrix.getRowColumn(1, 2) * matrix.getRowColumn(2, 1));
            det -= matrix.getRowColumn(0, 1) * (matrix.getRowColumn(1, 0) * matrix.getRowColumn(2, 2) - matrix.getRowColumn(1, 2) * matrix.getRowColumn(2, 0));
            det += matrix.getRowColumn(0, 2) * (matrix.getRowColumn(1, 0) * matrix.getRowColumn(2, 1) - matrix.getRowColumn(1, 1) * matrix.getRowColumn(2, 0));
            return det;
        }
        else
        {
            Matrix submatrix = new Matrix(matrix.rows - 1, matrix.rows - 1);
            Expression det = new();
            // always use first row
            for (int column = 0; column < matrix.rows; column++)
            {
                det += matrix.getRowColumn(0, column) * new Expression((float)Math.Pow(-1, column)) * getDeterminant(matrix.getSubmatrix(0, column));
            }


            return det;
        }
    }

}