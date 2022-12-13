namespace AdventOfCode2022._2022
{
    internal static class Day6
    {
        internal static void Part2()
        {
            var signal = Reader.ReadAsString("Day6");

            var result = GetMarketValue(signal, 14);

            Console.WriteLine(result);
        }

        internal static void Part1()
        {
            var signal = Reader.ReadAsString("Day6");

            var result = GetMarketValue(signal, 4);

            Console.WriteLine(result);
        }

        static int GetMarketValue(string signal, int distinctLimit)
        {
            for (int i = distinctLimit; i < signal.Length; i++)
            {
                var currentMarker = signal.Substring(i - distinctLimit, distinctLimit);

                if (currentMarker.Distinct().Count() == distinctLimit)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
