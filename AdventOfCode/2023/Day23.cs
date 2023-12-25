namespace AdventOfCode._2023
{
    internal static class Day23
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsCharDictionaryMatrix("2023", "Day23");

            var startPoint = input.Single(x => x.Key.Item1 == 0 && x.Value == '.').Key;
            var endPoint = input.Single(x => x.Key.Item1 == input.Keys.Select(x => x.Item1).Max() && x.Value == '.').Key;

            //var memo = new Dictionary<((int X, int Y) PreviousPoint, (int X, int Y) CurrentPoint), int>();

            var history = new List<(int, int)>();
            var result = GetLongestPath(startPoint, null, endPoint, input, history, false);

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsCharDictionaryMatrix("2023", "Day23Test");

            var startPoint = input.Single(x => x.Key.Item1 == 0 && x.Value == '.').Key;
            var endPoint = input.Single(x => x.Key.Item1 == input.Keys.Select(x => x.Item1).Max() && x.Value == '.').Key;

            //var memo = new Dictionary<((int X, int Y) PreviousPoint, (int X, int Y) CurrentPoint), int>();
            var history = new List<(int, int)>();

            var result = GetLongestPath(startPoint, null, endPoint, input, history, true);

            Console.WriteLine(result);
        }

        private static int GetLongestPath(
            (int X, int Y) currentPoint,
            (int X, int Y)? previousPoint,
            (int X, int Y) endPoint,
            Dictionary<(int, int), char> map,
            List<(int X, int Y)> history,
            bool ignoreSlopes)
        {
            //if (previousPoint.HasValue && memo.ContainsKey((previousPoint.Value, currentPoint))) return memo[(previousPoint.Value, currentPoint)];
            history.Add(currentPoint);

            var nextPoints = GetNextPoints(currentPoint, previousPoint, map, ignoreSlopes, history);

            if (nextPoints.Count == 0) return 0;

            if (nextPoints.Count > 1)
            {
                var maxPath = nextPoints.Select(np => GetLongestPath(np, currentPoint, endPoint, map, new List<(int, int)>(history), ignoreSlopes)).Max() + 1;
                //memo.Add((previousPoint!.Value, currentPoint), maxPath);
                return maxPath;
            }

            var nextSingle = nextPoints.Single();
            if (nextSingle == endPoint) 
                return 1;

            var next = GetLongestPath(nextSingle, currentPoint, endPoint, map, history, ignoreSlopes);
            return 1 + next;
        }

        private static IList<(int, int)> GetNextPoints((int X, int Y) currentPoint, (int X, int Y)? previousPoint, Dictionary<(int, int), char> map, bool ignoreSlopes, List<(int X, int Y)> history)
        {
            var points = new List<(int, int)>();

            var north = (currentPoint.X - 1, currentPoint.Y);
            var south = (currentPoint.X + 1, currentPoint.Y);
            var west = (currentPoint.X, currentPoint.Y - 1);
            var east = (currentPoint.X, currentPoint.Y + 1);

            var currentItem = (map[currentPoint]);

            if (!history.Contains(north) && (ignoreSlopes || currentItem == '^' || currentItem == '.') && previousPoint != north && map.TryGetValue(north, out var valueN) && valueN != '#') points.Add(north);
            if (!history.Contains(south) && (ignoreSlopes || currentItem == 'v' || currentItem == '.') && previousPoint != south && map.TryGetValue(south, out var valueS) && valueS != '#') points.Add(south);
            if (!history.Contains(west) && (ignoreSlopes || currentItem == '<' || currentItem == '.') && previousPoint != west && map.TryGetValue(west, out var valueW) && valueW != '#') points.Add(west);
            if (!history.Contains(east) && (ignoreSlopes || currentItem == '>' || currentItem == '.') && previousPoint != east && map.TryGetValue(east, out var valueE) && valueE != '#') points.Add(east);

            return points;
        }
    }
}
