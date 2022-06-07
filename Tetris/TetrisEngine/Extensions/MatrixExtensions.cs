namespace Engine.Extensions;

public static class MatrixExtensions
{
    public static int[] GetRow(this int[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
            .Select(i => matrix[rowNumber,i])
            .ToArray();
    }

    public static int Rows(this int[,] matrix) => matrix.GetLength(0);
    public static int Columns(this int[,] matrix) => matrix.GetLength(1);
    
    public static void SetRow(this int[,] matrix, int rowNumber,int column, Func<int, int> value)
    {
        Enumerable.Range(0,column).ToList().ForEach(i=> matrix[rowNumber, i] = value(i));
    }
 
    
    public static int[,] SubMatrix(this int[,] matrix, int rowNumber, int columnNumber)
    {
        int[,] jagged = new int[rowNumber,columnNumber];
        for (int row = 0; row < rowNumber; row++) 
            jagged.SetRow(row, columnNumber, i=>matrix[row, i]);
        return jagged;
    }
}