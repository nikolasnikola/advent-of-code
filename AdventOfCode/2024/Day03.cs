using System.Text.RegularExpressions;

namespace AdventOfCode._2024
{
	internal static class Day03
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2024", "Day03");

			Regex regex = new Regex(@"mul\((\d+),(\d+)\)");

            var sum = 0;

            foreach (Match match in regex.Matches(input))
            {
                if (match.Success)
                {
					int x = int.Parse(match.Groups[1].Value);
					int y = int.Parse(match.Groups[2].Value);

                    sum += x * y;
				}
            }

			Console.WriteLine(sum);
        }

        internal static void Part2()
        {
			var input = Reader.ReadAsString("2024", "Day03");

			Regex regex = new Regex(@"mul\((\d+),(\d+)\)");
			var sum = 0;

			var splittedDo = input.Split("do()", StringSplitOptions.RemoveEmptyEntries);

			foreach (var splitDo in splittedDo)
			{
				var splittedDont = splitDo.Split("don't()", StringSplitOptions.RemoveEmptyEntries);

				foreach (Match match in regex.Matches(splittedDont[0]))
				{
					if (match.Success)
					{
						int x = int.Parse(match.Groups[1].Value);
						int y = int.Parse(match.Groups[2].Value);

						sum += x * y;
					}
				}

			}

			Console.WriteLine(sum);
		} 
    }
}
