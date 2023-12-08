using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal static class Day08
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2020", "Day08");

            var accumulator = 0;
            var currentIndex = 0;
            var seen = new HashSet<int>();

            while (!seen.Contains(currentIndex))
            {
                seen.Add(currentIndex);
                var splitted = input[currentIndex].Split(" ");
                var command = splitted[0];
                var value = int.Parse(splitted[1]);

                if (command == "acc")
                {
                    accumulator += value;
                    currentIndex++;
                }

                else if (command == "jmp")
                {
                    currentIndex += value;
                }
                else
                {
                    currentIndex++;
                }
            }

            Console.WriteLine(accumulator);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2020", "Day08");

            var possibleChanges = Enumerable.Range(0, input.Length)
                .Where(i => !input[i].StartsWith("acc"));

            foreach (var indexToChange in possibleChanges)
            {
                var finished = false;
                string[] changedInput = input.Clone() as string[] ?? Array.Empty<string>();

                var current = changedInput[indexToChange];

                if (current.StartsWith("jmp")) changedInput[indexToChange] = current.Replace("jmp", "nop");
                else changedInput[indexToChange] = current.Replace("nop", "jmp");

                var accumulator = 0;
                var currentIndex = 0;
                var seen = new HashSet<int>();

                while (currentIndex < changedInput.Length)
                {
                    if (seen.Contains(currentIndex))
                    {
                        finished = true;
                        break;
                    }

                    seen.Add(currentIndex);
                    var splitted = changedInput[currentIndex].Split(" ");
                    var command = splitted[0];
                    var value = int.Parse(splitted[1]);

                    if (command == "acc")
                    {
                        accumulator += value;
                        currentIndex++;
                    }

                    else if (command == "jmp")
                    {
                        currentIndex += value;
                    }
                    else
                    {
                        currentIndex++;
                    }
                }

                if (!finished)
                {
                    Console.WriteLine(accumulator);
                    break;
                }    
            }
        }
    }
}
