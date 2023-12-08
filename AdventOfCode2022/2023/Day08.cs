using System.Text.RegularExpressions;

namespace AdventOfCode._2023
{
    internal static class Day08
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2023", "Day08");

            var data = input.Split("\r\n\r\n");

            var instructions = data[0];
            
            var map = GetMap(data[1].Split("\r\n"));
            var totalSteps = 0;
            var currentPoint = "AAA";
            var target = "ZZZ";

            while (currentPoint != target)
            {
                var instruction = instructions[totalSteps++ % instructions.Length];
                currentPoint = instruction == 'L' ? map[currentPoint].Item1 : map[currentPoint].Item2;
            }

            Console.WriteLine(totalSteps);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsString("2023", "Day08");

            var data = input.Split("\r\n\r\n");

            var instructions = data[0];

            var map = GetMap(data[1].Split("\r\n"));

            var currentPoints = map.Keys.Where(k => k.EndsWith('A')).ToList();
            var targetEnding = "Z";

            var targetRepeats = new List<long>();

            foreach (var cp in currentPoints)
            {
                var currentPoint = cp;
                var currentTotalSteps = 0;
                while (!currentPoint.EndsWith(targetEnding))
                {
                    var instruction = instructions[currentTotalSteps++ % instructions.Length];
                    currentPoint = instruction == 'L' ? map[currentPoint].Item1 : map[currentPoint].Item2;
                }
                targetRepeats.Add(currentTotalSteps);
            }

            Console.WriteLine(LeastCommonMultiple(targetRepeats));
        }

        private static long Lcm(long n1, long n2)
        {
            return n2 == 0 ? n1 : Lcm(n2, n1 % n2);
        }

        private static long LeastCommonMultiple(List<long> numbers)
        {
            return numbers.Aggregate((S, val) => S * val / Lcm(S, val));
        }

        private static Dictionary<string, (string, string)> GetMap(string[] input)
        {
            return input.Select(p =>
                            Regex.Match(p, @"(\S+) = \((\S+), (\S+)\)"))
                .Select(match =>
                new KeyValuePair<string, (string, string)>(match.Groups[1].Value, (match.Groups[2].Value, match.Groups[3].Value)))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
