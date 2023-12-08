namespace AdventOfCode.Models.Day18
{
    public class Point3D : IEquatable<Point3D>
    {
        public Point3D(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public double DistanceTo(Point3D point)
        {
            var xDiff = point.X - X;
            var yDiff = point.Y - Y;
            var zDiff = point.Z - Z;

            return Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2) + Math.Pow(zDiff, 2));
        }

        public bool AreTouching(Point3D point)
        {
            var xDiff = Math.Abs(point.X - X);
            var yDiff = Math.Abs(point.Y - Y);
            var zDiff = Math.Abs(point.Z - Z);

            return xDiff + yDiff + zDiff == 1 && (xDiff == 1 || yDiff == 1 || zDiff == 1);
        }

        public List<Point3D> GetNeighbours()
        {
            List<Point3D> neighbours = new()
            {
                new Point3D(X + 1, Y, Z),
                new Point3D(X - 1, Y, Z),
                new Point3D(X, Y + 1, Z),
                new Point3D(X, Y - 1, Z),
                new Point3D(X, Y, Z + 1),
                new Point3D(X, Y, Z - 1)
            };

            return neighbours;
        }

        public bool Equals(Point3D? other)
        {
            return other is not null & X == other!.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Point3D);
        }

        public static bool operator ==(Point3D? left, Point3D? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point3D? left, Point3D? right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
        }
    }
}
