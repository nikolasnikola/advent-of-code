using AdventOfCode2022.Dto.Day18;

namespace AdventOfCode2022._2022
{
    internal static class Day18
    {
        static List<Point3D> FindAirPockets(Point3D start, int[,,] cube)
        {
            Stack<Point3D> stack = new();
            var visited = new HashSet<Point3D>();

            stack.Push(start);
            while (stack.Any())
            {
                Point3D cur = stack.Pop();
                visited.Add(cur);
                if (cube[cur.X, cur.Y, cur.Z] == 0)
                {
                    cube[cur.X, cur.Y, cur.Z] = 1;
                    foreach (Point3D neighbour in cur.GetNeighbours())
                    {
                        if (!visited.Contains(neighbour) && WithinCube(neighbour, cube))
                        {
                            stack.Push(neighbour);
                        }
                    }
                }
            }
            return FilterAirPockets(cube);
        }

        static List<Point3D> FilterAirPockets(int[,,] cube)
        {
            List<Point3D> airPockets = new();

            for (int x = 0; x < cube.GetLength(0); x++)
            {
                for (int y = 0; y < cube.GetLength(1); y++)
                {
                    for (int z = 0; z < cube.GetLength(2); z++)
                    {
                        if (cube[x,y,z] == 0)
                        {
                            airPockets.Add(new Point3D(x, y, z));
                        }
                    }
                }
            }
            return airPockets;
        }

        static bool WithinCube(Point3D p, int[,,] cube)
        {
            return p.X >= 0 && p.Y >= 0 && p.Z >= 0 && p.X < cube.GetLength(0) && p.Y < cube.GetLength(1)
                    && p.Z < cube.GetLength(2);
        }

        static int[,,] GetCube(List<Point3D> points)
        {
            var maxX = points.Select(p => p.X).Max();
            var maxY = points.Select(p => p.Y).Max();
            var maxZ = points.Select(p => p.Z).Max();
            var maxLength = Math.Max(maxX, Math.Max(maxY, maxZ));
            var dimension = Convert.ToInt32(maxLength + 1);

            int[,,] cube = new int[dimension, dimension, dimension];

            foreach (var pt in points)
            {
                cube[pt.X, pt.Y, pt.Z] = 2;
            }

            return cube;
        }

        internal static void Part2()
        {
            var pointCoordinates = Reader.ReadAsStringList("Day18");

            var points = GetPointsList(pointCoordinates).ToList();

            var lavaSurface = GetLavaSurface(points);
            var airPockets = FindAirPockets(new Point3D(0, 0, 0), GetCube(points));

            int airPocketTouches = 0;

            foreach (var point in airPockets)
            {
                var numberOfTouches = 0;

                foreach (var pt in points)
                {
                    if (point.AreTouching(pt))
                    {
                        numberOfTouches++;
                    };
                }

                airPocketTouches += numberOfTouches;
            }

            Console.WriteLine(lavaSurface - airPocketTouches);
        }

        internal static void Part1()
        {
            var pointCoordinates = Reader.ReadAsStringList("Day18");

            var points = GetPointsList(pointCoordinates).ToList();
            int result = GetLavaSurface(points);

            Console.WriteLine(result);
        }

        private static int GetLavaSurface(List<Point3D> points)
        {
            int result = 0;

            foreach (var point in points)
            {
                var numberOfTouches = 0;

                foreach (var pt in points)
                {
                    if (point.DistanceTo(pt) == 1) numberOfTouches++;
                }

                result += 6 - numberOfTouches;
            }

            return result;
        }

        static IEnumerable<Point3D> GetPointsList(string[] points)
        {
            foreach (var pt in points)
            {
                var coordinates = pt.Split(',');
                yield return new Point3D(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));
            }
        }
    }
}
