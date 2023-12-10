namespace AdventOfCode._2023
{
    internal static class Day09
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day09");

            var reports = input.Select(x => x.Split(" ").Select(n => int.Parse(n)));

            int sum = reports.Select(x => Extrapolate(x, false)).Sum();

            Console.WriteLine(sum);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day09");

            var reports = input.Select(x => x.Split(" ").Select(n => int.Parse(n)));

            int sum = reports.Select(x => Extrapolate(x, true)).Sum();

            Console.WriteLine(sum);
        }

        private static int Extrapolate(IEnumerable<int> report, bool backwards)
        {
            if (backwards) report = report.Reverse();

            var sum = 0;
            var numberToSum = report.Last();
            var lastNumbers = new List<int>();

            var currentArray = report.ToList();

            while (!currentArray.All(x => x == 0))
            {
                var newArray = new List<int>();
                for (int i = 1; i < currentArray.Count; i++)
                {
                    newArray.Add(currentArray[i] - currentArray[i - 1]);
                }
                currentArray = newArray;
                lastNumbers.Add(currentArray.LastOrDefault());
            }

            sum += numberToSum + lastNumbers.Sum();
            return sum;
        }
    }
}