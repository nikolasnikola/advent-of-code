﻿using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace AdventOfCode._2023
{
    internal static class Day21
    {
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

        internal static void Part2()
        {
            var input = Reader.ReadAsCharDictionaryMatrixWithStartPoint("2023", "Day21Test", 'S', out var startPosition);

            var map = input.Where(x => x.Value == '.' || x.Value == 'S').ToDictionary(x => x.Key, x => x.Value);
            var targetSteps = 50;

            var rows = map.Keys.Max(x => x.Item1) + 1;
            var cols = map.Keys.Max(x => x.Item2) + 1;

            var memo = new Dictionary<(int, int), HashSet<(int, int)>>();
            var currentPositions = new HashSet<(int, int)>() { startPosition };

            foreach (var _ in Enumerable.Range(0, targetSteps))
            {
                var newPositions = new HashSet<(int, int)>();

                foreach (var position in currentPositions)
                {
                    if (memo.ContainsKey(position)) newPositions.UnionWith(memo[position]);
                    else
                    {
                        if (TryGetNorth2(position, map, rows, cols, out var north)) { newPositions.Add(north); }
                        if (TryGetSouth2(position, map, rows, cols, out var south)) { newPositions.Add(south); }
                        if (TryGetWest2(position, map, rows, cols, out var west)) { newPositions.Add(west); }
                        if (TryGetEast2(position, map, rows, cols, out var east)) { newPositions.Add(east); }

                        memo.Add(position, newPositions);
                    }
                }

                currentPositions = newPositions;
            }

            Console.WriteLine(currentPositions.Count);
        }

        private static bool TryGetNorth((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1 - 1, currentPosition.Item2);
            return map.ContainsKey(position);
        }

        private static bool TryGetSouth((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1 + 1, currentPosition.Item2);
            return map.ContainsKey(position);
        }

        private static bool TryGetWest((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1, currentPosition.Item2 - 1);
            return map.ContainsKey(position);
        }

        private static bool TryGetEast((int, int) currentPosition, Dictionary<(int, int), char> map, out (int, int) position)
        {
            position = (currentPosition.Item1, currentPosition.Item2 + 1);
            return map.ContainsKey(position);
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