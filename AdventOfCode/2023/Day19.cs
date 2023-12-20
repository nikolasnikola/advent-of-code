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
            var input = Reader.ReadAsString("2023", "Day19");
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
                    var anyAcceptance = currentRule.Items.Any(i => i.RuleNameIfTrue == ruleNameLookup);

                    if (anyAcceptance)
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
                            }
                            andConditions.Add(new ExitCondition()
                            {
                                PartName = item.PartName,
                                Operator = item.Operator,
                                Value = item.Value,
                                IsNegation = true,
                            });
                        }
                        var newCondition = currentConditions[0];
                        newCondition.OrConditions = new List<ExitCondition>(currentConditions.Skip(1));

                        if (acceptedIfNothingTrue)
                        {
                            var firstItem = currentRule.Items[0];
                            var otherConditions = new List<ExitCondition>();

                            foreach (var item in currentRule.Items.Skip(1))
                            {
                                otherConditions.Add(new ExitCondition()
                                {
                                    PartName = item.PartName,
                                    Operator = item.Operator,
                                    Value = item.Value,
                                    IsNegation = true,
                                });
                            }
                            newCondition.OrConditions.Add(new ExitCondition()
                            {
                                PartName = firstItem.PartName,
                                Operator = firstItem.Operator,
                                Value = firstItem.Value,
                                IsNegation = true,
                                AndConditions = otherConditions
                            });
                        }

                        conditions.Add(newCondition);
                    }

                    if (!anyAcceptance && acceptedIfNothingTrue)
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

                    if (!finished)
                    {
                        ruleNameLookup = currentRule.Name;
                        currentRule = rules.Single(x => x.Value.RuleNameIfNothingTrue == currentRule.Name || x.Value.Items.Any(i => i.RuleNameIfTrue == currentRule.Name)).Value;
                    }
                }
                exitConditions.Add(new Exit { Conditions = conditions });
            }

            var exitsWithBranches = new List<Exit>();
            foreach (var exit in exitConditions)
            {
                var lastCondition = exit.Conditions[0];

                if (lastCondition.OrConditions != null && lastCondition.OrConditions.Any())
                {
                    var otherConditions = exit.Conditions.Skip(1);
                    foreach (var orCond in lastCondition.OrConditions)
                    {
                        var previousItems = new List<ExitCondition>(otherConditions) { orCond };
                        exitsWithBranches.Add(new Exit() { Conditions = previousItems });
                    }
                }
                exitsWithBranches.Add(exit);
            }

            long result = 0;

            foreach (var exit in exitsWithBranches)
            {
                long currentResult = 1;

                var dictionary = new Dictionary<string, List<int>>()
                {
                    {"x",  Enumerable.Range(1, 4000).ToList()},
                    {"m",  Enumerable.Range(1, 4000).ToList()},
                    {"a",  Enumerable.Range(1, 4000).ToList()},
                    {"s",  Enumerable.Range(1, 4000).ToList()},
                };

                foreach (var condition in exit.Conditions)
                {
                    dictionary[condition.PartName] = dictionary[condition.PartName].Where(i =>
                    {
                        bool greaterThanCondition = condition.IsNegation ? i <= condition.Value : i > condition.Value;
                        bool lessThanCondition = condition.IsNegation ? i >= condition.Value : i < condition.Value;
                        return condition.Operator == Operator.GreaterThan ? greaterThanCondition : lessThanCondition;
                    }).ToList();

                    foreach (var andCondition in condition.AndConditions)
                    {
                        dictionary[andCondition.PartName] = dictionary[andCondition.PartName].Where(i =>
                        {
                            bool greaterThanCondition = andCondition.IsNegation ? i <= andCondition.Value : i > andCondition.Value;
                            bool lessThanCondition = andCondition.IsNegation ? i >= andCondition.Value : i < andCondition.Value;
                            return andCondition.Operator == Operator.GreaterThan ? greaterThanCondition : lessThanCondition;
                        }).ToList();
                    }
                }

                foreach (var items in dictionary.Values)
                {
                    currentResult *= items.Count;
                }

                result += currentResult;
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