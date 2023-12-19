using System.Text.RegularExpressions;

namespace AdventOfCode._2023
{
    internal static class Day18
    {
        internal static void Part1()
        {
            var digPlan = Reader.ReadAsStringList("2023", "Day18");

            var positions = new List<(long, long)>();
            (long,long) startPosition = (0, 0);
            var lastPosition = startPosition;
            positions.Add(startPosition);
            long perimeter = 0;

            foreach (var item in digPlan)
            {
                var instruction = item.Split(' ');
                var newPosition = ProceedInstruction(positions, instruction[0], long.Parse(instruction[1]), lastPosition);
                perimeter += Math.Abs(newPosition.Item1 + newPosition.Item2 - (lastPosition.Item1 + lastPosition.Item2));
                lastPosition = newPosition;
            }

            var result = CalculatePolygonArea(positions, perimeter);

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var digPlan = Reader.ReadAsStringList("2023", "Day18");

            var positions = new List<(long, long)>();
            (long, long) startPosition = (0, 0);
            var lastPosition = startPosition;
            positions.Add(startPosition);
            long perimeter = 0;

            foreach (var item in digPlan)
            {
                var instruction = item.Split(' ')[2];
                var match = Regex.Match(instruction, @"\(#(\S+)\)");
                var value = match.Groups[1].Value;
                var meters = long.Parse(value[..5], System.Globalization.NumberStyles.HexNumber);
                var direction = GetDirection(value[5..]);

                var newPosition = ProceedInstruction(positions, direction, meters, lastPosition);
                perimeter += Math.Abs(newPosition.Item1 + newPosition.Item2 - (lastPosition.Item1 + lastPosition.Item2));
                lastPosition = newPosition;
            }

            var result = CalculatePolygonArea(positions, perimeter);

            Console.WriteLine(result);
        }

        private static string GetDirection(string value) =>
            value switch
            {
                "0" => "R",
                "1" => "D",
                "2" => "L",
                _ => "U"
            };

        private static (long, long) ProceedInstruction(List<(long, long)> positions, string direciton, long meters, (long, long) lastItem)
        {
            var newItem = lastItem;
            if (direciton == "R") newItem = (lastItem.Item1, lastItem.Item2 + meters);
            else if (direciton == "L") newItem = (lastItem.Item1, lastItem.Item2 - meters);
            else if (direciton == "D") newItem = (lastItem.Item1 + meters, lastItem.Item2);
            else if (direciton == "U") newItem = (lastItem.Item1 - meters, lastItem.Item2);

            positions.Add(newItem);
            return newItem;
        }

        private static long CalculatePolygonArea(List<(long, long)> points, long perimeter)
        {
            int n = points.Count;

            long sum = 0;

            for (int i = 0; i < n - 1; i++)
            {
                sum += (points[i].Item1 * points[i + 1].Item2) - (points[i + 1].Item1 * points[i].Item2);
            }

            long result = Math.Abs((sum) / 2) + perimeter / 2 + 1;

            return result;
        }
    }
}