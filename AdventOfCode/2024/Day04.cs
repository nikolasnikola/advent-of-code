namespace AdventOfCode._2024
{
	internal static class Day04
    {
		internal static void Part1()
		{
			var input = Reader.ReadAsCharMatrix("2024", "Day04Test");

			var counter = 0;

			var rows = input.GetLength(0);
			var cols = input.GetLength(1);

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					if (input[i,j] == 'X')
					{

					}
				}
			}

			Console.WriteLine();
		}

		internal static void Part2()
		{
			var input = Reader.ReadAsCharMatrix("2024", "Day04");

			Console.WriteLine();
		}

		private static int GetCountForPosition(char[,] input, int i, int j, int rows, int cols)
		{
			// right
			
		}

		private static char? GetRightLetter(char[,] input, int i, int j, int cols)
		{

		}
	}
}
