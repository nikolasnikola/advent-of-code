using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal static class Day05
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2020", "Day05");

            var currentMax = 0;

            foreach (var seat in input)
            {
                var row = GetSeat(0, 127, seat[..7], 0, 'F');
                var col = GetSeat(0, 7, seat[7..], 0, 'L');

                var id = row * 8 + col;

                Console.WriteLine($"The seat ID for {seat} is {id}");
                currentMax = Math.Max(currentMax, id);
            }

            Console.WriteLine($"The max value is: " + currentMax);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2020", "Day05");

            var currentMax = 0;
            var currentMin = int.MaxValue;
            var sum = 0;

            foreach (var seat in input)
            {
                var row = GetSeat(0, 127, seat[..7], 0, 'F');
                var col = GetSeat(0, 7, seat[7..], 0, 'L');

                var id = row * 8 + col;

                Console.WriteLine($"The seat ID for {seat} is {id}");
                currentMax = Math.Max(currentMax, id);
                currentMin = Math.Min(currentMin, id);
                sum += id;
            }

            var missingSeat = SumRange(currentMin, currentMax) - sum;
            Console.WriteLine($"The missing seat is: " + missingSeat);
        }

        static int SumNatural(int n)
        {
            return (n * (n + 1)) / 2;
        }

        static int SumRange(int l, int r)
        {
            return SumNatural(r) -
                   SumNatural(l - 1);
        }

        static int GetSeat(int from, int to, string characters, int index, char lowerChar)
        {
            if (index == characters.Length-1)
            {
                return characters[index] == lowerChar ? from : to;
            }
            var half = (from + to) / 2;
            if (characters[index] == lowerChar) return GetSeat(from, half, characters, index + 1, lowerChar);
            else return GetSeat(half + 1, to, characters, index + 1, lowerChar);
        }
    }
}
