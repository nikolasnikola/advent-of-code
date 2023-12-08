using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2020
{
    internal static class Day01
    {
        internal static void Part1()
        {
            var entries = Reader.ReadAsIntegerArray("2020", "Day01");
           
            long result = GetResult(entries);

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var entries = Reader.ReadAsIntegerArray("2020", "Day01");

            long result = GetResult2(entries);

            Console.WriteLine(result);
        }

        private static long GetResult(int[] entries)
        {
            var differences = new HashSet<int>();
            foreach (var entry in entries)
            {
                if (differences.TryGetValue(entry, out var value))
                {
                    return entry * (2020 - value);
                }
                differences.Add(2020 - entry);
            }

            return 0;
        }

        private static long GetResult2(int[] entries)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                var target = 2020 - entries[i];
                var differences = new HashSet<int>();

                for (int j = i + 1; j < entries.Length; j++)
                {
                    if (differences.Contains(entries[j]))
                    {
                        return entries[i] * entries[j] * (2020 - entries[i] - entries[j]);
                    }
                    differences.Add(target - entries[j]);
                }
            }

            return 0;
        }
    }
}
