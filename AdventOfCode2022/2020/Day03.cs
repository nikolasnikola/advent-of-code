namespace AdventOfCode._2020
{
    internal static class Day03
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsCharMatrix("2020", "Day03");

            var result = GetResultForSlope(input, 3, 1);

            Console.WriteLine(result);
        }

        internal static void Part2()
        {

            var input = Reader.ReadAsCharMatrix("2020", "Day03");

            long result1 = GetResultForSlope(input, 1, 1);
            long result2 = GetResultForSlope(input, 3, 1);
            long result3 = GetResultForSlope(input, 5, 1);
            long result4 = GetResultForSlope(input, 7, 1);
            long result5 = GetResultForSlope(input, 1, 2);

            Console.WriteLine(result1*result2*result3*result4*result5);
        }

        static int GetResultForSlope(char[,] input, int right, int bottom)
        {
            var totalCols = input.GetLength(1);
            var totalRows = input.GetLength(0);

            var currentCol = 0;
            var result = 0;

            for (int i = bottom; i < totalRows; i+=bottom)
            {
                currentCol += right;
                var currentRowItems = GetRow(input, i);
                if (currentRowItems[currentCol % totalCols] == '#') result++;
            }

            return result;
        }

        static char[] GetRow(char[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

    }
}
