using System.Text;

class Matrix
{

    // list of rows
    // [[1,2,3],
    //  [1,2,3]]
    int rows;
    int columns;
    List<List<float>> values;


    public Matrix(List<List<float>> values)
    {
        this.values = values;
        this.rows = values.Count;
        this.columns = values[0].Count;
    }
    public Matrix(int rows, int columns)
    {
        List<List<float>> values = createEmptyMatrix(rows, columns);
        this.values = values;
        this.rows = values.Count;
        this.columns = values[0].Count;
    }


    public static List<List<float>> createEmptyMatrix(int rows, int columns)
    {
        List<List<float>> returnValue = new List<List<float>>() { };

        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            returnValue.Add(new List<float>() { });
            for (int currentColumn = 0; currentColumn < columns; currentColumn++)
            {
                returnValue[currentRow].Add(0);
            }
        }

        return returnValue;
    }
    public static float calculateDotProductVector(List<float> vectorA, List<float> vectorB)
    {
        float sum = 0;
        for (int i = 0; i < vectorA.Count; i++)
        {
            sum += vectorA[i] * vectorB[i];
        }
        return sum;
    }

    public List<float> getColumnList(int column)
    {
        // columns start at 0
        List<float> returnValue = new List<float>() { };
        foreach (List<float> row in this.values)
        {
            returnValue.Add(row[column]);
        }
        return returnValue;
    }
    public List<float> getRowList(int row)
    {
        // row starts at 0
        List<float> returnValue = new List<float>() { };
        return this.values[row];
    }

    public void setRowColumn(int row, int column, float value)
    {
        this.values[row][column] = value;
    }
    public float getRowColumn(int row, int column)
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



    public Matrix? multiply(Matrix other)
    {
        // this.rows, this.columns x other.rows, other.columns
        if (this.columns != other.rows)
        {
            return null;
        }


        Matrix returnMatrix = new Matrix(this.rows, other.columns);
        List<float> columnToProcess;
        List<float> rowToProcess;
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

}