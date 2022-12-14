using AdventOfCode2022.Dto.Day14;

namespace AdventOfCode2022._2022
{
    internal static class Day14
    {
        internal static void Part2()
        {
            var rockPaths = Reader.ReadAsStringList("Day14");

            var paths = rockPaths.Select(rp => rp.Split(" -> "));

            var points = paths
                .Select(path => path.Select(point =>
                {
                    var coordinates = point.Split(",");
                    return new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
                })
                .ToList());

            var startingPosition = new Point(500, 0);

            var allPointsIncludingStart = new List<Point>() { startingPosition };
            allPointsIncludingStart.AddRange(points.SelectMany(x => x));

            (int maxX, int minX, int maxY, int minY) boundings = GetBoundings(allPointsIncludingStart);

            var colBoundings = boundings.maxX - boundings.minX;

            boundings.minX -= colBoundings*2;
            boundings.maxX += colBoundings*2;

            var rows = boundings.maxY - boundings.minY + 3;
            var cols = (boundings.maxX - boundings.minX) + 1;

            var allRocks = points.SelectMany(pts => GetAllBetweenBoundings(pts));

            var map = InitializeEmptyMap(rows, cols, startingPosition, boundings.minX, boundings.minY, allRocks, true);
            SimulateSandFalling(startingPosition, boundings, map, false);
        }

        internal static void Part1()
        {
            var rockPaths = Reader.ReadAsStringList("Day14");

            var paths = rockPaths.Select(rp => rp.Split(" -> "));

            var points = paths
                .Select(path => path.Select(point =>
                {
                    var coordinates = point.Split(",");
                    return new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
                })
                .ToList());

            var startingPosition = new Point(500, 0);

            var allPointsIncludingStart = new List<Point>() { startingPosition };
            allPointsIncludingStart.AddRange(points.SelectMany(x => x));

            (int maxX, int minX, int maxY, int minY) boundings = GetBoundings(allPointsIncludingStart);

            var rows = boundings.maxY - boundings.minY + 1;
            var cols = boundings.maxX - boundings.minX + 1;

            var allRocks = points.SelectMany(pts => GetAllBetweenBoundings(pts));

            var map = InitializeEmptyMap(rows, cols, startingPosition, boundings.minX, boundings.minY, allRocks, false);
            SimulateSandFalling(startingPosition, boundings, map, true);
        }

        private static void SimulateSandFalling(Point startingPosition, (int maxX, int minX, int maxY, int minY) boundings, char[,] map, bool spaceBetweenRows)
        {
            PrintMap(map, 0, -1, -1, spaceBetweenRows);

            var totalSandUnits = 0;

            bool canThrowSand = true;

            while (canThrowSand)
            {
                canThrowSand = ThrowSandUnit(map, startingPosition, boundings.minX, boundings.minY, totalSandUnits, spaceBetweenRows);
                totalSandUnits++;
            }
        }

        static bool ThrowSandUnit(char[,] map, Point startingPosition, int minX, int minY, int totalSandUnits, bool spaceBetweenRows)
        {
            var currentC = startingPosition.X - minX;
            var currentR = startingPosition.Y - minY;

            var canMove = true;
            var moved = false;
            var isCurrentStart = true;
            var depth = 0;

            var rowsSpaceIndex = spaceBetweenRows ? 2 : 1;

            while (canMove)
            {
                var move = MoveSandUnit(map, currentR, currentC, isCurrentStart, rowsSpaceIndex);

                if (move.Item4)
                {
                    PrintMap(map, totalSandUnits, -1, -1, spaceBetweenRows);
                    return false;
                }

                canMove = move.Item1;

                if (canMove)
                {
                    moved = true;
                    currentR = move.Item2;
                    currentC= move.Item3;
                }

                // PrintMap(map, totalSandUnits, currentR, currentC);
                isCurrentStart = false;
                depth++;
            }

            if (!moved && depth == 1)
            {
                map[currentR, currentC] = 'o';
                PrintMap(map, ++totalSandUnits, -1, -1, spaceBetweenRows);
            }

            return moved;
        }

        static (bool, int, int, bool) MoveSandUnit(char[,] map, int currentR, int currentC, bool isCurrentStart, int rowsSpaceIndex)
        {
            var movedDown = MoveDown(map, currentR, currentC, isCurrentStart, rowsSpaceIndex);

            if (movedDown.Item1)
            {
                return movedDown;
            }
            else
            {
                var movedDownLeft = MoveDownLeft(map, currentR, currentC, isCurrentStart, rowsSpaceIndex);
                if (movedDownLeft.Item1)
                {
                    return movedDownLeft;
                }
                else
                {
                    var movedDownRight = MoveDownRight(map, currentR, currentC, isCurrentStart, rowsSpaceIndex);
                    if (movedDownRight.Item1) return movedDownRight;
                }
            }

            return (false, currentR, currentC, false);
        }

        static (bool, int, int, bool) MoveDown(char[,] map, int currentR, int currentC, bool isCurrentStart, int rowsSpaceIndex)
        {
            var totalRows = map.GetLength(0);
            int downR = currentR + 1;

            if (downR < totalRows)
            {
                if (map[downR, currentC] == '.')
                {
                    map[currentR, currentC] = isCurrentStart ? '+' : '.';
                    map[downR, currentC] = 'o';

                    if (!isCurrentStart) UpdateMap(currentR, currentC * rowsSpaceIndex, ".");
                    UpdateMap(downR, currentC * rowsSpaceIndex, "\x1b[93mo\x1b[0m");

                    return (true, downR, currentC, false);
                }
            }
            else
            {
                map[currentR, currentC] = isCurrentStart ? '+' : '.';
                return (true, currentR, currentC, true);
            }

            return (false, currentR, currentC, false);
        }

        static (bool, int, int, bool) MoveDownLeft(char[,] map, int currentR, int currentC, bool isCurrentStart, int rowsSapceIndex)
        {
            var totalRows = map.GetLength(0);

            int downR = currentR + 1;
            int leftC = currentC - 1;

            if (downR < totalRows && leftC >= 0)
            {
                if (map[downR, leftC] == '.')
                {
                    map[currentR, currentC] = isCurrentStart ? '+' : '.';
                    map[downR, leftC] = 'o';

                    if (!isCurrentStart) UpdateMap(currentR, currentC * rowsSapceIndex, ".");
                    UpdateMap(downR, leftC * rowsSapceIndex, "\x1b[93mo\x1b[0m");

                    return (true, downR, leftC, false);
                }
            }
            else
            {
                map[currentR, currentC] = isCurrentStart ? '+' : '.';
                return (true, currentR, currentC, true);
            }

            return (false, currentR, currentC, false);
        }

        static (bool, int, int, bool) MoveDownRight(char[,] map, int currentR, int currentC, bool isCurrentStart, int rowsSpaceIndex)
        {
            var totalRows = map.GetLength(0);
            var totalCols = map.GetLength(1);

            int downR = currentR + 1;
            int rightC = currentC + 1;

            if (downR < totalRows && rightC < totalCols)
            {
                if (map[downR, rightC] == '.')
                {
                    map[currentR, currentC] = isCurrentStart ? '+' : '.';
                    map[downR, rightC] = 'o';

                    if (!isCurrentStart) UpdateMap(currentR, currentC * rowsSpaceIndex, ".");
                    UpdateMap(downR, rightC * rowsSpaceIndex, "\x1b[93mo\x1b[0m");

                    return (true, downR, rightC, false);
                }
            }
            else
            {
                map[currentR, currentC] = isCurrentStart ? '+' : '.';
                return (true, currentR, currentC, true);
            }

            return (false, currentR, currentC, false);
        }

        static char[,] InitializeEmptyMap(int rows, int cols, Point startingPosition, int minX, int minY, IEnumerable<Point> rocks, bool addFloor)
        {
            var map = new char[rows, cols];

            for (int i = 0; i < rows * cols; i++) map[i / cols, i % cols] = '.';

            var startingCol = startingPosition.X - minX;
            var startingRow = startingPosition.Y - minY;

            map[startingRow, startingCol] = '+';

            foreach (var rock in rocks)
            {
                var rockCol = rock.X - minX;
                var rockRow = rock.Y - minY;

                map[rockRow, rockCol] = '#';
            }

            if (addFloor)
            {
                var points = Point.GetPointsBetween(new Point(rows-1, 0), new Point(rows-1, cols-1));

                foreach (var p in points)
                {
                    map[p.X, p.Y] = '#';
                }
            }

            return map;
        }

        static void UpdateMap(int row, int column, string item)
        {
            Console.SetCursorPosition(column, row);
            Console.Write(item);
            //Thread.Sleep(1);
        }

        static void PrintMap(char[,] map, int totalSandUnits, int highlighedR, int highlightedC, bool spaceBetweenRows)
        {
            Console.SetCursorPosition(0, 0);

            var betweenRows = spaceBetweenRows? " " : string.Empty;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == highlighedR && j == highlightedC)
                    {
                        Console.Write("\x1b[93m" + map[i, j] + "\x1b[0m" + betweenRows);
                    }
                    else
                    {
                        if (map[i, j] == 'o')
                        {
                            Console.Write("\x1b[32m" + map[i, j] + "\x1b[0m" + betweenRows);
                        }
                        else
                        {
                            Console.Write(map[i, j] + betweenRows);
                        }
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n**************");
            Console.WriteLine("Total sand units: " + totalSandUnits);
            Console.WriteLine("**************");

            //Thread.Sleep(10);
        }

        static List<Point> GetAllBetweenBoundings(List<Point> boundings)
        {
            var result = new List<Point>();

            for (int i = 1; i < boundings.Count; i++)
            {
                result.AddRange(Point.GetPointsBetween(boundings[i-1], boundings[i]));
            }

            return result.Distinct().ToList();
        }

        static (int, int, int, int) GetBoundings(IEnumerable<Point> points)
        {
            var result = points.Aggregate(
                  new
                  {
                      MinX = int.MaxValue,
                      MaxX = int.MinValue,
                      MinY = int.MaxValue,
                      MaxY = int.MinValue,
                  },
                  (accumulator, p) => new
                  {
                      MinX = Math.Min(p.X, accumulator.MinX),
                      MaxX = Math.Max(p.X, accumulator.MaxX),
                      MinY = Math.Min(p.Y, accumulator.MinY),
                      MaxY = Math.Max(p.Y, accumulator.MaxY),
                  });

            return (result.MaxX, result.MinX, result.MaxY, result.MinY);
        }
    }
}

