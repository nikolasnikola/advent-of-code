using System.Data.Common;

namespace AdventOfCode._2023
{
    internal static class Day04
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day04");

            var result = input
                .Select(GetScratchcard)
                .Sum(c => c.GetPoints());

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day04");

            var cards = input
                .Select(GetScratchcard)
                .ToList();

            var copies = new Dictionary<int, int>();

            foreach (var card in cards)
            {
                if (copies.ContainsKey(card.CardId))
                {
                    card.AddCopies(copies[card.CardId]);
                }

                var winningCount = card.GetOwnedWinningNumbers().Count;

                for (int i = 1; i <= winningCount; i++)
                {
                    if (!copies.ContainsKey(card.CardId + i)) copies.Add(card.CardId + i, 0);
                    copies[card.CardId + i] += card.NumberOfInstances;
                }
            }

            var result = cards.Sum(c => c.NumberOfInstances);

            Console.WriteLine(result);
        }

        private static Scratchcard GetScratchcard(string input)
        {
            var inputParts = input.Split(':');
            var scratchcard = new Scratchcard() { CardId = int.Parse(inputParts[0].Split(" ").Last()) };

            var numberParts = inputParts[1].Split("|");

            scratchcard.WinningNumbers = numberParts[0].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
            scratchcard.OwnedNumbers = numberParts[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();

            return scratchcard;
        }
    }

    internal class Scratchcard
    {
        public Scratchcard()
        {
            NumberOfInstances = 1;
        }

        public int CardId { get; set; }
        public IList<int> WinningNumbers { get; set; }
        public IList<int> OwnedNumbers { get; set; }

        public int NumberOfInstances { get; set; }

        public void AddCopies(int numberOfCopies)
        {
            NumberOfInstances += numberOfCopies;
        }

        public IList<int> GetOwnedWinningNumbers() => OwnedNumbers.Intersect(WinningNumbers).ToList();
        public double GetPoints()
        {
            if (!GetOwnedWinningNumbers().Any()) return 0;
            return Math.Pow(2, GetOwnedWinningNumbers().Count - 1);
        }
    }
}
