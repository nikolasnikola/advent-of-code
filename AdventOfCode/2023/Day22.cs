namespace AdventOfCode._2023
{
    internal static class Day22
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day22");

            var bricksOrdered = input.Select((v, i) => GetBrick(v, i)).OrderBy(x => Math.Min(x.Cube1.Z, x.Cube2.Z)).ToList();

            var minZ = Math.Min(bricksOrdered[0].Cube1.Z, bricksOrdered[0].Cube2.Z);
            var maxZ = Math.Max(bricksOrdered[bricksOrdered.Count - 1].Cube1.Z, bricksOrdered[bricksOrdered.Count - 1].Cube2.Z);
            ProcessFalling(bricksOrdered, minZ, maxZ);

            var allBricks = bricksOrdered.Select(x => x.Id);

            var topBricks = allBricks.Except(bricksOrdered.SelectMany(x => x.SettledOn).Distinct());

            var singleSupporters = bricksOrdered.Where(x => x.SettledOn.Count == 1).Select(x => x.SettledOn.Single()).Distinct();

            var multipleSupporters = bricksOrdered.Where(x => x.SettledOn.Count > 1).SelectMany(x => x.SettledOn).Distinct().Except(singleSupporters).Distinct();

            Console.WriteLine(topBricks.Count() + multipleSupporters.Count());
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day22");

            var bricksOrdered = input.Select((v, i) => GetBrick(v, i)).OrderBy(x => Math.Min(x.Cube1.Z, x.Cube2.Z)).ToList();

            var minZ = Math.Min(bricksOrdered[0].Cube1.Z, bricksOrdered[0].Cube2.Z);
            var maxZ = Math.Max(bricksOrdered[bricksOrdered.Count - 1].Cube1.Z, bricksOrdered[bricksOrdered.Count - 1].Cube2.Z);
            ProcessFalling(bricksOrdered, minZ, maxZ);

            var singleSupporters = bricksOrdered.Where(x => x.SettledOn.Count == 1).Select(x => x.SettledOn.Single()).Distinct();

            var result = 0;
            foreach (var singleSupporter in singleSupporters)
            {
                var willFall = new HashSet<int>();
                GetFalls(bricksOrdered, singleSupporter, willFall);
                result += willFall.Count;
            }

            Console.WriteLine(result);
        }

        private static void GetFalls(List<Brick> bricksOrdered, int singleSupporter, HashSet<int> willFall)
        {
            var currentBrick = bricksOrdered.Single(x => x.Id == singleSupporter);

            foreach (var itemId in currentBrick.Supports)
            {
                var brickItem = bricksOrdered.Single(x => x.Id == itemId);
                if (brickItem.SettledOn.Except(willFall).Count() == 1)
                {
                    GetFalls(bricksOrdered, brickItem.Id, willFall);
                    willFall.Add(brickItem.Id);
                }
            }
        }

        private static void ProcessFalling(List<Brick> bricksOrdered, int minZ, int maxZ)
        {
            var settled = new List<SettledState>();

            for (int i = minZ; i <= maxZ; i++)
            {
                foreach (var brick in bricksOrdered.Where(b => !b.Settled && (b.Cube1.Z == i || b.Cube2.Z == i)))
                {
                    var currentBrickFromX = Math.Min(brick.Cube1.X, brick.Cube2.X);
                    var currentBrickToX = Math.Max(brick.Cube1.X, brick.Cube2.X);
                    var currentBrickFromY = Math.Min(brick.Cube1.Y, brick.Cube2.Y);
                    var currentBrickToY = Math.Max(brick.Cube1.Y, brick.Cube2.Y);

                    var max = settled
                        .Where(x => x.Xs.Any(x => x >= currentBrickFromX && x <= currentBrickToX) && x.Ys.Any(x => x >= currentBrickFromY && x <= currentBrickToY))
                        .Select(x => (x.BrickId, x.Zs.Max()))
                        .GroupBy(x => x.Item2)
                        .OrderByDescending(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.ToList())
                        .FirstOrDefault(new KeyValuePair<int, List<(int BrickId, int)>>(0, null));

                    var fromZ = max.Key + 1;
                    var toZ = fromZ + Math.Abs(brick.Cube1.Z - brick.Cube2.Z);

                    brick.Settled = true;
                    brick.Cube1.Z = brick.Cube1.Z < brick.Cube2.Z ? fromZ : toZ;
                    brick.Cube2.Z = brick.Cube2.Z < brick.Cube1.Z ? fromZ : toZ;

                    if (max.Value != null && max.Value.Count > 0)
                    {
                        var settledOn = max.Value.Select(x => x.BrickId);
                        brick.SettledOn.UnionWith(settledOn);

                        foreach (var supporter in settledOn)
                        {
                            bricksOrdered.Single(x => x.Id == supporter).Supports.Add(brick.Id);
                        }
                    }

                    var xs = new List<int>();
                    var ys = new List<int>();
                    var zs = new List<int>();
                    for (int x = currentBrickFromX; x <= currentBrickToX; x++) xs.Add(x);
                    for (int y = currentBrickFromY; y <= currentBrickToY; y++) ys.Add(y);
                    for (int z = fromZ; z <= toZ; z++) zs.Add(z);

                    settled.Add(new SettledState { BrickId = brick.Id, Xs = xs, Ys = ys, Zs = zs });
                }
            }
        }

        private static Brick GetBrick(string input, int brickId)
        {
            var splitted = input.Split('~');

            var cube1 = splitted[0].Split(',');
            var cube2 = splitted[1].Split(",");

            return new Brick()
            {
                Cube1 = new BrickCube() { X = int.Parse(cube1[0]), Y = int.Parse(cube1[1]), Z = int.Parse(cube1[2]) },
                Cube2 = new BrickCube() { X = int.Parse(cube2[0]), Y = int.Parse(cube2[1]), Z = int.Parse(cube2[2]) },
                Id = brickId
            };
        }
    }

    public class Brick
    {
        public Brick()
        {
            Settled = false;
            SettledOn = new HashSet<int>();
            Supports = new HashSet<int>();
        }

        public int Id { get; set; }
        public BrickCube Cube1 { get; set; }
        public BrickCube Cube2 { get; set; }
        public bool Settled { get; set; }
        public HashSet<int> SettledOn { get; set; }
        public HashSet<int> Supports { get; set; }

        public bool IsSingleCube => Cube1.X == Cube2.X && Cube1.Y == Cube2.Y && Cube1.Z == Cube2.Z;
        public int XLength => Math.Abs(Cube1.X - Cube2.X) + 1;
        public int YLength => Math.Abs(Cube1.Y - Cube2.Y) + 1;
        public int ZLength => Math.Abs(Cube1.Z - Cube2.Z) + 1;
    }

    public class BrickCube
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    public class SettledState
    {
        public int BrickId { get; set; }
        public List<int> Xs { get; set; }
        public List<int> Ys { get; set; }
        public List<int> Zs { get; set; }
    }
}
