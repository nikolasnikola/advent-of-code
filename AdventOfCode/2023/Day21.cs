using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace AdventOfCode._2023
{
    internal static class Day21
    {
        private static int _rows;
        private static int _cols;
        internal static void Part1()
        {
            var input = Reader.ReadAsCharDictionaryMatrixWithStartPoint("2023", "Day21", 'S', out var startPosition);

            var map = input.Where(x => x.Value == '.' || x.Value == 'S').ToDictionary(x => x.Key, x => x.Value);
            var targetSteps = 64;

            var memo = new Dictionary<(int, int), HashSet<(int, int)>>();
            var currentPositions = new HashSet<(int, int)>() { startPosition };

            foreach (var _ in Enumerable.Range(1, targetSteps))
            {
                var newPositions = new HashSet<(int, int)>();

                foreach (var position in currentPositions)
                {
                    if (memo.ContainsKey(position)) newPositions.UnionWith(memo[position]);
                    else
                    {
                        if (TryGetNorth(position, map, out var north)) { newPositions.Add(north); }
                        if (TryGetSouth(position, map, out var south)) { newPositions.Add(south); }
                        if (TryGetWest(position, map, out var west)) { newPositions.Add(west); }
                        if (TryGetEast(position, map, out var east)) { newPositions.Add(east); }

                        memo.Add(position, newPositions);
                    }
                }

                currentPositions = newPositions;
            }

            Console.WriteLine(currentPositions.Count);
        }

        // need more brainstorming
        internal static void Part2()
        {
            var input = Reader.ReadAsCharDictionaryMatrixWithStartPoint("2023", "Day21", 'S', out var startPosition);

            var map = input.Where(x => x.Value == '.' || x.Value == 'S').ToDictionary(x => x.Key, x => x.Value);
            var targetSteps = 50;

            _rows = map.Keys.Max(x => x.Item1) + 1;
            _cols = map.Keys.Max(x => x.Item2) + 1;

            // Add new map copies on 4 sides
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (map.ContainsKey((i, j)))
                    {
                        map.Add((i - _rows, j), map[(i, j)]);
                        map.Add((i + _rows, j), map[(i, j)]);
                        map.Add((i, j + _cols), map[(i, j)]);
                        map.Add((i, j - _cols), map[(i, j)]);
                    }
                }
            }

            var memo = new Dictionary<(int, int), HashSet<(int, int)>>();
            var currentPositions = new HashSet<(int, int)>() { startPosition };

            int innerMapSteps = 0;
            int outerMapSteps = 0;
            var steps = 0;

            while (innerMapSteps == 0 || outerMapSteps == 0)
            {
                steps++;
                var newPositions = new HashSet<(int, int)>();
                var left = currentPositions.Min(x => x.Item2);
                var right = currentPositions.Max(x => x.Item2);
                var top = currentPositions.Min(x => x.Item1);
                var bottom = currentPositions.Max(x => x.Item1);

                foreach (var position in currentPositions)
                {
                    if (memo.ContainsKey(position)) newPositions.UnionWith(memo[position]);
                    else
                    {
                        if (TryGetNorth(position, map, out var north)) { newPositions.Add(north); }
                        if (TryGetSouth(position, map, out var south)) { newPositions.Add(south); }
                        if (TryGetWest(position, map, out var west)) { newPositions.Add(west); }
                        if (TryGetEast(position, map, out var east)) { newPositions.Add(east); }

                        memo.Add(position, newPositions);
                    }
                }

                currentPositions = newPositions;

                if (innerMapSteps == 0
                    && currentPositions.Any(x => x.Item1 <= 0)
                    && currentPositions.Any(x => x.Item1 >= _rows - 1)
                    && currentPositions.Any(x => x.Item2 <= 0)
                    && currentPositions.Any(x => x.Item2 >= _cols - 1))
                {
                    innerMapSteps = steps;
                    var topPosition = currentPositions.Where(x => x.Item1 == top);
                    var bottomPosition = currentPositions.Where(x => x.Item1 == bottom);
                    var leftPosition = currentPositions.Where(x => x.Item2 == left);
                    var rightPosition = currentPositions.Where(x => x.Item2 == right);
                }

                if (outerMapSteps == 0
                    && currentPositions.Any(x => x.Item1 <= -_rows)
                    && currentPositions.Any(x => x.Item1 >= _rows*2 - 1)
                    && currentPositions.Any(x => x.Item2 <= -_cols)
                    && currentPositions.Any(x => x.Item2 >= _cols*2 - 1))
                {
                    outerMapSteps = steps;
                }
            }

            Console.WriteLine(currentPositions.Count);
        }

        private static bool TryGetNorth((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1 - 1, currentPosition.Item2);
            return map.ContainsKey(position) || position.Item1 < -_rows;
        }

        private static bool TryGetSouth((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1 + 1, currentPosition.Item2);
            return map.ContainsKey(position) || position.Item1 > _rows*2-1;
        }

        private static bool TryGetWest((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1, currentPosition.Item2 - 1);
            return map.ContainsKey(position) || position.Item2 < -_cols;
        }

        private static bool TryGetEast((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1, currentPosition.Item2 + 1);
            return map.ContainsKey(position) || position.Item2 > _cols*2-1;
        }

        private static bool TryGetNorth2((int, int) currentPosition, Dictionary<(int, int), char> map, int rows, int cols, out (int, int) position)
        {
            position = (currentPosition.Item1 - 1, currentPosition.Item2);
            var reducedPosition = GetReducedPosition(rows, cols, position);

            return map.ContainsKey(reducedPosition);
        }

        private static bool TryGetSouth2((int, int) currentPosition, Dictionary<(int, int), char> map, int rows, int cols, out (int, int) position)
        {
            position = (currentPosition.Item1 + 1, currentPosition.Item2);
            var reducedPosition = GetReducedPosition(rows, cols, position);
            return map.ContainsKey(reducedPosition);
        }

        private static bool TryGetWest2((int, int) currentPosition, Dictionary<(int, int), char> map, int rows, int cols, out (int, int) position)
        {
            position = (currentPosition.Item1, currentPosition.Item2 - 1);
            var reducedPosition = GetReducedPosition(rows, cols, position);
            return map.ContainsKey(reducedPosition);
        }

        private static bool TryGetEast2((int, int) currentPosition, Dictionary<(int, int), char> map, int rows, int cols, out (int, int) position)
        {
            position = (currentPosition.Item1, currentPosition.Item2 + 1);
            var reducedPosition = GetReducedPosition(rows, cols, position);
            return map.ContainsKey(reducedPosition);
        }

        private static (int reducedPositionX, int reducedPositionY) GetReducedPosition(int rows, int cols, (int, int) position)
        {
            var reducedPositionX = position.Item1 < 0
                ? position.Item1 + (Math.Abs(position.Item1) / rows + 1) * rows
                : position.Item1 >= rows ? position.Item1 % rows : position.Item1;

            var reducedPositionY = position.Item2 < 0
                ? position.Item2 + (Math.Abs(position.Item2) / cols + 1) * cols
                : position.Item2 >= cols ? position.Item2 % cols : position.Item2;

            var reducedPosition = (reducedPositionX, reducedPositionY);
            return reducedPosition;
        }
    }
}