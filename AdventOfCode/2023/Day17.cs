namespace AdventOfCode._2023
{
    internal static class Day17
    {
        internal static void Part1()
        {
            var map = Reader.ReadAsIntegerDictionaryMatrix("2023", "Day17");

            int result = CalculateHeatloss(0, 3, map);

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var map = Reader.ReadAsIntegerDictionaryMatrix("2023", "Day17");

            int result = CalculateHeatloss(4, 10, map);

            Console.WriteLine(result);
        }

        static int CalculateHeatloss(int minDirectionCount, int maxDirectionCount, Dictionary<(int,int), int> map)
        {
            var goal = (map.Keys.Max(x => x.Item1), map.Keys.Max(x => x.Item2));
            var queue = new PriorityQueue<Crucible, int>();

            queue.Enqueue(new Crucible(Position: (0, 0), Direction: (0, 1), DirectionCount: 0), 0);
            queue.Enqueue(new Crucible(Position: (0, 0), Direction: (1, 0), DirectionCount: 0), 0);

            var seen = new HashSet<Crucible>();
            while (queue.TryDequeue(out var crucible, out var heatloss))
            {
                if (crucible.Position == goal && crucible.DirectionCount >= minDirectionCount) return heatloss;
                foreach (var next in GetMoves(crucible, minDirectionCount, maxDirectionCount))
                {
                    if (map.ContainsKey(next.Position) && !seen.Contains(next))
                    {
                        seen.Add(next);
                        queue.Enqueue(next, heatloss + map[next.Position]);
                    }
                }
            }
            return -1;
        }


        private static IEnumerable<Crucible> GetMoves(Crucible crucible, int minDirectionCount, int maxDirectionCount)
        {
            if (crucible.DirectionCount < maxDirectionCount)
            {
                yield return crucible with
                {
                    Position = (crucible.Position.Item1 + crucible.Direction.Item1, crucible.Position.Item2 + crucible.Direction.Item2),
                    DirectionCount = crucible.DirectionCount + 1
                };
            }

            if (crucible.DirectionCount >= minDirectionCount)
            {
                var dir = (crucible.Direction.Item2, crucible.Direction.Item1);
                yield return new Crucible(Position: (crucible.Position.Item1 + dir.Item1, crucible.Position.Item2 + dir.Item2), Direction: dir, DirectionCount: 1);
                yield return new Crucible(Position: (crucible.Position.Item1 - dir.Item1, crucible.Position.Item2 - dir.Item2), Direction: (-dir.Item1, -dir.Item2), DirectionCount: 1);
            }
        }
    }

    record Crucible((int, int) Position, (int, int) Direction, int DirectionCount);
}