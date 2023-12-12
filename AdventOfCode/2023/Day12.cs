using NUnit.Framework;

namespace AdventOfCode._2023
{
    internal static class Day12
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day12");

            long totalCount = 0;

            foreach (var item in input)
            {
                var data = item.Split(" ");
                var mustHaveIndexes = data[0].IndexOfAll('#');
                var allowedIndexes = data[0].IndexOfAll('?').Union(mustHaveIndexes).ToArray();
                var spaceSize = data[0].Length;
                var numbers = data[1].Split(",").Select(x => int.Parse(x)).ToArray();

                totalCount += CountArrangeNumbersWithGaps(spaceSize, numbers, mustHaveIndexes, allowedIndexes);
            }

            Console.WriteLine(totalCount);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day12");

            long totalCount = 0;

            foreach (var item in input)
            {
                var data = item.Split(" ");
                var sprintConditions = string.Join("?", data[0], data[0], data[0], data[0], data[0]);
                var contiguousGroups = string.Join(",", data[1], data[1], data[1], data[1], data[1]);
                var mustHaveIndexes = sprintConditions.IndexOfAll('#');
                var allowedIndexes = sprintConditions.IndexOfAll('?').Union(mustHaveIndexes).ToArray();
                var spaceSize = sprintConditions.Length;
                var numbers = contiguousGroups.Split(",").Select(x => int.Parse(x)).ToArray();

                totalCount += CountArrangeNumbersWithGaps(spaceSize, numbers, mustHaveIndexes, allowedIndexes);
            }

            Console.WriteLine(totalCount);
        }

        private static int[] IndexOfAll(this string source, char target)
        {
            return source.Select((c, idx) => c == target ? idx : -1).Where(idx => idx != -1).ToArray();
        }

        public static long CountArrangeNumbersWithGaps(int spaceSize, int[] numbers, int[] mustHaveIndexes, int[] allowedIndexes)
        {
            List<int> currentArrangement = new List<int>();
            Dictionary<(int, int), long> memo = new Dictionary<(int, int), long>();
            return CountArrangeRecursive(spaceSize, numbers, currentArrangement, 0, 0, mustHaveIndexes, allowedIndexes, memo);
        }

        static long CountArrangeRecursive(int spaceSize, int[] numbers, List<int> currentArrangement, int index, int startIndex, int[] mustHaveIndexes, int[] allowedIndexes, Dictionary<(int, int), long> memo)
        {
            var numbersLeft = numbers.Length - index;

            if (!mustHaveIndexes.Where(x => x < currentArrangement.LastOrDefault()).All(x => currentArrangement.Contains(x)))
                return 0;

            if (spaceSize - index < numbers[index..].Sum() + numbersLeft - 1)
                return 0;

            if (index == numbers.Length)
            {
                if (mustHaveIndexes.All(x => currentArrangement.Contains(x))) return 1;
                return 0;
            }

            if (memo.ContainsKey((index, startIndex)))
            {
                return memo[(index, startIndex)];
            }

            long count = 0;

            for (int i = startIndex; i <= spaceSize - numbers[index]; i++)
            {
                if (!IsAllowedToPlace(i, numbers[index], allowedIndexes))
                {
                    continue;
                }

                PlaceNumber(currentArrangement, i, numbers[index]);
                count += CountArrangeRecursive(spaceSize, numbers, currentArrangement, index + 1, i + numbers[index] + 1, mustHaveIndexes, allowedIndexes, memo);
                RemoveNumber(currentArrangement, i, numbers[index]);
            }

            memo[(index, startIndex)] = count;
            return count;
        }

        static void PlaceNumber(List<int> arrangement, int startIndex, int length)
        {
            for (int i = startIndex; i < startIndex + length; i++)
            {
                arrangement.Add(i);
            }
        }

        static bool IsAllowedToPlace(int startIndex, int length, int[] allowedIndexes)
        {
            return Enumerable.Range(startIndex, length).All(x => allowedIndexes.Contains(x));
        }

        static void RemoveNumber(List<int> arrangement, int startIndex, int length)
        {
            for (int i = startIndex; i < startIndex + length; i++)
            {
                arrangement.Remove(i);
            }
        }
    }

    public class ArrangeTests
    {
        [Test]
        public void ArrangeNumbersWithGaps_Arranged()
        {
            var spaceSize = 15;
            var numbers = new[] { 1, 3, 1, 6 };
            var mustHave = new int[] { 1, 3, 5, 7, 9, 11, 13 };
            var allowed = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            var result = Day12.CountArrangeNumbersWithGaps(spaceSize, numbers, mustHave, allowed);

            Assert.AreEqual(1, result);
        }
    }
}