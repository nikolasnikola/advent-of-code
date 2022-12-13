using System.Reflection;

namespace AdventOfCode2022
{
    internal static class Reader
    {
        internal static int[] ReadAsIntegerArray(string fileName)
        {
            var lines = GetLines(fileName);

            return lines.Select(l => int.Parse(l)).ToArray();
        }

        internal static string[] ReadAsStringList(string fileName)
        {
            var lines = GetLines(fileName);

            return lines;
        }

        internal static string ReadAsString(string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @$"Data\{fileName}.txt");
            string text = File.ReadAllText(path);

            return text;
        }

        internal static T?[] ReadAsObjectArray<T>(string fileName) where T : class
        {
            var lines = GetLines(fileName);

            return lines.Select(l => Activator.CreateInstance(typeof(T), l.Split(" ")) as T).ToArray();

        }

        internal static char[,] ReadAsCharMatrix(string fileName)
        {
            var lines = GetLines(fileName);
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

        internal static int[,] ReadAsIntegerMatrix(string fileName, char? splitter = null)
        {
            var lines = GetLines(fileName);

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

        private static string[] GetLines(string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @$"Data\{fileName}.txt");
            return File.ReadAllLines(path);
        }
    }
}
