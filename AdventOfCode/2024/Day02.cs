namespace AdventOfCode._2024
{
	internal static class Day02
	{
		internal static void Part1()
		{
			var input = Reader.ReadAsStringList("2024", "Day02");

			var reports = input.Select(x => x.Split(' ').Select(n => int.Parse(n)).ToArray());

			var safeCount = 0;

			foreach (var report in reports)
			{
				if (IsSafe(report)) safeCount++;
			}

			Console.WriteLine(safeCount);
		}

		internal static void Part2()
		{
			var input = Reader.ReadAsStringList("2024", "Day02");

			var reports = input.Select(x => x.Split(' ').Select(n => int.Parse(n)).ToArray());

			var safeCount = 0;

			foreach (var report in reports)
			{
				if (IsSafe(report))
				{
					safeCount++;
				}
				else
				{
					for (int i = 0; i < report.Length; i++)
					{
						var newArray = report.RemoveAt(i);

						if (IsSafe(newArray))
						{
							safeCount++;
							break;
						}
					}
				}
			}

			Console.WriteLine(safeCount);
		}

		private static bool IsSafe(int[] report)
		{
			if (report.Length < 2) return false;

			bool increasing = report[1] > report[0];
			bool decreasing = report[1] < report[0];

			for (int i = 1; i < report.Length; i++)
			{
				var diff = Math.Abs(report[i] - report[i - 1]);

				if (diff < 1 || diff > 3 ||
					(increasing && report[i] <= report[i - 1]) ||
					(decreasing && report[i] >= report[i - 1]))
				{
					return false;
				}
			}

			return increasing || decreasing;
		}

		private static int[] RemoveAt(this int[] source, int index)
		{
			int[] dest = new int[source.Length - 1];
			if (index > 0)
				Array.Copy(source, 0, dest, 0, index);

			if (index < source.Length - 1)
				Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

			return dest;
		}
	}
}
