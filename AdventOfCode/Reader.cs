using System.Reflection;

namespace AdventOfCode
{
    internal static class Reader
    {
        internal static int[] ReadAsIntegerArray(string year, string fileName)
        {
            var lines = GetLines(year, fileName);

            return lines.Select(l => int.Parse(l)).ToArray();
        }

        internal static long[] ReadAsLongArray(string year, string fileName)
        {
            var lines = GetLines(year, fileName);

            return lines.Select(l => long.Parse(l)).ToArray();
        }

        internal static string[] ReadAsStringList(string year, string fileName)
        {
            var lines = GetLines(year, fileName);

            return lines;
        }

        internal static string ReadAsString(string year, string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @$"{year}\Data\{fileName}.txt");
            string text = File.ReadAllText(path);

            return text;
        }

        internal static T?[] ReadAsObjectArray<T>(string year,string fileName) where T : class
        {
            var lines = GetLines(year, fileName);

            return lines.Select(l => Activator.CreateInstance(typeof(T), l.Split(" ")) as T).ToArray();

        }

        internal static char[,] ReadAsCharMatrix(string year, string fileName)
        {
            var lines = GetLines(year, fileName);
            int i = 0;
            char[,] matrix = new char[lines.Count(), lines.First().Length];

            foreach (var line in lines)
            {
                int j = 0;
                foreach (var c in line)
                {
                    matrix[i, j] = c;
                    j++;
                }
                i++;
            }

            return matrix;
        }

        internal static char[,] ReadAsCharMatrixWithStartPoint(string year, string fileName, char startSymbol, out (int,int) point)
        {
            var lines = GetLines(year, fileName);
            int i = 0;
            char[,] matrix = new char[lines.Count(), lines.First().Length];
            point = (0, 0);

            foreach (var line in lines)
            {
                int j = 0;
                foreach (var c in line)
                {
                    if (c == startSymbol) point = (i, j);
                    matrix[i, j] = c;
                    j++;
                }
                i++;
            }

            return matrix;
        }

        internal static int[,] ReadAsIntegerMatrix(string year, string fileName, char? splitter = null)
        {
            var lines = GetLines(year, fileName);

            int i = 0, j = 0;
            int[,] matrix = new int[lines.Count(), lines.First().Length];

            foreach (var line in lines)
            {
                j = 0;
                var trimmed = splitter.HasValue ? line.Trim().Split(splitter.Value) : line.ToCharArray().Select(c => c.ToString());
                foreach (var col in trimmed)
                {
                    matrix[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }

            return matrix;
        }

        internal static Dictionary<(int, int), int> ReadAsIntegerDictionaryMatrix(string year, string fileName, char? splitter = null)
        {
            var lines = GetLines(year, fileName);
            return (
                from row in Enumerable.Range(0, lines.Length)
                from col in Enumerable.Range(0, lines[0].Length)
                let cell = int.Parse(lines[row].Substring(col, 1))
                let pos = (row, col)
                select new KeyValuePair<(int, int), int>(pos, cell)
            ).ToDictionary(x => x.Key, v => v.Value);
        }

        private static string[] GetLines(string year, string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @$"{year}\Data\{fileName}.txt");
            return File.ReadAllLines(path);
        }
    }
}
