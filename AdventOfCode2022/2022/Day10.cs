namespace AdventOfCode._2022
{
    internal static class Day10
    {
        internal static void Part2()
        {
            var commands = Reader.ReadAsStringList("2022", "Day10");

            GenerateCRTScreen(commands);
        }

        internal static void Part1()
        {
            var commands = Reader.ReadAsStringList("2022", "Day10");

            var result = GetSignalStrengthsSum(commands);

            Console.WriteLine(result);
        }

        static void GenerateCRTScreen(string[] commands)
        {
            List<int> signalStrengths = new();

            List<(int, int)> tickCommands = new();

            int lastTickCommand = 0;

            for (int i = 0; i < commands.Length; i++)
            {
                lastTickCommand++;
                if (commands[i].StartsWith("addx"))
                {
                    lastTickCommand++;
                    var value = int.Parse(commands[i][5..]);
                    tickCommands.Add((lastTickCommand, value));
                }
            }
            int x = 1;

            var grouped = Enumerable.Range(0, 240).Chunk(40);

            foreach (var row in grouped)
            {
                for (int i = 0; i < 40; i++)
                {
                    var currentTick = row[i] + 1;

                    var command = tickCommands.FirstOrDefault(c => c.Item1 == currentTick);

                    var pixelValue = Math.Abs(x - i) < 2 ? '#' : '.';

                    Console.Write(pixelValue);

                    if (command != default)
                    {
                        x += command.Item2;
                    }
                }
                Console.Write("\n");
            }

            for (int i = 1; i <= lastTickCommand; i++)
            {
                var command = tickCommands.FirstOrDefault(c => c.Item1 == i);

                if (i % 40 == 0)
                {
                    signalStrengths.Add(x * i);
                }


                if (command != default)
                {
                    x += command.Item2;
                }
            }
        }

        static int GetSignalStrengthsSum(string[] commands)
        {
            List<int> signalStrengths = new();

            List<(int, int)> tickCommands = new();

            int lastTickCommand = 0;

            for (int i = 0; i < commands.Length; i++)
            {
                lastTickCommand++;
                if (commands[i].StartsWith("addx"))
                {
                    lastTickCommand++;
                    var value = int.Parse(commands[i][5..]);
                    tickCommands.Add((lastTickCommand, value));
                }
            }

            int x = 1;
            for (int i = 1; i <= lastTickCommand; i++)
            {
                var command = tickCommands.FirstOrDefault(c => c.Item1 == i);

                if ((i - 20) % 40 == 0)
                {
                    signalStrengths.Add(x * i);
                }

                if (command != default)
                {
                    x += command.Item2;
                }
            }

            return signalStrengths.Sum();
        }
    }
}
