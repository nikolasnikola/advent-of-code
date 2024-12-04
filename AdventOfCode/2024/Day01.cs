namespace AdventOfCode._2024
{
    internal static class Day01
    {
		internal static void Part1()
		{
			var input = Reader.ReadAsStringList("2024", "Day01");

			var leftArray = input
				.Select(x => int.Parse(x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0]))
				.OrderBy(x => x)
				.ToArray();
			
			var rightArray = input
				.Select(x => int.Parse(x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]))
				.OrderBy(x => x)
				.ToArray();

			var sum = 0;

			for (int i = 0; i < leftArray.Length; i++)
			{
				sum += Math.Abs(leftArray[i] - rightArray[i]);
			}

			Console.WriteLine(sum);
		}

		internal static void Part2()
		{
			var input = Reader.ReadAsStringList("2024", "Day01");

			var leftItems = input
				.Select(x => int.Parse(x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0]))
				.GroupBy(x => x)
				.ToDictionary(g => g.Key, g => g.Count());

			var rightItems = input
				.Select(x => int.Parse(x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]))
				.GroupBy(x => x)
				.ToDictionary(g => g.Key, g => g.Count());

			var similarity = 0;

			foreach (var item in leftItems)
			{
				if (rightItems.TryGetValue(item.Key, out var repeats))
				{
					similarity += item.Key * repeats * item.Value;
				}
			}

			Console.WriteLine(similarity);
		}
	}
}
