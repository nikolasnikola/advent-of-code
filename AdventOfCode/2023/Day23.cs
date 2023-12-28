namespace AdventOfCode._2023
{
    internal static class Day23
    {
        private static Dictionary<((int X, int Y) PreviousPoint, (int X, int Y) CurrentPoint, string History), int> memo = new Dictionary<((int X, int Y) PreviousPoint, (int X, int Y) CurrentPoint, string History), int>();

        internal static void Part1()
        {
            var input = Reader.ReadAsCharDictionaryMatrix("2023", "Day23");

            var startPoint = input.Single(x => x.Key.Item1 == 0 && x.Value == '.').Key;
            var endPoint = input.Single(x => x.Key.Item1 == input.Keys.Select(x => x.Item1).Max() && x.Value == '.').Key;

            //var memo = new Dictionary<((int X, int Y) PreviousPoint, (int X, int Y) CurrentPoint), int>();

            var history = new List<(int, int)>();
            var result = GetLongestPath(startPoint, null, endPoint, input, history, false);

            Console.WriteLine("Part 1: " + result.Item1);
        }

        internal static void Part2()
        {
            //var input = Reader.ReadAsCharMatrix("2023", "Day23");

            //var lastRow = input.GetLength(0) - 1;
            //var startPoint = (0, Array.IndexOf(MatrixHelper.GetRow(input, 0), '.'));
            //var endPoint = (lastRow, Array.IndexOf(MatrixHelper.GetRow(input, lastRow), '.'));

            var input = Reader.ReadAsCharDictionaryMatrix("2023", "Day23Test");

            var startPoint = input.Single(x => x.Key.Item1 == 0 && x.Value == '.').Key;
            var endPoint = input.Single(x => x.Key.Item1 == input.Keys.Select(x => x.Item1).Max() && x.Value == '.').Key;

            //var memo = new Dictionary<((int X, int Y) PreviousPoint, (int X, int Y) CurrentPoint), int>();
            var history = new List<(int, int)>();

            var res = GetLongestPath(startPoint, null, endPoint, input, history, true);
            //var result2 = GetLongestPath2(startPoint, null, endPoint, input, history, true);
            //var result = GetLongestPath3(startPoint, endPoint, input, true);

            Console.WriteLine("Part 2: " + (res.Item1));
        }

        private static (int, List<(int, int)>) GetLongestPath(
            (int X, int Y) currentPoint,
            (int X, int Y)? previousPoint,
            (int X, int Y) endPoint,
            Dictionary<(int, int), char> map,
            List<(int X, int Y)> history,
            bool ignoreSlopes)
        {
            //if (previousPoint.HasValue && memo.ContainsKey((previousPoint.Value, currentPoint))) 
            //    return (memo[(previousPoint.Value, currentPoint)], history);
            history.Add(currentPoint);

            var nextPoints = GetNextPoints(currentPoint, previousPoint, map, ignoreSlopes, history);

            if (nextPoints.Count == 0)
                return (0, null);

            if (nextPoints.Count > 1)
            {
                var allPaths = nextPoints.Select(np => GetLongestPath(np, currentPoint, endPoint, map, new List<(int, int)>(history), ignoreSlopes));
                var maxPath = allPaths.Where(x => x.Item2 != null).OrderByDescending(x => x.Item1).FirstOrDefault();

                if (maxPath == default) return (0, null);
                history = maxPath.Item2;
                //memo.Add((previousPoint!.Value, currentPoint, ToHistoryString(history)), maxPath.Item1+1);
                return (maxPath.Item1 + 1, history);
            }

            var nextSingle = nextPoints.Single();
            if (nextSingle == endPoint)
            {
                history.Add(nextSingle);
                return (1, history);
            }

            var next = GetLongestPath(nextSingle, currentPoint, endPoint, map, history, ignoreSlopes);

            if (next.Item2 == null) return (0, null);
            return (1 + next.Item1, next.Item2);
        }

        private static (int, List<(int, int)>) GetLongestPath2(
           (int X, int Y) currentPoint,
           (int X, int Y)? previousPoint,
           (int X, int Y) endPoint,
           char[,] map,
           List<(int X, int Y)> history,
           bool ignoreSlopes)
        {
            history.Add(currentPoint);
            var historyStr = ToHistoryString(history);
            if (previousPoint.HasValue && memo.ContainsKey((previousPoint.Value, currentPoint, historyStr)))
                return (memo[(previousPoint.Value, currentPoint, historyStr)], history);

            var nextPoints = GetNextPoints2(currentPoint, previousPoint, map, ignoreSlopes, history);

            if (nextPoints.Count == 0)
            {
                memo.Add((previousPoint!.Value, currentPoint, historyStr), 0);
                return (0, null);
            }

            if (nextPoints.Count > 1)
            {
                var allPaths = nextPoints.Select(np => GetLongestPath2(np, currentPoint, endPoint, map, new List<(int, int)>(history), ignoreSlopes));
                var maxPath = allPaths.Where(x => x.Item2 != null).OrderByDescending(x => x.Item1).FirstOrDefault();

                if (maxPath == default) return (0, null);
                history = maxPath.Item2;
                memo.Add((previousPoint!.Value, currentPoint, ToHistoryString(history)), maxPath.Item1 + 1);
                return (maxPath.Item1 + 1, history);
            }

            var nextSingle = nextPoints.Single();
            if (nextSingle == endPoint)
            {
                history.Add(nextSingle);
                memo.Add((previousPoint!.Value, currentPoint, ToHistoryString(history)), 1);
                return (1, history);
            }

            var next = GetLongestPath2(nextSingle, currentPoint, endPoint, map, history, ignoreSlopes);

            if (next.Item2 == null)
            {
                memo.Add((previousPoint!.Value, currentPoint, historyStr), 0);
                return (0, null);
            }

            if (previousPoint.HasValue) memo.Add((previousPoint.Value, currentPoint, historyStr), 1 + next.Item1);
            return (1 + next.Item1, next.Item2);
        }

        private static (int, List<(int, int)>) GetLongestPath3(
            (int X, int Y) startPoint,
            (int X, int Y) endPoint,
            char[,] map,
            bool ignoreSlopes)
        {
            var memo = new HashSet<((int X, int Y) point, int length, string history)>();
            var stack = new Stack<((int X, int Y) currentPoint, (int X, int Y)? previousPoint, List<(int, int)> history, int length)>();
            var longestPath = new List<(int, int)>();
            var allPaths = new List<List<(int, int)>>();

            stack.Push((startPoint, null, new List<(int, int)> { startPoint }, 1));

            while (stack.Count > 0)
            {
                var (currentPoint, previousPoint, history, length) = stack.Pop();

                // Check if current point is the endpoint
                if (currentPoint == endPoint )
                {
                    allPaths.Add(history);

                    if (history.Count > longestPath.Count)
                    {
                        longestPath = new List<(int, int)>(history);
                        continue;
                    }
                }

                // Avoid revisiting the same point
                if (history.Count > 1 && currentPoint == history[history.Count - 2])
                    continue;

                // Memoize the current point and length
                memo.Add((currentPoint, length, ToHistoryString(history)));

                var nextPoints = GetNextPoints2(currentPoint, previousPoint, map, ignoreSlopes, history);

                foreach (var nextPoint in nextPoints)
                {
                    var nextHistory = new List<(int, int)>(history) { nextPoint };

                    // Check if the result is already memoized
                    var memoKey = (nextPoint, length + 1, ToHistoryString(history));
                    if (memo.Contains(memoKey))
                    {
                        continue;
                        //Console.WriteLine(history.Count);
                    }
                        

                    stack.Push((nextPoint, currentPoint, nextHistory, length + 1));
                }
            }

            // Return the longest path found
            if (longestPath.Count > 0)
                return (longestPath.Count, longestPath);

            // No path found
            return (0, null);
        }

        private static string ToHistoryString(List<(int,int)> history)
        {
            return string.Join(";", history.Select(x => $"{x.Item1},{x.Item2}"));
        }

        private static List<(int, int)> ParseHistory(string historyStr)
        {
            return historyStr.Split(";").Select(x =>
            {
                var items = x.Split(',');
                return (int.Parse(items[0]), int.Parse(items[1]));
            }).ToList();
        }

        private static IList<(int, int)> GetNextPoints((int X, int Y) currentPoint, (int X, int Y)? previousPoint, Dictionary<(int, int), char> map, bool ignoreSlopes, List<(int X, int Y)> history)
        {
            var points = new List<(int, int)>();

            var north = (currentPoint.X - 1, currentPoint.Y);
            var south = (currentPoint.X + 1, currentPoint.Y);
            var west = (currentPoint.X, currentPoint.Y - 1);
            var east = (currentPoint.X, currentPoint.Y + 1);

            var currentItem = map[currentPoint];

            if (!history.Contains(north) && (ignoreSlopes || currentItem == '^' || currentItem == '.') && previousPoint != north && map.TryGetValue(north, out var valueN) && valueN != '#') points.Add(north);
            if (!history.Contains(south) && (ignoreSlopes || currentItem == 'v' || currentItem == '.') && previousPoint != south && map.TryGetValue(south, out var valueS) && valueS != '#') points.Add(south);
            if (!history.Contains(west) && (ignoreSlopes || currentItem == '<' || currentItem == '.') && previousPoint != west && map.TryGetValue(west, out var valueW) && valueW != '#') points.Add(west);
            if (!history.Contains(east) && (ignoreSlopes || currentItem == '>' || currentItem == '.') && previousPoint != east && map.TryGetValue(east, out var valueE) && valueE != '#') points.Add(east);

            return points;
        }

        private static IList<(int, int)> GetNextPoints2((int X, int Y) currentPoint, (int X, int Y)? previousPoint, char[,] map, bool ignoreSlopes, List<(int X, int Y)> history)
        {
            var points = new List<(int, int)>();

            var north = (currentPoint.X - 1, currentPoint.Y);
            var south = (currentPoint.X + 1, currentPoint.Y);
            var west = (currentPoint.X, currentPoint.Y - 1);
            var east = (currentPoint.X, currentPoint.Y + 1);

            var currentItem = map[currentPoint.X, currentPoint.Y];

            if (!history.Contains(north) && (ignoreSlopes || currentItem == '^' || currentItem == '.') && previousPoint != north && map.CanGoNext(north, out var valueN) && valueN != '#') points.Add(north);
            if (!history.Contains(south) && (ignoreSlopes || currentItem == 'v' || currentItem == '.') && previousPoint != south && map.CanGoNext(south, out var valueS) && valueS != '#') points.Add(south);
            if (!history.Contains(west) && (ignoreSlopes || currentItem == '<' || currentItem == '.') && previousPoint != west && map.CanGoNext(west, out var valueW) && valueW != '#') points.Add(west);
            if (!history.Contains(east) && (ignoreSlopes || currentItem == '>' || currentItem == '.') && previousPoint != east && map.CanGoNext(east, out var valueE) && valueE != '#') points.Add(east);

            return points;
        }

        private static bool CanGoNext(this char[,] map, (int, int) next, out char nextVal)
        {
            nextVal = '0';
            var canGo = next.Item1 >= 0 && next.Item1 < map.GetLength(0) && next.Item2 >= 0 && next.Item2 < map.GetLength(1);

            if (canGo) nextVal = map[next.Item1, next.Item2];

            return canGo;
        }
    }
}
