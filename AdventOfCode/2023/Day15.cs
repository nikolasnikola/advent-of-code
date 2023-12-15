using NUnit.Framework;

namespace AdventOfCode._2023
{
    internal static class Day15
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2023", "Day15");

            var result = input.Split(',').Sum(i => GetHashCode(i));

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsString("2023", "Day15");

            var commands = input.Split(',');

            var boxes = Enumerable.Range(0, 256).Select(n => new Box(n)).ToList();

            foreach (var command in commands)
            {
                if (command.Contains('-'))
                {
                    var cmdParts = command.Split('-');
                    var label = cmdParts[0];
                    var box = boxes[GetHashCode(label)];

                    var lensToRemove = box.Lenses.SingleOrDefault(x => x.Label == label);
                    if (lensToRemove != null) box.Lenses.Remove(lensToRemove);
                }

                else
                {
                    var cmdParts = command.Split('=');
                    var label = cmdParts[0];
                    var focal = int.Parse(cmdParts[1]);
                    var box = boxes[GetHashCode(label)];

                    var existingLens = box.Lenses.SingleOrDefault(x => x.Label == label);

                    if (existingLens != null) existingLens.Focal = focal;
                    else box.Lenses.Add(new Lens() { Label = label, Focal = focal });
                }
            }

            var result = CalculateResult(boxes);

            Console.WriteLine(result);
        }

        internal static int GetHashCode(string input) => input.Aggregate(0, (currentValue, c) => (currentValue + c) * 17 % 256);

        private static int CalculateResult(List<Box> boxes)
        {
            var result = 0;

            foreach (var box in boxes)
            {
                for (int i = 1; i<= box.Lenses.Count; i++)
                {
                    result += (box.BoxNumber + 1) * i * box.Lenses[i-1].Focal;
                }
            }

            return result;
        }
    }

    internal class Box
    {
        public Box(int boxNumber) { BoxNumber = boxNumber; Lenses = new List<Lens>(); }
        public int BoxNumber { get; set; }
        public IList<Lens> Lenses { get; set; }
    }

    internal class Lens
    {
        public string Label { get; set; }
        public int Focal { get; set; }
    }

    public class Day15Tests
    {
        [Test]
        public void HashTest()
        {
            var input = "HASH";
            var result = Day15.GetHashCode(input);

            Assert.AreEqual(52, result);
        }
    }
}