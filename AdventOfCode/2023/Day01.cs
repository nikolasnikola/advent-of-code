namespace AdventOfCode._2023
{
    internal static class Day01
    {
        internal static void Part1()
        {
            var entries = Reader.ReadAsStringList("2023", "Day01");

            int result = entries.Select(x => GetNumber(x)).Sum();

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var entries = Reader.ReadAsStringList("2023", "Day01");

            int result = entries.Select(x => GetNumber2(x)).Sum();

            Console.WriteLine(result);
        }

        private static int GetNumber(string input)
        {
            var firstNumber = int.Parse(input.First(x => char.IsNumber(x)).ToString());
            var lastNumber = int.Parse(input.Last(x => char.IsNumber(x)).ToString());

            return firstNumber * 10 + lastNumber;
        }

        private static int GetNumber2(string input)
        {
            var validInputs = new Dictionary<string, int>
            {
                { "1", 1 },
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 }
            };

            var currentMinValue = 0;
            var currentMaxValue = 0;
            var currentMinIndex = int.MaxValue;
            var currentMaxIndex = int.MinValue;

            foreach (var number in validInputs)
            {
                var firstIndex = input.IndexOf(number.Key);
                var lastIndex = input.LastIndexOf(number.Key);

                if (firstIndex >= 0 && firstIndex < currentMinIndex) { currentMinIndex = firstIndex; currentMinValue = number.Value; }
                if (lastIndex >= 0 && lastIndex > currentMaxIndex) { currentMaxIndex = lastIndex; currentMaxValue = number.Value; }
            }

            return currentMinValue * 10 + currentMaxValue;
        }
    }
}
