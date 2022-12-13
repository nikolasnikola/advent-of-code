namespace AdventOfCode2022._2022
{
    internal static class Day13
    {
        internal static void Part2()
        {
            var input = Reader.ReadAsString("Day13");

            var items = input.Split("\r\n\r\n").SelectMany(i => i.Split("\r\n")).ToList();

            string dividerPacket1 = "[[2]]";
            string dividerPacket2 = "[[6]]";

            items.Add(dividerPacket1);
            items.Add(dividerPacket2);

            var ordered = items.OrderBy(i => i, new Day13Comparer()).ToList();

            var index1 = ordered.IndexOf(dividerPacket1) + 1;
            var index2 = ordered.IndexOf(dividerPacket2) + 1;

            var result = index1 * index2;

            Console.WriteLine(result);
        }

        internal static void Part1()
        {
            var input = Reader.ReadAsString("Day13");

            var pairs = input.Split("\r\n\r\n");

            List<int> rightOrdered = new();

            var comparer = new Day13Comparer();

            for (int i = 1; i <= pairs.Length; i++)
            {
                var pair = pairs[i - 1];

                var pairItems = pair.Split("\r\n");

                if (comparer.Compare(pairItems[0], pairItems[1]) == -1)
                {
                    rightOrdered.Add(i);
                }
            }

            var result = rightOrdered.Sum();

            Console.WriteLine(result);
        }
    }
}
