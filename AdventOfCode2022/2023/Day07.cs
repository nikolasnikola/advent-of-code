using NUnit.Framework;

namespace AdventOfCode._2023
{
    internal static class Day07
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day07");

            var hands = input.Select(x => GetHand(x, false)).OrderBy(x => (int)x.GetHandType()).ThenBy(x => x.GetHandStrenght()).ToArray();

            var result = 0;

            for (int i=0; i < hands.Length; i++)
            {
                result += hands[i].Bid * (i+1);
            }

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day07");

            var hands = input.Select(x => GetHand(x, true)).OrderBy(x => (int)x.GetHandType()).ThenBy(x => x.GetHandStrenght()).ToArray();

            var result = 0;

            for (int i = 0; i < hands.Length; i++)
            {
                result += hands[i].Bid * (i + 1);
            }

            Console.WriteLine(result);
        }

        private static CamelCardHand GetHand(string input, bool withJokers)
        {
            var splitted = input.Split(" ");

            return new CamelCardHand(withJokers)
            {
                HandSet = splitted[0],
                Bid = int.Parse(splitted[1]),
            };
        }
    }

    internal class CamelCardHand
    {
        private readonly bool _withJokers;

        public CamelCardHand(bool withJokers)
        {
            _withJokers = withJokers;
        }

        public string HandSet { get; set; }

        public int Bid { get; set; }

        public HandType GetHandType()
        {
            if (_withJokers) return GetHandTypeWithJokers();
            if (CheckRepeats(5)) return HandType.FiveOfKind;
            if (CheckRepeats(4)) return HandType.FourOfKind;
            if (CheckRepeats(3) && CheckRepeats(2)) return HandType.FullHouse;
            if (CheckRepeats(3)) return HandType.ThreeOfKind;
            if (CheckRepeats(2, 2)) return HandType.TwoPair;
            if (CheckRepeats(2)) return HandType.OnePair;

            return HandType.HighCard;
        }

        public double GetHandStrenght()
        {
            var jStreinght = _withJokers ? 1 : 11;
            var strenghtMap = new Dictionary<char, int>()
            {
                { 'A', 14 },
                { 'K', 13 },
                { 'Q', 12 },
                { 'J', jStreinght },
                { 'T', 10 },
                { '9', 9 },
                { '8', 8 },
                { '7', 7 },
                { '6', 6 },
                { '5', 5 },
                { '4', 4 },
                { '3', 3 },
                { '2', 2 },
            };

            return Math.Pow(15, 4) * strenghtMap[HandSet[0]]
                + Math.Pow(15, 3) * strenghtMap[HandSet[1]]
                + Math.Pow(15, 2) * strenghtMap[HandSet[2]]
                + Math.Pow(15, 1) * strenghtMap[HandSet[3]]
                + Math.Pow(15, 0) * strenghtMap[HandSet[4]];
        }

        private HandType GetHandTypeWithJokers()
        {
            if (CheckRepeatsWithJokers(5)) return HandType.FiveOfKind;
            if (CheckRepeatsWithJokers(4)) return HandType.FourOfKind;
            if (CheckFullHouseWithJokers()) return HandType.FullHouse;
            if (CheckRepeatsWithJokers(3)) return HandType.ThreeOfKind;
            if (CheckTwoPairsWithJokers()) return HandType.TwoPair;
            if (CheckRepeatsWithJokers(2)) return HandType.OnePair;

            return HandType.HighCard;
        }

        private bool CheckRepeats(int numberOfRepeats, int times = 1)
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            foreach (char c in HandSet)
            {
                if (charCount.ContainsKey(c)) charCount[c]++;
                else charCount[c] = 1;
            }

            return charCount.Values.Count(v => v == numberOfRepeats) == times;
        }

        private bool CheckRepeatsWithJokers(int numberOfRepeats)
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            var jokers = HandSet.Count(x => x == 'J');
            var nonJokers = HandSet.Where(x => x != 'J');

            if (jokers == 5 && numberOfRepeats == 5) return true;

            foreach (char c in nonJokers)
            {
                if (charCount.ContainsKey(c)) charCount[c]++;
                else charCount[c] = 1;
            }

            return charCount.Values.Any(v => v+jokers == numberOfRepeats);
        }

        private bool CheckFullHouseWithJokers()
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            var jokers = HandSet.Count(x => x == 'J');
            var nonJokers = HandSet.Where(x => x != 'J');

            foreach (char c in nonJokers)
            {
                if (charCount.ContainsKey(c)) charCount[c]++;
                else charCount[c] = 1;
            }

            if (jokers == 3) return true;

            if (jokers == 2) return charCount.ContainsValue(2);

            if (jokers == 1) return charCount.ContainsValue(3) || charCount.Values.Count(v => v == 2) == 2;

            return charCount.ContainsValue(2) && charCount.ContainsValue(3);
        }

        private bool CheckTwoPairsWithJokers()
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            var jokers = HandSet.Count(x => x == 'J');
            var nonJokers = HandSet.Where(x => x != 'J');

            if (jokers > 0) return false;

            foreach (char c in nonJokers)
            {
                if (charCount.ContainsKey(c)) charCount[c]++;
                else charCount[c] = 1;
            }

            return charCount.Values.Count(v => v == 2) == 2;
        }
    }

    internal enum HandType
    {
        FiveOfKind = 7,
        FourOfKind = 6,
        FullHouse = 5,
        ThreeOfKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1
    }

    public class CardTests
    {
        [Test]
        public void GetHandTypeTests()
        {
            var t1 = "JJJJJ";
            var t2 = "JJ234";
            var t3 = "J2234";

            var result = new CamelCardHand(true) { HandSet = t1 };
            Assert.AreEqual(HandType.FiveOfKind, result.GetHandType());

            var result2 = new CamelCardHand(true) { HandSet = t2 };
            Assert.AreEqual(HandType.ThreeOfKind, result2.GetHandType());

            var result3 = new CamelCardHand(true) { HandSet = t3 };
            Assert.AreEqual(HandType.ThreeOfKind, result3.GetHandType());
        }

        [Test]
        public void GetHandStrenghtTests()
        {
            var t1 = "AAAA2";
            var t2 = "AAAAJ";

            var c1 = new CamelCardHand(true) { HandSet = t1 };
            var c2 = new CamelCardHand(true) { HandSet = t2 };
            Assert.IsTrue(c1.GetHandStrenght() > c2.GetHandStrenght());
        }
    }
}
