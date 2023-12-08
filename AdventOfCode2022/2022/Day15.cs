using AdventOfCode.Models.Day14;

namespace AdventOfCode._2022
{
    internal static class Day15
    {
        internal static void Part2(int limit)
        {
            var sensorsData = Reader.ReadAsStringList("2022", "Day15");

            var sensorBeacon = sensorsData.Select(sd => GetSensorBeaconPoints(sd)).ToList();

            var sensorsWithDistances = sensorBeacon.Select(sb => (sb.Item1, Math.Abs(sb.Item1.X - sb.Item2.X) + Math.Abs(sb.Item1.Y - sb.Item2.Y)));

            var missingPoint = FindUndetectedBeacon(sensorsWithDistances, limit);

            Console.WriteLine("\n**************");
            Console.WriteLine("Undetected Bacon Score: " + (long)missingPoint.X * 4000000 + (long)missingPoint.Y);
            Console.WriteLine("**************");
        }

        internal static void Part1(int resultRow)
        {
            var sensorsData = Reader.ReadAsStringList("2022", "Day15Test");

            var sensorBeacon = sensorsData.Select(sd => GetSensorBeaconPoints(sd)).ToList();

            var allSbs = sensorBeacon.Select(x => x.Item1).Union(sensorBeacon.Select(x => x.Item2)).ToList(); ;

            (int maxX, int minX, int maxY, int minY) = Point.GetBoundings(allSbs);
            int maxDistance = GetMaxDistance(sensorBeacon);

            var colBoundings = maxX - minX;
            var rowBoundings = maxY - minY;

            minX -= maxDistance;
            maxX += maxDistance;
            minY -= maxDistance;
            maxY += maxDistance;

            var rows = maxY - minY + 1;
            var cols = maxX - minX + 1;

            //var map = InitializeEmptyMap(rows, cols, minX, minY, sensorBeacon);

            bool spaceBetweenCols = true;
            int rowsSpaceIndex = spaceBetweenCols ? 2 : 1;

            //PrintMap(map, true, 0);

            var result = 0;
            List<(int, int)> alreadyChecked = new List<(int, int)>();
            int sensorsChecked = 0;

            foreach (var sb in sensorBeacon)
            {
                Console.WriteLine($"\nChecking sensor: {++sensorsChecked} of {sensorBeacon.Count}...");
                result += GetNoBeaconPoints(sb, resultRow, alreadyChecked, allSbs);
                Console.Write($" => : {result}");
            }

            Console.WriteLine("\n**************");
            Console.WriteLine("Total positions without beacon: " + alreadyChecked.Distinct().Count());
            Console.WriteLine("**************");

            //var result = GetResultForRow(map, resultRow, minY);

            //PrintMap(map, true, result);
        }

        static Point FindUndetectedBeacon(IEnumerable<(Point, int)> sensorsWithDistances, int limit)
        {
            foreach (var sensor in sensorsWithDistances)
            {
                var currentPoint = sensor.Item1;
                var distance = sensor.Item2;

                var start = currentPoint.Y - distance - 1;
                var end = currentPoint.Y + distance + 1;

                var x = 0;

                for (int i = start; i <= end; i++)
                {
                    var left = new Point(currentPoint.X + x, i);
                    var right = new Point(currentPoint.X - x, i);

                    if (IsNotVisibleByAnotherSensor(sensorsWithDistances, left, limit))
                    {
                        return left;
                    }
                    if (IsNotVisibleByAnotherSensor(sensorsWithDistances, right, limit))
                    {
                        return right;
                    }

                    x = i < currentPoint.Y ? x + 1 : x - 1;
                }
            }

            return new Point(-1, -1);
        }

        static bool IsNotVisibleByAnotherSensor(IEnumerable<(Point, int)> sensorsWithDistance, Point point, int limit)
        {
            if (point.X < 0 || point.X > limit || point.Y < 0 || point.Y > limit)
            {
                return false;
            }

            foreach (var sensor in sensorsWithDistance)
            {
                var distanceB = Math.Abs(sensor.Item1.X - point.X) + Math.Abs(sensor.Item1.Y - point.Y);
                if (sensor.Item2 >= distanceB)
                {
                    return false;
                }
            }
            return true;
        }

        static int GetNoBeaconPoints((Point, Point) sb, int targetRow, List<(int, int)> alreadyChecked, List<Point> allSbs)
        {
            var distance = Math.Abs(sb.Item1.X - sb.Item2.X) + Math.Abs(sb.Item1.Y - sb.Item2.Y);

            var currentPoint = sb.Item1;

            var points = GetPointsForRow(currentPoint, distance, targetRow);

            var enumerablePoints = points.Where(p =>
                        !(p.Item1 == currentPoint.X && p.Item2 == currentPoint.Y)
                        //&& !alreadyChecked.Contains((p.Item1, p.Item2))
                        && targetRow == p.Item2
                        && !allSbs.Contains(new Point(p.Item1, p.Item2)))
                .ToList();

            alreadyChecked.AddRange(enumerablePoints);

            return enumerablePoints.Count;
        }

        static List<(int, int)> GetPointsForRow(Point s, int distance, int targetRow)
        {
            var result = new List<(int, int)>();

            for (int x = s.X - distance; x <= s.X + distance; x++)
            {
                if (Math.Abs(s.X - x) + Math.Abs(s.Y - targetRow) <= distance)
                {
                    result.Add((x, targetRow));
                }
            }

            return result;
        }


        static char[,] InitializeEmptyMap(int rows, int cols, int minX, int minY, IEnumerable<(Point, Point)> sensorBeacon)
        {
            var map = new char[rows, cols];

            for (int i = 0; i < rows * cols; i++) map[i / cols, i % cols] = '.';

            foreach (var sb in sensorBeacon)
            {
                var sensorC = sb.Item1.X - minX;
                var sensorR = sb.Item1.Y - minY;

                var beaconC = sb.Item2.X - minX;
                var beaconR = sb.Item2.Y - minY;

                map[sensorR, sensorC] = 'S';
                map[beaconR, beaconC] = 'B';
            }

            return map;
        }

        static (Point, Point) GetSensorBeaconPoints(string sensorRow)
        {
            var splitted = sensorRow.Split(": ");

            var sensorCoordinates = splitted[0][10..];
            var sensorXY = sensorCoordinates.Split(", ");
            var sensorX = int.Parse(sensorXY[0][2..]);
            var sensorY = int.Parse(sensorXY[1][2..]);
            var sensor = new Point(sensorX, sensorY);

            var beaconCoordinates = splitted[1][21..];
            var beaconXY = beaconCoordinates.Split(", ");
            var beaconX = int.Parse(beaconXY[0][2..]);
            var beaconY = int.Parse(beaconXY[1][2..]);
            var beacon = new Point(beaconX, beaconY);

            return (sensor, beacon);
        }

        static int GetMaxDistance(IEnumerable<(Point, Point)> sbPairs)
        {
            var result = sbPairs.Aggregate(
                  new
                  {
                      Max = int.MinValue,
                  },
                  (accumulator, p) => new
                  {
                      Max = Math.Max(Math.Abs(p.Item1.X - p.Item2.X) + Math.Abs(p.Item1.Y - p.Item2.Y), accumulator.Max),
                  });

            return result.Max;
        }

        static void UpdateMap(int row, int column, string item)
        {
            Console.SetCursorPosition(column, row);
            Console.Write(item);
            Thread.Sleep(10);
        }

        static void PrintMap(char[,] map, bool spaceBetweenRows, int result)
        {
            Console.SetCursorPosition(0, 0);

            var betweenRows = spaceBetweenRows ? " " : string.Empty;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '#')
                    {
                        Console.Write("\x1b[32m" + map[i, j] + "\x1b[0m" + betweenRows);
                    }
                    else
                    {
                        Console.Write(map[i, j] + betweenRows);
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n**************");
            Console.WriteLine("Total positions without beacon: " + result);
            Console.WriteLine("**************");

            //Thread.Sleep(10);
        }
    }
}
