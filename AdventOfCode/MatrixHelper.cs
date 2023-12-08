using NUnit.Framework;

namespace AdventOfCode
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

        public static T[] GetNeigbores<T>(T[,] matrix, int row, int column)
        {
            List<T> result = new List<T>();
            int rowMinimum = Math.Max(row - 1, 0);
            int rowMaximum = Math.Min(row + 1, matrix.GetLength(0) - 1);
            int columnMinimum = Math.Max(column - 1, 0);
            int columnMaximum = Math.Min(column + 1, matrix.GetLength(1) - 1);
            for (int i = rowMinimum; i <= rowMaximum; i++)
                for (int j = columnMinimum; j <= columnMaximum; j++)
                    if (i != row || j != column)
                        result.Add(matrix[i, j]);
            return result.ToArray();
        }

        public static (int, int)[] GetNeighborCells<T>(T[,] matrix, int row, int column)
        {
            var result = new List<(int, int)>();
            int rowMinimum = Math.Max(row - 1, 0);
            int rowMaximum = Math.Min(row + 1, matrix.GetLength(0) - 1);
            int columnMinimum = Math.Max(column - 1, 0);
            int columnMaximum = Math.Min(column + 1, matrix.GetLength(1) - 1);
            for (int i = rowMinimum; i <= rowMaximum; i++)
                for (int j = columnMinimum; j <= columnMaximum; j++)
                    if (i != row || j != column)
                        result.Add((i, j));
            return result.ToArray();
        }
    }

    public class Tests
    {
        [Test]
        public void NeighborTests()
        {
            var matrixToTest = new int[,]
            {
                { 01, 02, 03, 04, 05 },
                { 06, 07, 08, 09, 10 },
                { 11, 12, 13, 14, 15 },
                { 16, 17, 18, 19, 20 },
                { 21, 22, 23, 24, 25 },
            };

            var result = MatrixHelper.GetNeigbores(matrixToTest, 2, 2);
            CollectionAssert.AreEquivalent(new int[] { 7, 8, 9, 12, 14, 17, 18, 19 }, result);

            var result2 = MatrixHelper.GetNeigbores(matrixToTest, 0, 0);
            CollectionAssert.AreEquivalent(new int[] { 2, 6, 7 }, result2);

            var result3 = MatrixHelper.GetNeigbores(matrixToTest, 4, 1);
            CollectionAssert.AreEquivalent(new int[] { 16, 17, 18, 21, 23 }, result3);

            var result4 = MatrixHelper.GetNeigbores(matrixToTest, 3, 4);
            CollectionAssert.AreEquivalent(new int[] { 14, 15, 19, 24, 25 }, result4);

            var result5 = MatrixHelper.GetNeigbores(matrixToTest, 1, 1);
            CollectionAssert.AreEquivalent(new int[] { 1, 2, 3, 6, 8, 11, 12, 13 }, result5);
        }
    }
}
