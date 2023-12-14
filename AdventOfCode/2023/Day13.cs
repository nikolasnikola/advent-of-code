namespace AdventOfCode._2023
{
    internal static class Day13
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2023", "Day13");

            var fields = input.Split("\r\n\r\n").Select(x => GetCharMatrix(x));
            var result = 0;

            foreach(var field in fields) 
            {
                var vertical = GetVerticalReflectionIndex(field);
                var horizontal = GetHorizontalReflectionIndex(field);

                result += vertical + 100 * horizontal ;
            }

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsString("2023", "Day13");

            var fields = input.Split("\r\n\r\n").Select(x => GetCharMatrix(x));
            var result = 0;

            foreach (var field in fields)
            {
                var vertical = GetVerticalReflectionIndexWithSmudge(field);
                var horizontal = GetHorizontalReflectionIndexWithSmudge(field);

                result += vertical + 100 * horizontal;
            }

            Console.WriteLine(result);
        }

        private static int GetVerticalReflectionIndex(char[,] field)
        {
            var cols = field.GetLength(1);

            for (int i = 1; i < cols; i++)
            {
                var previousColumn = MatrixHelper.GetColumn(field, i - 1);
                var currentColumn = MatrixHelper.GetColumn(field, i);

                if (new string(previousColumn) == new string(currentColumn))
                {
                    var left = i - 2;
                    var right = i+1;
                    var isReflection = true;

                    while (left >=0 && right < cols)
                    {
                        if (new string(MatrixHelper.GetColumn(field, left)) != new string(MatrixHelper.GetColumn(field, right)))
                        {
                            isReflection = false;
                            break;
                        }
                        left--;
                        right++;
                    }

                    if (isReflection) return i;
                }
            }

            return 0;
        }

        private static int GetHorizontalReflectionIndex(char[,] field)
        {
            var rows = field.GetLength(0);
            for (int i = 1; i < rows; i++)
            {
                var previousRow = MatrixHelper.GetRow(field, i - 1);
                var currentRow = MatrixHelper.GetRow(field, i);

                if (new string(previousRow) == new string(currentRow))
                {
                    var top = i - 2;
                    var bottom = i + 1;
                    var isReflection = true;

                    while (top >= 0 && bottom < rows)
                    {
                        if (new string(MatrixHelper.GetRow(field, top)) != new string(MatrixHelper.GetRow(field, bottom)))
                        {
                            isReflection = false;
                            break;
                        }
                        top--;
                        bottom++;
                    }

                    if (isReflection) return i;
                }
            }

            return 0;
        }

        private static int GetVerticalReflectionIndexWithSmudge(char[,] field)
        {
            var cols = field.GetLength(1);

            for (int i = 1; i < cols; i++)
            {
                var previousColumn = MatrixHelper.GetColumn(field, i - 1);
                var currentColumn = MatrixHelper.GetColumn(field, i);

                var diff = GetCharArrayDifference(previousColumn, currentColumn);

                if (diff <= 1)
                {
                    var left = i - 2;
                    var right = i + 1;
                    var isReflection = true;

                    while (left >= 0 && right < cols)
                    {
                        diff += GetCharArrayDifference(MatrixHelper.GetColumn(field, left), MatrixHelper.GetColumn(field, right));
                        if (diff > 1)
                        {
                            isReflection = false;
                            break;
                        }
                        left--;
                        right++;
                    }

                    if (isReflection && diff == 1) return i;
                }
            }

            return 0;
        }

        private static int GetHorizontalReflectionIndexWithSmudge(char[,] field)
        {
            var rows = field.GetLength(0);
            for (int i = 1; i < rows; i++)
            {
                var previousRow = MatrixHelper.GetRow(field, i - 1);
                var currentRow = MatrixHelper.GetRow(field, i);

                var diff = GetCharArrayDifference(previousRow, currentRow);

                if (diff <= 1)
                {
                    var top = i - 2;
                    var bottom = i + 1;
                    var isReflection = true;

                    while (top >= 0 && bottom < rows)
                    {
                        diff += GetCharArrayDifference(MatrixHelper.GetRow(field, top), MatrixHelper.GetRow(field, bottom));
                        if (diff > 1)
                        {
                            isReflection = false;
                            break;
                        }
                        top--;
                        bottom++;
                    }

                    if (isReflection && diff == 1) return i;
                }
            }

            return 0;
        }

        private static int GetCharArrayDifference(char[] input1, char[] input2)
        {
            var diff = 0;

            for (int i = 0; i<input1.Length; i++)
            {
                if (input1[i] != input2[i]) diff++;
            }

            return diff;
        }

        private static char[,] GetCharMatrix(string input)
        {
            var lines = input.Split("\r\n");
            int i = 0;
            char[,] matrix = new char[lines.Count(), lines.First().Length];

            foreach (var line in lines)
            {
                int j = 0;
                foreach (var c in line)
                {
                    matrix[i, j] = c;
                    j++;
                }
                i++;
            }

            return matrix;
        }
    }
}