namespace AdventOfCode._2022
{
    internal static class Day1
    {
        internal static void Part2()
        {
            var calories = Reader.ReadAsString("2022", "Day1");

            var separatedByElf = calories.Split("\r\n\r\n");

            var max = separatedByElf
                .Select(elfCalories
                    => elfCalories.Split("\r\n")
                    .Select(c => int.Parse(c))
                    .Sum())
                .OrderByDescending(s => s)
                .Take(3)
                .Sum();

            Console.WriteLine(max);
        }

        internal static void Part1()
        {
            var calories = Reader.ReadAsString("2022", "Day1");

            var separatedByElf = calories.Split("\r\n\r\n");

            var max = separatedByElf
                .Select(elfCalories
                    => elfCalories.Split("\r\n")
                    .Select(c => int.Parse(c))
                    .Sum())
                .Max();

            Console.WriteLine(max);
        }
    }
}
