namespace AdventOfCode._2023
{
    internal static class Day14
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsCharMatrix("2023", "Day14");
            var result = 0;

            for (int i = 0; i < input.GetLength(1); i++) 
            { 
                var column = MatrixHelper.GetColumn(input, i);
                var cubeRocks = column.IndexOfAll('#');

                var roundedRocks = column.IndexOfAll('O');

                if (cubeRocks.Any() && roundedRocks.Any())
                {
                    if (cubeRocks.First() > 0)
                    {
                        var firstSegment = roundedRocks.Where(x => x >= 0 && x < cubeRocks.First());
                        result += GetResultForSegment(0, column.Length, firstSegment.Count());
                    }

                    for (int j = 0; j < cubeRocks.Length - 1; j++)
                    {
                        var segment = roundedRocks.Where(x => x >= cubeRocks[j] && x <= cubeRocks[j + 1]);
                        result += GetResultForSegment(cubeRocks[j] + 1, column.Length, segment.Count());
                    }

                    if (cubeRocks.Last() < column.Length - 1)
                    {
                        var lastSegment = roundedRocks.Where(x => x > cubeRocks.Last() && x < column.Length);
                        result += GetResultForSegment(cubeRocks.Last() + 1, column.Length, lastSegment.Count());
                    }
                }
                else
                {
                    result += GetResultForSegment(0, column.Length, roundedRocks.Count());
                }
                
            }

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsCharMatrix("2023", "Day14");

            var memo = new List<string>() { GetStringValue(input) };

            var isSame = false;

            long index = 0;
            int repeatIndex = 0;

            while (!isSame)
            {
                MoveNorth(input);
                MoveWest(input);
                MoveSouth(input);
                MoveEast(input);
                index++;

                var newString = GetStringValue(input);

                if (memo.Contains(newString))
                {
                    isSame = true;
                    repeatIndex = memo.IndexOf(newString);
                }
                else memo.Add(newString);
            }

            long toCompare = (1000000000 - repeatIndex) % (memo.Count - repeatIndex) + repeatIndex; 

            var valid = memo[(int)toCompare];
            int result = GetResult(valid);

            Console.WriteLine(result);
        }

        private static int GetResult(string valid)
        {
            var validState = GetCharMatrix(valid);

            var result = 0;

            for (int i = 0; i < validState.GetLength(1); i++)
            {
                var column = MatrixHelper.GetColumn(validState, i);
                var roundedRocks = column.IndexOfAll('O');

                result += roundedRocks.Select(x => column.Length - x).Sum();
            }

            return result;
        }

        private static char[,] GetCharMatrix(string input)
        {
            var lines = input.Split("\r\n");
            int i = 0;
            char[,] matrix = new char[lines.Length, lines.First().Length];

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

        private static string GetStringValue(char[,] map)
        {
            var items = new List<string>();

            for (int i = 0; i< map.GetLength(0); i++)
            {
                items.Add(new string(MatrixHelper.GetRow(map, i)));
            }

            return string.Join("\r\n", items);
        }

        private static void MoveNorth(char[,] map)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                var column = MatrixHelper.GetColumn(map, i);
                var cubeRocks = column.IndexOfAll('#');

                var roundedRocks = column.IndexOfAll('O');

                if (cubeRocks.Any() && roundedRocks.Any())
                {
                    if (cubeRocks.First() > 0)
                    {
                        var firstSegment = roundedRocks.Where(x => x >= 0 && x < cubeRocks.First());

                        foreach (var c in Enumerable.Range(0, cubeRocks.First()))
                        {
                            map[c, i] = c < firstSegment.Count() ? 'O' : '.';
                        }
                    }

                    for (int j = 0; j < cubeRocks.Length - 1; j++)
                    {
                        var segment = roundedRocks.Where(x => x > cubeRocks[j] && x < cubeRocks[j + 1]);

                        var from = cubeRocks[j] + 1;
                        var count = cubeRocks[j + 1] - cubeRocks[j] - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[c, i] = c-from < segment.Count() ? 'O' : '.';
                        }
                    }

                    if (cubeRocks.Last() < column.Length - 1)
                    {
                        var lastSegment = roundedRocks.Where(x => x > cubeRocks.Last() && x < column.Length);

                        var from = cubeRocks.Last() + 1;
                        var count = column.Length - cubeRocks.Last() - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[c, i] = c - from < lastSegment.Count() ? 'O' : '.';
                        }
                    }
                }
                else if (roundedRocks.Any())
                {
                    foreach (var c in Enumerable.Range(0, column.Length))
                    {
                        map[c, i] = c < roundedRocks.Length ? 'O' : '.';
                    }
                }

            }
        }

        private static void MoveSouth(char[,] map)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                var column = MatrixHelper.GetColumn(map, i);
                var cubeRocks = column.IndexOfAll('#');

                var roundedRocks = column.IndexOfAll('O');

                if (cubeRocks.Any() && roundedRocks.Any())
                {
                    if (cubeRocks.Last() < column.Length - 1)
                    {
                        var lastSegment = roundedRocks.Where(x => x > cubeRocks.Last() && x < column.Length);

                        var from = cubeRocks.Last() + 1;
                        var count = column.Length - cubeRocks.Last() - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[c, i] = c >= from + count - lastSegment.Count() ? 'O' : '.';
                        }
                    }

                    for (int j = 0; j < cubeRocks.Length - 1; j++)
                    {
                        var segment = roundedRocks.Where(x => x > cubeRocks[j] && x < cubeRocks[j + 1]);

                        var from = cubeRocks[j] + 1;
                        var count = cubeRocks[j + 1] - cubeRocks[j] - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[c, i] = c >= from + count - segment.Count() ? 'O' : '.';
                        }
                    }

                    if (cubeRocks.First() > 0)
                    {
                        var firstSegment = roundedRocks.Where(x => x >= 0 && x < cubeRocks.First());

                        foreach (var c in Enumerable.Range(0, cubeRocks.First()))
                        {
                            map[c, i] = c >=  cubeRocks.First() - firstSegment.Count() ? 'O' : '.';
                        }
                    }
                }
                else if (roundedRocks.Any())
                {
                    foreach (var c in Enumerable.Range(0, column.Length))
                    {
                        map[c, i] = c >= column.Length - roundedRocks.Length ? 'O' : '.';
                    }
                }

            }
        }

        private static void MoveWest(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                var row = MatrixHelper.GetRow(map, i);
                var cubeRocks = row.IndexOfAll('#');

                var roundedRocks = row.IndexOfAll('O');

                if (cubeRocks.Any() && roundedRocks.Any())
                {
                    if (cubeRocks.First() > 0)
                    {
                        var firstSegment = roundedRocks.Where(x => x >= 0 && x < cubeRocks.First());

                        foreach (var c in Enumerable.Range(0, cubeRocks.First()))
                        {
                            map[i, c] = c < firstSegment.Count() ? 'O' : '.';
                        }
                    }

                    for (int j = 0; j < cubeRocks.Length - 1; j++)
                    {
                        var segment = roundedRocks.Where(x => x > cubeRocks[j] && x < cubeRocks[j + 1]);

                        var from = cubeRocks[j] + 1;
                        var count = cubeRocks[j + 1] - cubeRocks[j] - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[i, c] = c - from < segment.Count() ? 'O' : '.';
                        }
                    }

                    if (cubeRocks.Last() < row.Length - 1)
                    {
                        var lastSegment = roundedRocks.Where(x => x > cubeRocks.Last() && x < row.Length);

                        var from = cubeRocks.Last() + 1;
                        var count = row.Length - cubeRocks.Last() - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[i, c] = c - from < lastSegment.Count() ? 'O' : '.';
                        }
                    }
                }
                else if (roundedRocks.Any())
                {
                    foreach (var c in Enumerable.Range(0, row.Length))
                    {
                        map[i, c] = c < roundedRocks.Length ? 'O' : '.';
                    }
                }

            }
        }

        private static void MoveEast(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                var row = MatrixHelper.GetRow(map, i);
                var cubeRocks = row.IndexOfAll('#');

                var roundedRocks = row.IndexOfAll('O');

                if (cubeRocks.Any() && roundedRocks.Any())
                {
                    if (cubeRocks.Last() < row.Length - 1)
                    {
                        var lastSegment = roundedRocks.Where(x => x > cubeRocks.Last() && x < row.Length);

                        var from = cubeRocks.Last() + 1;
                        var count = row.Length - cubeRocks.Last() - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[i, c] = c >= from + count - lastSegment.Count() ? 'O' : '.';
                        }
                    }

                    for (int j = 0; j < cubeRocks.Length - 1; j++)
                    {
                        var segment = roundedRocks.Where(x => x > cubeRocks[j] && x < cubeRocks[j + 1]);

                        var from = cubeRocks[j] + 1;
                        var count = cubeRocks[j + 1] - cubeRocks[j] - 1;

                        foreach (var c in Enumerable.Range(from, count))
                        {
                            map[i, c] = c >= from + count - segment.Count() ? 'O' : '.';
                        }
                    }

                    if (cubeRocks.First() > 0)
                    {
                        var firstSegment = roundedRocks.Where(x => x >= 0 && x < cubeRocks.First());

                        foreach (var c in Enumerable.Range(0, cubeRocks.First()))
                        {
                            map[i, c] = c >= cubeRocks.First() - firstSegment.Count() ? 'O' : '.';
                        }
                    }
                }
                else if (roundedRocks.Any())
                {
                    foreach (var c in Enumerable.Range(0, row.Length))
                    {
                        map[i, c] = c >= row.Length - roundedRocks.Length ? 'O' : '.';
                    }
                }

            }
        }

        private static int GetResultForSegment(int segmentIndex, int length, int numberOfItems)
        {
            return Enumerable.Range(length - segmentIndex - numberOfItems+1, numberOfItems).Sum();
        }

        private static int[] IndexOfAll(this char[] source, char target)
        {
            return source.Select((c, idx) => c == target ? idx : -1).Where(idx => idx != -1).ToArray();
        }
    }
}