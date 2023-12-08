using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal static class Day09
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsLongArray("2020", "Day09");
            var result = GetFirstWeakness(25, input);
           
            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsLongArray("2020", "Day09");
            var weak = GetFirstWeaknessWithIndex(25, input);

            long result = 0;

            for (int i = 0; i<weak.Item2-1; i++)
            {
                var currentSum = input[i];
                var finished = false;
                var currentMin = input[i];
                var currentMax = input[i];

                for (int j = i+1; j<weak.Item2; j++)
                {
                    currentSum += input[j];
                    currentMin = Math.Min(currentMin, input[j]);
                    currentMax = Math.Max(currentMax, input[j]);

                    if (currentSum == weak.Item1)
                    {
                        result = currentMin + currentMax;
                        finished = true;
                        break;
                    }
                    else if (currentSum > weak.Item1) break;
                }

                if (finished) break;
            }

            Console.WriteLine(result);
        }

        static long GetFirstWeakness(int preambleSize, long[] input)
        {
            for (int i = preambleSize; i < input.Length; i++)
            {
                if (!CanSum(input[i], input.Skip(i-preambleSize).Take(preambleSize)))
                {
                    return input[i];
                }
            }

            return 0;
        }

        static (long, int) GetFirstWeaknessWithIndex(int preambleSize, long[] input)
        {
            for (int i = preambleSize; i < input.Length; i++)
            {
                if (!CanSum(input[i], input.Skip(i - preambleSize).Take(preambleSize)))
                {
                    return (input[i], i);
                }
            }

            return (0,0);
        }
        
        static bool CanSum(long target, IEnumerable<long> numbers)
        {
            var differences = new HashSet<long>();

            foreach (var n in numbers)
            {
                if (differences.Contains(n)) return true;
                differences.Add(target - n);
            }

            return false;
        }
    }
}
