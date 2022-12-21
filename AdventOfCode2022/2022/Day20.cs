using AdventOfCode2022.Dto.Day20;

namespace AdventOfCode2022._2022
{

    internal static class Day20
    {
        internal static void Part2()
        {
            var items = Reader.ReadAsIntegerArray("Day20").ToList();
            var length = items.Count;

            var nodes = GetNodes(items, 811589153);

            foreach (int counter in Enumerable.Range(0, 10)) 
            {
                foreach (var node in nodes)
                {
                    var index = Math.Abs(node.Value) % (nodes.Count - 1);

                    for (var i = 0; i < index; i++)
                    {
                        var nodeToShift = node.Value > 0 ? node : node.Previous;
                        nodeToShift.MoveRight();
                    }
                }
            }

            var circuralItems = nodes.Single(n => n.Value == 0).Take(3001).ToArray();

            var xCoordinate = circuralItems[1000];
            var yCoordinate = circuralItems[2000];
            var zCoordinate = circuralItems[3000];

            var result = xCoordinate + yCoordinate + zCoordinate;
            Console.WriteLine($"Coordinates: {xCoordinate}, {yCoordinate}, {zCoordinate}. Total sum {result}");
        }

        internal static void Part1()
        {
            var items = Reader.ReadAsIntegerArray("Day20").ToList();
            var length = items.Count;

            var nodes = GetNodes(items, 1);

            foreach (var node in nodes)
            {
                var index = Math.Abs(node.Value) % (nodes.Count - 1);

                for (var i = 0; i < index; i++)
                {
                    var nodeToShift = node.Value > 0 ? node : node.Previous;
                    nodeToShift.MoveRight();
                }
            }

            var circuralItems = nodes.Single(n => n.Value == 0).Take(3001).ToArray();

            var xCoordinate = circuralItems[1000];
            var yCoordinate = circuralItems[2000];
            var zCoordinate = circuralItems[3000];

            var result = xCoordinate + yCoordinate + zCoordinate;
            Console.WriteLine($"Coordinates: {xCoordinate}, {yCoordinate}, {zCoordinate}. Total sum {result}");
        }

        static List<CircularNodes> GetNodes(List<int> items, long multiply)
        {
            var nodes = new List<CircularNodes>();

            for (int i = 0; i < items.Count; i++)
            {
                var node = new CircularNodes(items[i] * multiply);

                if (i > 0)
                {
                    node.Previous = nodes[i - 1];
                    nodes[i - 1].Next = node;
                }

                nodes.Add(node);
            }

            nodes.Last().Next = nodes.First();
            nodes.First().Previous = nodes.Last();

            return nodes;
        }
    }
}
