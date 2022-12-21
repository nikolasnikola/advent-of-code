using AdventOfCode2022.Dto.Day21;

namespace AdventOfCode2022._2022
{
    internal static class Day21
    {
        internal static void Part2()
        {
            var items = Reader.ReadAsStringList("Day21");
            var monkeyYells = GetMonkeyYells(items);

            var rootMonkey = monkeyYells.Single(m => m.MonkeyName == "root");
            var human = monkeyYells.Single(m => m.MonkeyName == "humn");
            human.Number = null;

            var m1 = monkeyYells.Single(m => m.MonkeyName == rootMonkey.OperationMonkey1);
            var m2 = monkeyYells.Single(m => m.MonkeyName == rootMonkey.OperationMonkey2);

            var m1dependsOnHuman = CheckIfDependsOnHuman(m1, monkeyYells);
            var m2dependsOnHuman = CheckIfDependsOnHuman(m2, monkeyYells);

            long result = 0;

            if (m1dependsOnHuman)
            {
                GetResult(m2, monkeyYells);
                result = FindHumanResult(m1, monkeyYells, m2.Number!.Value);
            }
            else
            {
                GetResult(m1, monkeyYells);
                result = FindHumanResult(m2, monkeyYells, m1.Number!.Value);
            }

            Console.Write(result);
        }

        internal static void Part1()
        {
            var items = Reader.ReadAsStringList("Day21");
            var monkeyYells = GetMonkeyYells(items);

            var rootMonkey = monkeyYells.Single(m => m.MonkeyName == "root");

            GetResult(rootMonkey, monkeyYells);

            Console.WriteLine(rootMonkey.Number);
        }

        static long FindHumanResult(MonkeyYell monkeyWithDependency, List<MonkeyYell> monkeyYells, long result)
        {
            var m1 = monkeyYells.Single(m => m.MonkeyName == monkeyWithDependency.OperationMonkey1);
            var m2 = monkeyYells.Single(m => m.MonkeyName == monkeyWithDependency.OperationMonkey2);

            var m1dependsOnHuman = CheckIfDependsOnHuman(m1, monkeyYells);
            var m2dependsOnHuman = CheckIfDependsOnHuman(m2, monkeyYells);

            if (m1dependsOnHuman)
            {
                if (!m2.Number.HasValue) GetResult(m2, monkeyYells);

                var newResult = GetNewResult(result, m2.Number!.Value, true, monkeyWithDependency.Operation);

                return m1.MonkeyName == "humn" ? newResult : FindHumanResult(m1, monkeyYells, newResult);
            }
            else
            {
                if (!m1.Number.HasValue) GetResult(m1, monkeyYells);

                var newResult = GetNewResult(result, m1.Number!.Value, false, monkeyWithDependency.Operation);

                return m2.MonkeyName == "humn" ? newResult : FindHumanResult(m2, monkeyYells, newResult);
            }
        }

        static long GetNewResult(long result, long knownMonkey, bool isUnknownMonkeyFirst, char operation) => operation switch
        {
            '+' => result - knownMonkey,
            '*' => result / knownMonkey,
            '-' => isUnknownMonkeyFirst ? result + knownMonkey : knownMonkey - result,
            _ => isUnknownMonkeyFirst ? result * knownMonkey : knownMonkey / result,
        };

        static bool CheckIfDependsOnHuman(MonkeyYell monkey, List<MonkeyYell> monkeyYells)
        {
            if (monkey.MonkeyName == "humn") return true;
            if (string.IsNullOrWhiteSpace(monkey.OperationMonkey1)) return false;

            var m1 = monkeyYells.Single(m => m.MonkeyName == monkey.OperationMonkey1);
            var m2 = monkeyYells.Single(m => m.MonkeyName == monkey.OperationMonkey2);

            if (m1.MonkeyName == "humn" || m2.MonkeyName == "humn") return true;

            var m1DependsOnHuman = m1.OperationMonkey1 == "humn" || m1.OperationMonkey2 == "humn";
            var m2DependsOnHuman = m2.OperationMonkey1 == "humn" || m1.OperationMonkey2 == "humn";

            if (m1DependsOnHuman || m2DependsOnHuman) return true;

            m1DependsOnHuman = CheckIfDependsOnHuman(m1, monkeyYells);
            m2DependsOnHuman = CheckIfDependsOnHuman(m2, monkeyYells);

            return m1DependsOnHuman || m2DependsOnHuman;
        }

        static void GetResult(MonkeyYell monkey, List<MonkeyYell> monkeyYells)
        {
            var m1 = monkeyYells.Single(m => m.MonkeyName == monkey.OperationMonkey1);
            var m2 = monkeyYells.Single(m => m.MonkeyName == monkey.OperationMonkey2);

            if (!m1.Number.HasValue)
            {
                GetResult(m1, monkeyYells);
            }

            if (!m2.Number.HasValue)
            {
                GetResult(m2, monkeyYells);
            }

            monkey.CalculateOperation(m1.Number!.Value, m2.Number!.Value);
        }

        static List<MonkeyYell> GetMonkeyYells(string[] items)
        {
            List<MonkeyYell> monkeyYells = new();

            foreach (var item in items)
            {
                var nameYell = item.Split(": ");

                var monkeyName = nameYell[0];
                var monkeyYell = new MonkeyYell(monkeyName);

                var yelling = nameYell[1].Trim();


                if (int.TryParse(yelling, out int num))
                {
                    monkeyYell.Number = num;
                }
                else
                {
                    var operationItems = yelling.Split(" ");
                    monkeyYell.OperationMonkey1 = operationItems[0];
                    monkeyYell.Operation = char.Parse(operationItems[1]);
                    monkeyYell.OperationMonkey2 = operationItems[2];
                }

                monkeyYells.Add(monkeyYell);
            }

            return monkeyYells;
        }
    }
}
