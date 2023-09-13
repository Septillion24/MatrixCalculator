Matrix matrix1 = new Matrix(new List<List<float>>() 
{
    new List<float> {1, 2},
    new List<float> {2, 3}
}
);

Matrix matrix2 = new Matrix(new List<List<float>>() 
{
    new List<float> {3, 4},
    new List<float> {1, 5}
}
);


Console.WriteLine(matrix1.multiply(matrix2).ToString());