using AdventOfCode2022.Dto.Day22;

namespace AdventOfCode2022._2022
{
    internal static class Day22
    {
        internal static void Part2()
        {
            var input = Reader.ReadAsString("Day22").Split("\r\n\r\n");

            var graph = GetGraph(input[0]);

            var map = new Map(graph);
            var instructions = GetInstructions(input[1]);

            ProceedInstructionsWithSides(map, instructions);

            PrintMap(map, false);
            var result = map.GetScore();
            Console.WriteLine("**************");
            Console.WriteLine($"Current row: {map.CurrentRow + 1}; Current column: {map.CurrentColumn + 1}; Current side: {map.CurrentSide}; Current direction: {map.CurrentDirection}") ;
            Console.WriteLine($"Position score: {result}");
            Console.WriteLine("**************");
        }

        internal static void Part1()
        {
            var input = Reader.ReadAsString("Day22").Split("\r\n\r\n");

            var graph = GetGraph(input[0]);

            var map = new Map(graph);
            var instructions = GetInstructions(input[1]);
            
            ProceedInstructions(map, instructions);

            PrintMap(map, false);
            var result = map.GetScore();
            Console.WriteLine("**************");
            Console.WriteLine($"Position score: {result}");
            Console.WriteLine("**************");
        }

        private static void ProceedInstructionsWithSides(Map map, List<PathDescription> instructions)
        {
            foreach (var instruction in instructions) //77
            {
                if (instruction.IsDirectionChange)
                {
                    map.ChangeDirection(instruction.Direction!.Value);
                }
                else
                {
                    foreach (var _ in Enumerable.Range(0, instruction.Steps!.Value))
                    {
                        if (!map.GoForwardWithSidesIfPossible())
                        {
                            //PrintMap(map, false);
                            break;
                        }
                        //PrintMap(map, false);
                    }
                }
            }
        }

        private static void ProceedInstructions(Map map, List<PathDescription> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.IsDirectionChange)
                {
                    map.ChangeDirection(instruction.Direction!.Value);
                }
                else
                {
                    foreach (var _ in Enumerable.Range(0, instruction.Steps!.Value))
                    {
                        if (!map.GoForwardIfPossible())
                        {
                            //PrintMap(map, false);
                            break;
                        }
                        //PrintMap(map, false);
                    }
                }
            }
        }

        static char[,] GetGraph(string graphStr)
        {
            var rows = graphStr.Split("\r\n");

            var totalRows = rows.Length;
            var totalColumns = rows.Max(r => r.Length);

            var graph = new char[totalRows, totalColumns];

            for (int i = 0; i < rows.Length; i++)
            {
                var rowData = rows[i].Replace(' ', '\0');
                for (int j = 0; j < rowData.Length; j++)
                {
                    graph[i, j] = rowData[j];
                }
            }

            return graph;
        }

        static List<PathDescription> GetInstructions(string input)
        {
            List<PathDescription> instructions = new();
            string currentSteps = string.Empty;

            foreach (var c in input)
            {
                if (c == 'R' || c == 'L')
                {
                    if (!string.IsNullOrWhiteSpace(currentSteps))
                    {
                        instructions.Add(new PathDescription(int.Parse(currentSteps)));
                        currentSteps = string.Empty;
                    }

                    instructions.Add(new PathDescription(c));
                }
                else
                {
                    currentSteps += c;
                }
            }

            if (!string.IsNullOrWhiteSpace(currentSteps))
            {
                instructions.Add(new PathDescription(int.Parse(currentSteps)));
            }

            return instructions;
        }

        static void PrintMap(Map map, bool spaceBetweenRows)
        {
            Console.WriteLine("**************");
            Console.WriteLine("Map");
            Console.WriteLine("**************");

            var graph = map.Graph;
            var betweenRows = spaceBetweenRows ? " " : string.Empty;
            var totalRows = graph.GetLength(0);

            for (int i = 0; i < totalRows; i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    Console.ResetColor();

                    if (map.CurrentRow == i && map.CurrentColumn == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('o' + betweenRows);
                        continue;
                    }
                    else if (graph[i, j] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    if (graph[i, j] == '\0')
                    {
                        Console.Write(" " + betweenRows);
                    }
                    else
                    {
                        Console.Write(graph[i, j] + betweenRows);
                    }
                    
                }
                Console.ResetColor();
                Console.WriteLine();
            }


            //Thread.Sleep(10);
        }
    }
}
