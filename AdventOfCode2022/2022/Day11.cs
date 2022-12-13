using AdventOfCode2022.Dto.Day11;

namespace AdventOfCode2022._2022
{
    internal static class Day11
    {
        internal static void Part2()
        {
            var roundTurns = Reader.ReadAsString("Day11Test");

            var result = ProceedRounds2(roundTurns, 1, 20);

            Console.WriteLine("\n\n**************************");
            Console.WriteLine("FINAL RESULT: " + result);
            Console.WriteLine("**************************");
        }

        internal static void Part1()
        {
            var roundTurns = Reader.ReadAsString("Day11Test");

            var result = ProceedRounds2(roundTurns, 3, 20);

            Console.WriteLine("FINAL RESULT: " + result);
        }

        static long ProceedRounds2(string roundTurns, int worryLevelDivide, int roundsCount)
        {
            var turns = roundTurns.Split("\r\n\r\n");
            var totalMonkeys = turns.Length;

            var monkeyItems = GetStartingItems2(turns);
            Dictionary<int, int> monkeyActivities = Enumerable
                .Range(0, totalMonkeys)
                .Select(i => KeyValuePair.Create(i, 0))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var i in Enumerable.Range(0, roundsCount))
            {
                Console.WriteLine("** Proceed round: " + i);
                foreach (var turn in turns)
                {
                    var turnRules = turn.Split("\r\n");

                    var currentMonkey = int.Parse(turnRules[0].Substring(7, 1));
                    Console.WriteLine(".... proceed monkey: " + currentMonkey);
                    var currentItems = monkeyItems.Where(i => i.CurrentMonkeyIndex == currentMonkey);

                    foreach (var item in currentItems)
                    {
                        ProceedOperation2(turnRules[2].Trim(), item);

                        var ss = CheckTest2(item, turnRules[3].Trim());

                        var monkeyToPass = CheckTest2(item, turnRules[3].Trim())
                            ? GetMonkeyIdIfTrue(turnRules[4].Trim())
                            : GetMonkeyIdIfFalse(turnRules[5].Trim());

                        item.CurrentMonkeyIndex = monkeyToPass;
                        monkeyActivities[currentMonkey]++;
                    }
                }
            }

            return monkeyActivities.Select(a => (long)a.Value).OrderByDescending(x => x).Take(2).Aggregate((a, x) => a * x);
        }


        static int ProceedRounds(string roundTurns, int worryLevelDivide, int roundsCount)
        {
            var turns = roundTurns.Split("\r\n\r\n");
            var totalMonkeys = turns.Length;

            Dictionary<int, List<int>> monkeyItems = GetStartingItems(turns);
            Dictionary<int, int> monkeyActivities = Enumerable
                .Range(0, totalMonkeys)
                .Select(i => KeyValuePair.Create(i, 0))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var i in Enumerable.Range(0, roundsCount))
            {
                Console.WriteLine("** Proceed round: " + i);
                foreach (var turn in turns)
                {
                    var turnRules = turn.Split("\r\n");

                    var currentMonkey = int.Parse(turnRules[0].Substring(7, 1));
                    Console.WriteLine(".... proceed monkey: " + currentMonkey);
                    var currentItems = monkeyItems[currentMonkey];

                    while (currentItems.Any())
                    {
                        var item = currentItems[0];

                        var newValue = ProceedOperation(turnRules[2].Trim(), item, worryLevelDivide);

                        var monkeyToPass = CheckTest(newValue, turnRules[3].Trim())
                            ? GetMonkeyIdIfTrue(turnRules[4].Trim())
                            : GetMonkeyIdIfFalse(turnRules[5].Trim());

                        monkeyItems[monkeyToPass].Add(newValue);
                        monkeyItems[currentMonkey].Remove(item);
                        monkeyActivities[currentMonkey]++;
                    }
                }
            }

            return monkeyActivities.Select(a => a.Value).OrderByDescending(x => x).Take(2).Aggregate((a, x) => a * x);
        }

        static bool CheckTest2(MonkeyItem monkeyItem, string turnRule)
        {
            int divisableBy = int.Parse(turnRule[19..]);

            var resultItem = monkeyItem.Divisables.Single(d => d.DivisableNumber == divisableBy);

            return resultItem.IsDivisable;
        }


        static bool CheckTest(int newValue, string turnRule) => newValue % int.Parse(turnRule[19..]) == 0;

        static int GetMonkeyIdIfTrue(string turnRule) => int.Parse(turnRule[25..]);
        static int GetMonkeyIdIfFalse(string turnRule) => int.Parse(turnRule[26..]);

        static void ProceedOperation2(string operationRule, MonkeyItem monkeyItem)
        {
            var operation = operationRule[17..].Split(" ");
            if (operation[2] == "old")
            {
                foreach (var div in monkeyItem.Divisables)
                {
                    div.Modulo = (div.Modulo * div.Modulo) % div.DivisableNumber;
                    div.IsDivisable = div.Modulo == 0;
                }

                return;
            }

            var by = int.Parse(operation[2]);

            foreach (var div in monkeyItem.Divisables)
            {

                if (operation[1] == "+")
                {
                    div.Modulo = (div.Modulo + by) % div.DivisableNumber;
                    div.IsDivisable = div.Modulo == 0;
                }
                else
                {
                    div.Modulo = (div.Modulo * by) % div.DivisableNumber;
                    div.IsDivisable = div.Modulo == 0;
                }
            }
        }

        static int ProceedOperation(string operationRule, int oldValue, int worryLevelDivide)
        {
            var operation = operationRule[17..].Split(" ");
            var by = operation[2] == "old" ? oldValue : int.Parse(operation[2]);

            var newValue = operation[1] == "+"
                ? oldValue + by
                : oldValue * by;

            return newValue / worryLevelDivide;
        }

        static List<MonkeyItem> GetStartingItems2(string[] turns)
        {
            List<MonkeyItem> monkeyItems = new();

            var divisableTests = GetAllDivisableTests(turns);

            foreach (var turn in turns)
            {
                var turnRules = turn.Split("\r\n");
                var monkeyId = int.Parse(turnRules[0].Substring(7, 1));

                var items = turnRules[1][18..].Split(',').Select(i => int.Parse(i));

                foreach (var item in items)
                {
                    var mi = new MonkeyItem() { CurrentMonkeyIndex = monkeyId, StartValue = item };

                    var divisables = divisableTests.Select(x => new DivisableInfo() { DivisableNumber = x, IsDivisable = item % x == 0, Modulo = item % x });

                    mi.Divisables = divisables.ToList();

                    monkeyItems.Add(mi);
                }
            }

            return monkeyItems;
        }

        static int[] GetAllDivisableTests(string[] turns)
        {
            var divisableTests = new List<int>();

            foreach (var turn in turns)
            {
                var turnRules = turn.Split("\r\n").Select(t => t.Trim());

                divisableTests.Add(int.Parse(turnRules.Single(t => t.StartsWith("Test:"))[19..]));
            }

            return divisableTests.ToArray();
        }

        static Dictionary<int, List<int>> GetStartingItems(string[] turns)
        {
            Dictionary<int, List<int>> monkeyItems = new();

            foreach (var turn in turns)
            {
                var turnRules = turn.Split("\r\n");
                var monkeyId = int.Parse(turnRules[0].Substring(7, 1));

                var items = turnRules[1][18..].Split(',').Select(i => int.Parse(i)).ToList();

                monkeyItems.Add(monkeyId, items);
            }

            return monkeyItems;
        }
    }
}
