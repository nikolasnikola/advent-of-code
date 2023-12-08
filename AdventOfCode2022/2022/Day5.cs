namespace AdventOfCode._2022
{
    internal static class Day5
    {
        internal static void Part2()
        {
            var stacks = Reader.ReadAsStringList("2022", "Day5-1");
            var procedure = Reader.ReadAsStringList("2022", "Day5-2");

            var stackPlan = GetStackPlan(stacks);

            ApplyProcedure(stackPlan, procedure, ApplyProcedureItem2);
        }

        internal static void Part1()
        {
            var stacks = Reader.ReadAsStringList("2022", "Day5-1");
            var procedure = Reader.ReadAsStringList("2022", "Day5-2");

            var stackPlan = GetStackPlan(stacks);

            ApplyProcedure(stackPlan, procedure, ApplyProcedureItem);
        }

        static void ApplyProcedure(List<List<char>> stacks, string[] procedure, Action<List<List<char>>, int, int, int> procedureAction)
        {
            foreach (var p in procedure)
            {
                var procedureParams = GetProcedureParameters(p);
                procedureAction(stacks, procedureParams.Item1, procedureParams.Item2, procedureParams.Item3);
            }

            foreach (var item in stacks)
            {
                Console.Write(item.Last());
            }
        }

        static void ApplyProcedureItem2(List<List<char>> stacks, int count, int from, int to)
        {
            var stackFrom = stacks[from - 1];
            var items = stackFrom.GetRange(stackFrom.Count - count, count);

            stackFrom.RemoveRange(stackFrom.Count - count, count);
            stacks[to - 1].AddRange(items);
        }

        static void ApplyProcedureItem(List<List<char>> stacks, int count, int from, int to)
        {
            for (int i = 0; i < count; i++)
            {
                var stackFrom = stacks[from - 1];
                var item = stackFrom[^1];

                stackFrom.RemoveAt(stackFrom.Count - 1);
                stacks[to - 1].Add(item);
            }
        }

        static (int, int, int) GetProcedureParameters(string procedureItem)
        {
            var count = procedureItem[5..^12];

            var from = procedureItem.Substring(procedureItem.Length - 6, 1);

            var to = procedureItem[^1].ToString();

            return (int.Parse(count), int.Parse(from), int.Parse(to));
        }

        static List<List<char>> GetStackPlan(string[] stacks)
        {
            List<List<char>> resultList = new();

            var stackRows = stacks.Select(r => ProcessOneRow(r)).ToList();

            var totalStacks = stackRows.First().Length;

            foreach (var item in Enumerable.Range(0, totalStacks))
            {
                resultList.Add(new List<char>());
            }

            for (int i = stackRows.Count - 1; i >= 0; i--)
            {
                var currentRow = stackRows[i];
                for (int j = 0; j < currentRow.Length; j++)
                {
                    if (currentRow[j] != default(char))
                    {
                        resultList[j].Add(currentRow[j]);
                    }
                }
            }

            return resultList;
        }

        static char[] ProcessOneRow(string stackRow)
        {
            var splitted = stackRow.Chunk(4);

            return splitted.Select(s => s.Where(char.IsLetter).FirstOrDefault()).ToArray();
        }
    }
}
