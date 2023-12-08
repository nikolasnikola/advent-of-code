using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode._2020
{
    internal static class Day11
    {
        internal static void Part1()
        {
            var matrix = Reader.ReadAsCharMatrix("2020", "Day11");

            var occupied = '#';
            var empty = 'L';
            var floor = '.';
            var occupiedToEmpty = 'O';
            var emptyToOccupied = 'E';

            bool finished = false;
            var occupiedCounter = 0;

            while (!finished)
            {
                bool stateChanged = false;
                occupiedCounter = 0;

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == floor) continue;
                        var neighbours = MatrixHelper.GetNeigbores(matrix, i, j);

                        if (matrix[i, j] == empty && !neighbours.Any(n => n == occupied || n == occupiedToEmpty))
                        {
                            matrix[i, j] = emptyToOccupied;
                            stateChanged = true;
                        }

                        else if (matrix[i, j] == occupied && neighbours.Count(n => n == occupied || n == occupiedToEmpty) >=4)
                        {
                            matrix[i, j] = occupiedToEmpty;
                            stateChanged = true;
                        }

                        if (matrix[i, j] == occupied || matrix[i,j] == emptyToOccupied) occupiedCounter++;
                    }
                }

                ChangeStates(matrix, occupied, empty, occupiedToEmpty, emptyToOccupied);

                finished = !stateChanged;
            }
            

            Console.WriteLine(occupiedCounter);
        }

        internal static void Part2()
        {
            var matrix = Reader.ReadAsCharMatrix("2020", "Day11");

            var occupied = '#';
            var empty = 'L';
            var floor = '.';
            var occupiedToEmpty = 'O';
            var emptyToOccupied = 'E';

            bool finished = false;
            var occupiedCounter = 0;

            while (!finished)
            {
                bool stateChanged = false;
                occupiedCounter = 0;

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == floor) continue;
                        var neighbours = GetVisibleSeats(matrix, i, j, floor);

                        if (matrix[i, j] == empty && !neighbours.Any(n => n == occupied || n == occupiedToEmpty))
                        {
                            matrix[i, j] = emptyToOccupied;
                            stateChanged = true;
                        }

                        else if (matrix[i, j] == occupied && neighbours.Count(n => n == occupied || n == occupiedToEmpty) >= 5)
                        {
                            matrix[i, j] = occupiedToEmpty;
                            stateChanged = true;
                        }

                        if (matrix[i, j] == occupied || matrix[i, j] == emptyToOccupied) occupiedCounter++;
                    }
                }

                ChangeStates(matrix, occupied, empty, occupiedToEmpty, emptyToOccupied);

                finished = !stateChanged;
            }


            Console.WriteLine(occupiedCounter);
        }

        static void ChangeStates(char[,] matrix, char occupied, char empty,  char occupiedToEmpty, char emptyToOccupied)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == occupiedToEmpty) matrix[i, j] = empty;
                    if (matrix[i, j] == emptyToOccupied) matrix[i, j] = occupied;
                }
            }
        }

        public static char[] GetVisibleSeats(char[,] matrix, int row, int column, char floor)
        {
            List<char> result = new List<char>();

            var topLeft = GetTopLeft(matrix, row, column, floor);
            var topRight = GetTopRight(matrix, row, column, floor);
            var bottomLeft = GetBottomLeft(matrix, row, column, floor);
            var bottomRight = GetBottomRight(matrix, row, column, floor);
            var top = GetTop(matrix, row, column, floor);
            var left = GetLeft(matrix, row, column, floor);
            var right = GetRight(matrix, row, column, floor);
            var bottom = GetBottom(matrix, row, column, floor);

            if (topLeft != null) result.Add(topLeft.Value);
            if (topRight != null) result.Add(topRight.Value);
            if (bottomLeft != null) result.Add(bottomLeft.Value);
            if (bottomRight != null) result.Add(bottomRight.Value);
            if (top != null) result.Add(top.Value);
            if (left != null) result.Add(left.Value);
            if (right != null) result.Add(right.Value);
            if (bottom != null) result.Add(bottom.Value);

            return result.ToArray();
        }

        static char? GetTopLeft(char[,] matrix, int row, int column, char floor)
        {
            while (--row >= 0 && --column >= 0)
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }

        static char? GetTopRight(char[,] matrix, int row, int column, char floor)
        {
            while (--row >= 0 && ++column < matrix.GetLength(1))
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }

        static char? GetBottomLeft(char[,] matrix, int row, int column, char floor)
        {
            while (++row < matrix.GetLength(0) && --column >= 0)
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }

        static char? GetBottomRight(char[,] matrix, int row, int column, char floor)
        {
            while (++row < matrix.GetLength(0) && ++column < matrix.GetLength(1))
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }

        static char? GetTop(char[,] matrix, int row, int column, char floor)
        {
            while (--row >= 0)
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }

        static char? GetBottom(char[,] matrix, int row, int column, char floor)
        {
            while (++row < matrix.GetLength(0))
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }

        static char? GetLeft(char[,] matrix, int row, int column, char floor)
        {
            while (--column >= 0)
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }

        static char? GetRight(char[,] matrix, int row, int column, char floor)
        {
            while (++column < matrix.GetLength(1))
            {
                if (matrix[row, column] != floor) return matrix[row, column];
            }

            return null;
        }
    }
}
