using System.Diagnostics;

namespace AdventOfCode2022.Dto.Day14
{
    public class Point : IEquatable<Point>
    {
        public Point(int x, int y) { X = x; Y = y; }

        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(Point? other)
        {
            return other is not null & X == other!.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Point);
        }

        public static bool operator == (Point? left, Point? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point? left, Point? right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
        }

        public static List<Point> GetPointsBetween(Point p1, Point p2)
        {
            List<Point> points = new();

            if (p1.X == p2.X)
            {
                if (p1.Y < p2.Y)
                {
                    for (int y = p1.Y; y <= p2.Y; y++)
                    {
                        Point p = new(p1.X, y);
                        points.Add(p);
                    }
                }

                else
                {
                    for (int y = p2.Y; y <= p1.Y; y++)
                    {
                        Point p = new(p2.X, y);
                        points.Add(p);
                    }
                }
                
            }
            else
            {
                if (p1.X < p2.X)
                {
                    for (int x = p1.X; x <= p2.X; x++)
                    {
                        Point p = new(x, p1.Y);
                        points.Add(p);
                    }
                }
                else
                {
                    for (int x = p2.X; x <= p1.X; x++)
                    {
                        Point p = new(x, p2.Y);
                        points.Add(p);
                    }
                }
            }

            return points;
        }
    }
}
