namespace AdventOfCode2022._2022
{
    internal static class Day3
    {
        internal static void Part2()
        {
            var items = Reader.ReadAsStringList("Day3");

            var splittedItems = items.Chunk(3);

            var result = splittedItems
                .SelectMany(i => FindDuplicatedItemsForGroup(i[0], i[1], i[2]))
                .Select(i => GetItemPriority(i))
                .Sum();

            Console.WriteLine(result);
        }

        internal static void Part1()
        {
            var items = Reader.ReadAsStringList("Day3");

            var duplicatedItems = items.SelectMany(i => FindDuplicatedItems(i));

            var result = duplicatedItems.Select(i => GetItemPriority(i)).Sum();

            Console.WriteLine(result);
        }

        static IEnumerable<char> FindDuplicatedItemsForGroup(string group1, string group2, string group3)
        {
            return group1.Intersect(group2).Intersect(group3);
        }

        static IEnumerable<char> FindDuplicatedItems(string items)
        {
            var compartmentSize = items.Length / 2;
            var compartment1 = items[..compartmentSize];
            var compartmente2 = items[compartmentSize..];

            return compartment1.Intersect(compartmente2);
        }

        static int GetItemPriority(char item)
        {
            return char.IsLower(item) ? item - 96 : item - 38;
        }
    }
}
