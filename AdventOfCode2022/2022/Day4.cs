namespace AdventOfCode._2022
{
    internal static class Day4
    {
        internal static void Part2()
        {
            var pairs = Reader.ReadAsStringList("2022", "Day4");

            var result = pairs.Select(p => GetPair(p)).Where(p => PairOverlapOther(p)).Count();

            Console.WriteLine(result);
        }

        internal static void Part1()
        {
            var pairs = Reader.ReadAsStringList("2022", "Day4");

            var result = pairs.Select(p => GetPair(p)).Where(p => PairFullyContainOther(p)).Count();

            Console.WriteLine(result);
        }

        static bool PairFullyContainOther((IEnumerable<int>, IEnumerable<int>) pairs)
        {
            var lowerLength = Math.Min(pairs.Item1.Count(), pairs.Item2.Count());

            return (pairs.Item1.Intersect(pairs.Item2)).Count() == lowerLength;
        }

        static bool PairOverlapOther((IEnumerable<int>, IEnumerable<int>) pairs)
        {
            return (pairs.Item1.Intersect(pairs.Item2)).Any();
        }


        static (IEnumerable<int>, IEnumerable<int>) GetPair(string pairItem)
        {
            var pairs = pairItem.Split(',');

            return (GetRange(pairs[0]), GetRange(pairs[1]));
        }

        static IEnumerable<int> GetRange(string pair)
        {
            var ranges = pair.Split("-");
            var from = int.Parse(ranges[0]);
            var to = int.Parse(ranges[1]);

            return Enumerable.Range(from, to - from + 1);
        }
    }
}
