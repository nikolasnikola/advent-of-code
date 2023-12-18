using AdventOfCode._2020;

namespace AdventOfCode._2023
{
    internal static class Day18
    {
        internal static void Part1()
        {
            var digPlan = Reader.ReadAsStringList("2023", "Day18Test");

            var positions = new List<(int, int)>();
            var startPosition = (0, 0);
            var lastPosition = startPosition;
            positions.Add(startPosition);
            foreach (var item in digPlan)
            {
                var instruction = item.Split(' ');
                lastPosition = ProceedInstruction(positions, instruction[0], instruction[1], lastPosition);
            }

            if (lastPosition != startPosition) {Console.WriteLine("POLYGON NOT CLOSED"); }

            //var boundings = GetBoundings(positions);

            var result = CalculatePolygonArea(positions);

            // Example vertices of the polygon
            double[] x = { 0, 0, 5, 5, 7, 7, 9, 9, 7, 7, 5, 5, 2, 2, 0 };
            double[] y = { 0, 6, 6, 4, 4, 6, 6, 1, 1, 0, 0, 2, 2, 0, 0 };

            double area = CalculatePolygonArea(x, y);

            Console.WriteLine("Area of the polygon: " + area);

            //for (int k = boundings.Item1; k <= boundings.Item2; k++)
            //    for (int l = boundings.Item3; l <= boundings.Item4; l++)
            //        if (IsPointInside((k, l), positions)) result++;

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var matrix = Reader.ReadAsCharMatrix("2023", "Day18Test");


            Console.WriteLine();
        }

        private static (int,int) ProceedInstruction(List<(int, int)> positions, string direciton, string meters, (int, int) lastItem)
        {
            var newItem = lastItem;
            if (direciton == "R") newItem = (lastItem.Item1, lastItem.Item2 + int.Parse(meters));
            else if (direciton == "L") newItem = (lastItem.Item1, lastItem.Item2 - int.Parse(meters));
            else if (direciton == "D") newItem = (lastItem.Item1 + int.Parse(meters), lastItem.Item2);
            else if (direciton == "U") newItem = (lastItem.Item1 - int.Parse(meters), lastItem.Item2);

            positions.Add(newItem);
            return newItem;
        }

        static double CalculatePolygonArea(double[] x, double[] y)
        {
            int n = x.Length;

            if (n != y.Length || n < 3)
            {
                throw new ArgumentException("Invalid number of vertices");
            }

            double sum = 0;

            for (int i = 0; i < n - 1; i++)
            {
                sum += (x[i] * y[i + 1]) - (x[i + 1] * y[i]);
            }

            // Include the last vertex (n-1) and the first vertex (0) for the last iteration
            sum += (x[n - 1] * y[0]) - (x[0] * y[n - 1]);

            double result = Math.Abs(sum) / 2.0;

            return result;
        }

        private static long CalculatePolygonArea(List<(int, int)> points)
        {
            int n = points.Count;

            long sum = 0;

            for (int i = 0; i < n - 1; i++)
            {
                sum += (points[i].Item1 * points[i + 1].Item2) - (points[i + 1].Item1 * points[i].Item2);
            }

            sum += (points[n - 1].Item1 * points[0].Item2) - (points[0].Item1 * points[n - 1].Item2);

            // Include the last vertex (n-1) and the first vertex (0) for the last iteration
            //sum1 += x[n - 1] * y[0];
            //sum2 += y[n - 1] * x[0];

            long result = Math.Abs((sum) / 2);

            return result;
        }
        private static bool IsPointInside((int, int) point, List<(int, int)> area)
        {
            bool isInRing = false;
            int j = area.Count - 1;

            for (int i = 0; i < area.Count; i++)
            {
                if (IsPositionOnLine(point, (area[i].Item1, area[i].Item2), (area[j].Item1, area[j].Item2)))
                {
                    return true;
                }

                if ((area[i].Item2 < point.Item2 && area[j].Item2 >= point.Item2)
                    || (area[j].Item2 < point.Item2 && area[i].Item2 >= point.Item2))
                {
                    if (area[i].Item1
                        + ((point.Item2 - area[i].Item2) / (area[j].Item2 - area[i].Item2) * (area[j].Item1 - area[i].Item1))
                        < point.Item1)
                    {
                        isInRing = !isInRing;
                    }
                }

                j = i;
            }

            return isInRing;
        }

        private static bool IsPositionOnLine((int, int) point, (int, int) lineStart, (int, int) lineEnd)
        {
            if (point.Item2 < Math.Min(lineStart.Item2, lineEnd.Item2)
                || point.Item2 > Math.Max(lineStart.Item2, lineEnd.Item2)
                || point.Item1 < Math.Min(lineStart.Item1, lineEnd.Item1)
                || point.Item1 > Math.Max(lineStart.Item1, lineEnd.Item1))
            {
                return false;
            }

            if (lineStart.Item1 == point.Item1)
            {
                return lineEnd.Item1 == point.Item1;
            }

            if (lineStart.Item2 == point.Item2)
            {
                return lineEnd.Item2 == point.Item2;
            }

            return (point.Item1 - lineStart.Item1) / (lineEnd.Item1 - lineStart.Item1)
                == (point.Item2 - lineStart.Item2) / (lineEnd.Item2 - lineStart.Item2);
        }

        private static (int, int, int, int) GetBoundings(IEnumerable<(int,int)> positions)
        {
            var result = positions.Aggregate(
                  new
                  {
                      MinRow = int.MaxValue,
                      MaxRow = int.MinValue,
                      MinCol = int.MaxValue,
                      MaxCol = int.MinValue,
                  },
                  (accumulator, p) => new
                  {
                      MinRow = Math.Min(p.Item1, accumulator.MinRow),
                      MaxRow = Math.Max(p.Item1, accumulator.MaxRow),
                      MinCol = Math.Min(p.Item2, accumulator.MinCol),
                      MaxCol = Math.Max(p.Item2, accumulator.MaxCol),
                  });

            return (result.MinRow, result.MaxRow, result.MinCol, result.MaxCol);
        }
    }
}