namespace AdventOfCode2022._2022
{
    internal static class Day8
    {
        internal static void Part2()
        {
            var map = Reader.ReadAsIntegerMatrix("Day8");

            var rows = map.GetLength(0);
            var cols = map.GetLength(1);
            var scores = new List<int>();

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    var currentItem = map[i, j];

                    scores.Add(GetScenicScore(map, currentItem, i, j));
                }
            }

            var result = scores.Max();
            Console.WriteLine(result);
        }

        internal static void Part1()
        {
            var map = Reader.ReadAsIntegerMatrix("Day8");

            var rows = map.GetLength(0);
            var cols = map.GetLength(1);
            var result = rows * 2 + cols * 2 - 4;

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    var currentItem = map[i, j];

                    if (IsVissible(map, map[i, j], i, j))
                    {
                        result++;
                    }
                }
            }

            Console.WriteLine(result);
        }

        static int GetScenicScore(int[,] map, int currentItem, int row, int column)
            => GetTopVisibleCount(map, currentItem, row, column)
            * GetBottomVisibleCount(map, currentItem, row, column)
            * GetLeftVisibleCount(map, currentItem, row, column)
            * GetRightVisibleCount(map, currentItem, row, column);

        static int GetTopVisibleCount(int[,] map, int currentItem, int row, int column)
        {
            var currentColumn = MatrixHelper.GetColumn(map, column);
            var result = 0;

            for (int i = row - 1; i >= 0; i--)
            {
                result++;
                if (currentItem <= currentColumn[i])
                {
                    break;
                }
            }
            return result;
        }

        static int GetBottomVisibleCount(int[,] map, int currentItem, int row, int column)
        {
            var currentColumn = MatrixHelper.GetColumn(map, column);
            var result = 0;

            for (int i = row + 1; i < currentColumn.Length; i++)
            {
                result++;
                if (currentItem <= currentColumn[i])
                {
                    break;
                }
            }
            return result;
        }

        static int GetLeftVisibleCount(int[,] map, int currentItem, int row, int column)
        {
            var currentRow = MatrixHelper.GetRow(map, row);
            var result = 0;

            for (int i = column - 1; i >= 0; i--)
            {
                result++;
                if (currentItem <= currentRow[i])
                {
                    break;
                }
            }
            return result;
        }

        static int GetRightVisibleCount(int[,] map, int currentItem, int row, int column)
        {
            var currentRow = MatrixHelper.GetRow(map, row);
            var result = 0;

            for (int i = column + 1; i < currentRow.Length; i++)
            {
                result++;
                if (currentItem <= currentRow[i])
                {
                    break;
                }
            }
            return result;
        }

        static bool IsVissible(int[,] map, int currentItem, int row, int column)
            => IsVisibleFromTop(map, currentItem, row, column)
            || IsVisibleFromBottom(map, currentItem, row, column)
            || IsVisibleFromLeft(map, currentItem, row, column)
            || IsVisibleFromRight(map, currentItem, row, column);


        static bool IsVisibleFromTop(int[,] map, int currentItem, int row, int column)
        {
            var currentColumn = MatrixHelper.GetColumn(map, column);

            for (int i = 0; i < row; i++)
            {
                if (currentItem <= currentColumn[i])
                {
                    return false;
                }
            }
            return true;
        }

        static bool IsVisibleFromBottom(int[,] map, int currentItem, int row, int column)
        {
            var currentColumn = MatrixHelper.GetColumn(map, column);

            for (int i = row + 1; i < currentColumn.Length; i++)
            {
                if (currentItem <= currentColumn[i])
                {
                    return false;
                }
            }
            return true;
        }

        static bool IsVisibleFromLeft(int[,] map, int currentItem, int row, int column)
        {
            var currentRow = MatrixHelper.GetRow(map, row);

            for (int i = 0; i < column; i++)
            {
                if (currentItem <= currentRow[i])
                {
                    return false;
                }
            }
            return true;
        }

        static bool IsVisibleFromRight(int[,] map, int currentItem, int row, int column)
        {
            var currentRow = MatrixHelper.GetRow(map, row);

            for (int i = column + 1; i < currentRow.Length; i++)
            {
                if (currentItem <= currentRow[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
