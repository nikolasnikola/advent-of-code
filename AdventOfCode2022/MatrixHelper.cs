namespace AdventOfCode2022
{
    public static class MatrixHelper
    {
        public static T[] GetColumn<T>(T[,] matrix, int columnNumber) 
            => Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();

        public static T[] GetRow<T>(T[,] matrix, int rowNumber) 
            => Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
    }
}
