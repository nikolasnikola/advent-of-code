using System.Data.Common;

namespace AdventOfCode._2023
{
    internal static class Day06
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day06");

            var times = input[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => int.Parse(x)).ToList();
            var distances = input[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => int.Parse(x)).ToList();

            var possibleWays = new List<int>();

            for (int i = 0; i < times.Count; i++)
            {
                var edge = times[i] / 2;
                var currentPossibleWays = 0;

                if (times[i] % 2 == 0)
                {
                    edge = (times[i] / 2) - 1;
                    currentPossibleWays++;
                }

                var finished = false;
                var currentTime = edge;

                while (!finished)
                {
                    if (currentTime * (times[i] - currentTime) <= distances[i])
                    {
                        finished = true;
                    }
                    else
                    {
                        currentPossibleWays += 2;
                        currentTime--;
                    }
                }

                possibleWays.Add(currentPossibleWays);
            }

            var result = possibleWays.Aggregate((a, x) => a * x);
            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day06");

            var time = long.Parse(input[0].Split(':')[1].Replace(" ", string.Empty));
            var distance = long.Parse(input[1].Split(':')[1].Replace(" ", string.Empty));

            var edge = time / 2;
            long currentPossibleWays = 0;

            if (time % 2 == 0)
            {
                edge = (time / 2) - 1;
                currentPossibleWays++;
            }

            var finished = false;
            var currentTime = edge;

            while (!finished)
            {
                if (currentTime * (time - currentTime) <= distance)
                {
                    finished = true;
                }
                else
                {
                    currentPossibleWays += 2;
                    currentTime--;
                }
            }

            Console.WriteLine(currentPossibleWays);
        }
    }
}
