namespace AdventOfCode._2023
{
    internal static class Day02
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day02");

            var result = input
                .Select(GetGame)
                .Where(x => x.MaxReds() <= 12 && x.MaxGreens() <= 13 && x.MaxBlues() <= 14)
                .Sum(x => x.GameId);

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day02");

            var result = input
                .Select(GetGame)
                .Sum(x => x.GetPower());

            Console.WriteLine(result);
        }

        private static CubeGame GetGame(string input)
        {
            var parts = input.Split(':');
            var gameId = int.Parse(parts[0][5..]);

            var subsetsStr = parts[1].Trim().Split(';');

            var subsets = new List<SubSet>();
            foreach (var subsetStr in subsetsStr)
            {
                var subsetItem = new SubSet();
                var items = subsetStr.Trim().Split(",");

                foreach (var item in items)
                {
                    var itemParts = item.Trim().Split(" ");

                    if (itemParts[1] == "blue") subsetItem.Blues = int.Parse(itemParts[0]);
                    if (itemParts[1] == "red") subsetItem.Reds = int.Parse(itemParts[0]);
                    if (itemParts[1] == "green") subsetItem.Greens = int.Parse(itemParts[0]);
                }

                subsets.Add(subsetItem);
            }

            return new CubeGame { GameId = gameId, SubSets = subsets };
        }
    }

    internal class CubeGame
    {
        public int GameId { get; set; }
        public IList<SubSet> SubSets { get; set; }

        public int MaxReds() => SubSets.Max(s => s.Reds);
        public int MaxGreens() => SubSets.Max(s => s.Greens);
        public int MaxBlues() => SubSets.Max(s => s.Blues);

        public int GetPower() => MaxBlues() * MaxReds() * MaxGreens();
    }

    internal class SubSet
    {
        public SubSet() { Reds = 0; Greens = 0; Blues = 0; }

        public int Reds { get; set; }
        public int Blues { get; set; }
        public int Greens { get; set; }
    }
}
