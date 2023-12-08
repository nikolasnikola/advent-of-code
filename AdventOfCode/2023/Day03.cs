using System.Data.Common;

namespace AdventOfCode._2023
{
    internal static class Day03
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsCharMatrix("2023", "Day03");

            List<int> partsToSum = new();

            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0;  j < input.GetLength(1); j++)
                {
                    if (char.IsDigit(input[i, j]))
                    {
                        var part = GetEnginePartForCell(i, j, input);
                        var neighbors = GetNeighborsForPart(input, part);
                        if (neighbors.Any(n => IsSymbol(input[n.Item1, n.Item2]))) partsToSum.Add(part.PartNumber);
                        j = part.EndCol;
                    }
                }
            }

            Console.WriteLine(partsToSum.Sum());
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsCharMatrix("2023", "Day03");

            var possibleGears = new Dictionary<(int, int), List<EnginePart>>();

            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    if (char.IsDigit(input[i, j]))
                    {
                        var part = GetEnginePartForCell(i, j, input);
                        var neighbors = GetNeighborsForPart(input, part);

                        foreach (var neighbor in neighbors)
                        {
                            if (input[neighbor.Item1, neighbor.Item2] == '*')
                            {
                                if (!possibleGears.ContainsKey((neighbor.Item1, neighbor.Item2)))
                                    possibleGears.Add((neighbor.Item1, neighbor.Item2), new List<EnginePart>());
                                possibleGears[(neighbor.Item1, neighbor.Item2)].Add(part);
                            }
                        }

                        j = part.EndCol;
                    }
                }
            }

            var result = possibleGears.Where(g => g.Value.Count == 2).Select(g => g.Value[0].PartNumber * g.Value[1].PartNumber).Sum();

            Console.WriteLine(result);
        } 

        private static EnginePart GetEnginePartForCell(int row, int col, char[,] schema)
        {
            var enginePart = new EnginePart() { StartCol = col, Row = row };
            List<int> list = new List<int>
            {
                int.Parse(schema[row, col++].ToString())
            };

            var done = false;

            while(!done)
            {
                if (col >= schema.GetLength(1) || !char.IsDigit(schema[row, col])) done = true;
                else list.Add(int.Parse(schema[row, col++].ToString()));
            }

            int total = 0;
            foreach (int entry in list)
            {
                total = 10 * total + entry;
            }

            enginePart.EndCol = col - 1;
            enginePart.PartNumber = total;

            return enginePart;
        }

        private static (int, int)[] GetNeighborsForPart(char[,] schema, EnginePart part)
        {
            var result = new List<(int, int)>();
            int rowMinimum = Math.Max(part.Row - 1, 0);
            int rowMaximum = Math.Min(part.Row + 1, schema.GetLength(0) - 1);
            int columnMinimum = Math.Max(part.StartCol - 1, 0);
            int columnMaximum = Math.Min(part.EndCol + 1, schema.GetLength(1) - 1);
            for (int i = rowMinimum; i <= rowMaximum; i++)
                for (int j = columnMinimum; j <= columnMaximum; j++)
                    if (i != part.Row || j < part.StartCol || j > part.EndCol)
                        result.Add((i,j));
            return result.ToArray();
        }

        private static bool IsSymbol(char c) => !char.IsDigit(c) && c != '.';
    }

    internal class EnginePart
    {
        public int PartNumber { get; set; }
        public int Row { get; set; }
        public int StartCol { get; set; }
        public int EndCol { get; set; }
    }
}
