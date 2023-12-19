using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023
{
    internal static class Day19
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2023", "Day19");
            var splitted = input.Split("\r\n\r\n");

            var rules = splitted[0].Split("\r\n").Select(GetRule).ToDictionary(x => x.Name, x => x);
            var parts = splitted[1].Split("\r\n").Select(GetParts).ToList();

            var result = 0;
            foreach (var part in parts)
            {
                result += ProceedPart(part, rules);
            }

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsString("2023", "Day19Test");
            var splitted = input.Split("\r\n\r\n");

            var rules = splitted[0].Split("\r\n").Select(GetRule).ToDictionary(x => x.Name, x => x);

            var rulesWithExit = rules
                .Where(x => x.Value.RuleNameIfNothingTrue == "A" || x.Value.Items.Any(i => i.RuleNameIfTrue == "A"))
                .ToList();

            var exitConditions = new List<Exit>();

            foreach (var rule in rulesWithExit)
            {
                bool finished = false;
                var currentRule = rules[rule.Key];
                var ruleNameLookup = "A";
                var conditions = new List<ExitCondition>();

                while (!finished)
                {
                    if (currentRule.Name == "in") finished = true;

                    var acceptedIfNothingTrue = currentRule.RuleNameIfNothingTrue == ruleNameLookup;
                    if (acceptedIfNothingTrue)
                    {
                        var firstItem = currentRule.Items[0];
                        var andConditions = new List<ExitCondition>();

                        foreach (var item in currentRule.Items.Skip(1))
                        {
                            andConditions.Add(new ExitCondition()
                            {
                                PartName = item.PartName,
                                Operator = item.Operator,
                                Value = item.Value,
                                IsNegation = true,
                            });
                        }
                        conditions.Add(new ExitCondition()
                        {
                            PartName = firstItem.PartName,
                            Operator = firstItem.Operator,
                            Value = firstItem.Value,
                            IsNegation = true,
                            AndConditions = andConditions
                        });
                    }

                    if (currentRule.Items.Any(i => i.RuleNameIfTrue == ruleNameLookup))
                    {
                        var andConditions = new List<ExitCondition>();

                        var currentConditions = new List<ExitCondition>();

                        foreach (var item in currentRule.Items)
                        {
                            if (item.RuleNameIfTrue == ruleNameLookup)
                            {
                                currentConditions.Add(new ExitCondition()
                                {
                                    PartName = item.PartName,
                                    Operator = item.Operator,
                                    Value = item.Value,
                                    IsNegation = false,
                                    AndConditions = new List<ExitCondition>(andConditions),
                                });
                                andConditions = new List<ExitCondition>();
                            }
                            else
                            {
                                andConditions.Add(new ExitCondition()
                                {
                                    PartName = item.PartName,
                                    Operator = item.Operator,
                                    Value = item.Value,
                                    IsNegation = true,
                                });
                            }
                        }
                        var newCondition = currentConditions.First();
                        newCondition.OrConditions = new List<ExitCondition>(currentConditions.Skip(1)); // create exit flow from all or conditions
                        conditions.Add(newCondition);
                    }

                    if (!finished)
                    {
                        ruleNameLookup = currentRule.Name;
                        currentRule = rules.Single(x => x.Value.RuleNameIfNothingTrue == currentRule.Name || x.Value.Items.Any(i => i.RuleNameIfTrue == currentRule.Name)).Value;
                    }
                }
                exitConditions.Add(new Exit { Conditions = conditions });
            }

            long result = 1;

            foreach (var exit in exitConditions)
            {
                var dictionary = new Dictionary<string, IEnumerable<int>>()
                {
                    {"x",  Enumerable.Range(1, 3999)},
                    {"m",  Enumerable.Range(1, 3999)},
                    {"a",  Enumerable.Range(1, 3999)},
                    {"s",  Enumerable.Range(1, 3999)},
                };

                foreach (var condition in exit.Conditions)
                {
                }
            }

            Console.WriteLine(result);
        }

        private static int ProceedPart(Part part, Dictionary<string, Rule> rules)
        {
            var currentRuleName = "in";

            while (currentRuleName != "A" && currentRuleName != "R")
            {
                var rule = rules[currentRuleName];
                var newRuleFound = false;

                foreach (var item in rule.Items)
                {
                    var itemValue = part.PartItems[item.PartName];

                    if (item.Operator == Operator.GreaterThan ? itemValue > item.Value : itemValue < item.Value)
                    {
                        currentRuleName = item.RuleNameIfTrue;
                        newRuleFound = true;
                        break;
                    }
                }

                if (!newRuleFound) { currentRuleName = rule.RuleNameIfNothingTrue; }
            }

            return currentRuleName == "A" ? part.GetSum() : 0;
        }

        private static Rule GetRule(string input)
        {
            var match = Regex.Match(input, @"(\S+){(\S+)}");
            var ruleName = match.Groups[1].Value;
            var conditions = match.Groups[2].Value.Split(',');

            var ruleItems = new List<RuleItem>();
            for (int i = 0; i < conditions.Length - 1; i++)
            {
                var condition = conditions[i];
                var ruleItem = new RuleItem();
                if (condition.Contains('>'))
                {
                    var splittedOperator = condition.Split('>');

                    var splittedCondition = splittedOperator[1].Split(':');

                    ruleItem.PartName = splittedOperator[0];
                    ruleItem.Value = int.Parse(splittedCondition[0]);
                    ruleItem.RuleNameIfTrue = splittedCondition[1];
                    ruleItem.Operator = Operator.GreaterThan;
                }
                else
                {
                    var splittedOperator = condition.Split('<');

                    var splittedCondition = splittedOperator[1].Split(':');

                    ruleItem.PartName = splittedOperator[0];
                    ruleItem.Value = int.Parse(splittedCondition[0]);
                    ruleItem.RuleNameIfTrue = splittedCondition[1];
                    ruleItem.Operator = Operator.LessThan;
                }

                ruleItems.Add(ruleItem);
            }

            var ruleNameIfNothingTrue = conditions[conditions.Length - 1];

            return new Rule { Name = ruleName, Items = ruleItems, RuleNameIfNothingTrue = ruleNameIfNothingTrue };
        }

        private static Part GetParts(string input)
        {
            var match = Regex.Match(input, @"{(\S+)}");
            var partItems = match.Groups[1].Value.Split(',');
            var dictionary = new Dictionary<string, int>();

            foreach (var item in partItems)
            {
                var splitted = item.Split("=");
                dictionary.Add(splitted[0], int.Parse(splitted[1]));
            }

            return new Part() { PartItems = dictionary };
        }
    }
}

public class Rule
{
    public string Name { get; set; }
    public IList<RuleItem> Items { get; set; }
    public string RuleNameIfNothingTrue { get; set; }

}

public class RuleItem
{
    public string PartName { get; set; }
    public Operator Operator { get; set; }
    public int Value { get; set; }
    public string RuleNameIfTrue { get; set; }
}

public class Part
{
    public Dictionary<string, int> PartItems { get; set; }
    public int GetSum() => PartItems.Values.Sum();
}

public class Exit
{
    public IList<ExitCondition> Conditions { get; set; }
}

public class ExitCondition
{
    public string PartName { get; set; }
    public Operator Operator { get; set; }
    public int Value { get; set; }
    public bool IsNegation { get; set; }
    public IList<ExitCondition> OrConditions { get; set; }
    public IList<ExitCondition> AndConditions { get; set; }
}

public enum Operator
{
    GreaterThan,
    LessThan,
}

/*
px => A : (m>2090 && !a<2006) || (x <2500 && s > 800)
in => px : s<1351
-------------------------------


pv => A : !a>1716
hdj => pv : !m>838
qqz => hdj : !s>2770 && m<1801
in => qqz : s!<1351 
--------------------------------
 */