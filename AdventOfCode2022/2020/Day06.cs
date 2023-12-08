using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal static class Day06
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2020", "Day06");

            var groups = input.Split("\r\n\r\n").Select(x => new string(x.Where(c => !char.IsWhiteSpace(c)).ToArray()));

            var sum = 0;

            foreach (var group in groups)
            {
                sum += group.Distinct().Count();
            }

            Console.WriteLine(sum);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsString("2020", "Day06");

            var groups = input.Split("\r\n\r\n");

            var sum = 0;

            foreach (var group in groups)
            {
                var items = group.Split("\r\n");
                var dictionary = new Dictionary<char, int>();
                foreach (var item in items)
                {
                    foreach (var c in item)
                    {
                        if (!dictionary.ContainsKey(c)) dictionary.Add(c, 0);
                        dictionary[c]++;
                    }
                }
                var totalCommon = dictionary.Where(d => d.Value == items.Count()).Count();
                Console.WriteLine("\n\nTotal in group " + group + ": " + totalCommon);

                sum += totalCommon;
            }

            Console.WriteLine(sum);
        }
    }
}
