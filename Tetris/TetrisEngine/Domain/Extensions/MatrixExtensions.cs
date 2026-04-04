namespace TetrisEngine.Domain.Extensions;

public static class MatrixExtensions
{
    public static int[] GetRow(this int[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.ColumnCount())
            .Select(i => matrix[rowNumber,i])
            .ToArray();
    }


    public static int RowCount(this int[,] matrix) => matrix.GetLength(0);
    public static int ColumnCount(this int[,] matrix) => matrix.GetLength(1);
    
}