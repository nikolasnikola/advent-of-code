using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal static class Day07
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2020", "Day07");
            var rules = GetRules(input);

            var target = "shiny gold";

            var bags = GetTarget(target, rules);

            Console.WriteLine(bags.Count());
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2020", "Day07");
            var rules = GetRules(input);
            var target = rules.Single(r => r.MainBagColor == "shiny gold");

            var totalBags = CountTarget(target, rules);

            Console.WriteLine(totalBags);
        }

        static int CountTarget(BagRule target, IEnumerable<BagRule> rules)
        {
            var total = 0;

            foreach (var c in target.Contains ?? Enumerable.Empty<BagContain>())
            {
                total += c.Quantity;

                var subTarget = rules.Single(r => r.MainBagColor == c.Color);
                total += c.Quantity * CountTarget(subTarget, rules);
            }

            return total;
        }

        static IEnumerable<string> GetTarget(string target, IEnumerable<BagRule> rules)
        {
            var ret = new HashSet<string>();
            var directTargets = rules.Where(r => r.Contains?.Any(c => c.Color == target) ?? false).Select(r => r.MainBagColor);

            if (directTargets.Any())
            {
                foreach (var t in directTargets)
                {
                    ret.Add(t);
                }
                
                foreach (var dt in directTargets)
                {
                    var innerTargets = GetTarget(dt, rules);
                    if (innerTargets.Any())
                    {
                        foreach (var t in innerTargets)
                        {
                            ret.Add(t);
                        }
                    }
                }
            }

            return ret;
        }

        static List<BagRule> GetRules(string[] input)
        {
            return input.Select(r =>
                            Regex.Match(r, @"^([\w\s]+) bags contain (.+)\.$"))
                .Select(match =>
                new BagRule()
                {
                    MainBagColor = match.Groups[1].Value.Trim(),
                    Contains = GetBagContains(match.Groups[2].Value)
                })
                .ToList();
        }

        static IEnumerable<BagContain> GetBagContains(string input)
        {
            if (input == "no other bags") return null;

            var ret = new List<BagContain>();
            foreach (var contain in input.Split(", "))
            {
                var match = Regex.Match(contain, @"^(\d+) ([\w\s]+?) (bag|bags)");
                if (match != null)
                {
                    ret.Add(new BagContain
                    {
                        Quantity = int.Parse(match.Groups[1].Value),
                        Color = match.Groups[2].Value,
                    });
                }
            }
            return ret;
        }
    }

    class BagRule
    {
        public string MainBagColor { get; set; }
        public IEnumerable<BagContain> Contains { get; set; }
    }

    class BagContain
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
    }
}
