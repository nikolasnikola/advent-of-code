namespace AdventOfCode._2023
{
    internal static class Day11
    {
        internal static void Part1()
        {
            var matrix = Reader.ReadAsCharMatrix("2023", "Day11");
            double totalDistance = CalculateTotalDistance(matrix, 1);

            Console.WriteLine(totalDistance);
        }

        internal static void Part2()
        {
            var matrix = Reader.ReadAsCharMatrix("2023", "Day11");
            double totalDistance = CalculateTotalDistance(matrix, 1000000);

            Console.WriteLine(totalDistance);
        }

        private static double CalculateTotalDistance(char[,] matrix, int emptySpaceIndex)
        {
            var emptyRows = new List<int>();

            var galaxies = new List<(int, int)>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var isEmptyRow = true;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == '#')
                    {
                        isEmptyRow = false;
                        galaxies.Add((i, j));
                    }
                }

                if (isEmptyRow) emptyRows.Add(i);
            }

            var emptyColumns = Enumerable.Range(0, matrix.GetLength(1) - 1).Except(galaxies.Select(g => g.Item2)).ToList();

            double totalDistance = 0;

            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    var ga = galaxies[i];
                    var gb = galaxies[j];

                    var rowsBetween = Math.Abs(ga.Item1 - gb.Item1);
                    var colsBetween = Math.Abs(ga.Item2 - gb.Item2);

                    var emptyRowsBetween = emptyRows.Count(x => x > Math.Min(ga.Item1, gb.Item1) && x < Math.Max(ga.Item1, gb.Item1));
                    var emptyColumnsBetween = emptyColumns.Count(x => x > Math.Min(ga.Item2, gb.Item2) && x < Math.Max(ga.Item2, gb.Item2));

                    var index = Math.Max(1, emptySpaceIndex - 1);

                    totalDistance += rowsBetween + colsBetween + emptyRowsBetween*index + emptyColumnsBetween*index;
                }
            }

            return totalDistance;
        }
    }
}