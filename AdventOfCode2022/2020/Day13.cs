using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2020
{
    internal static class Day13
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2020", "Day13");

            var estimate = int.Parse(input.First());
            var buses = input.Last().Split(',').Where(x => x != "x").Select(x => int.Parse(x));

            var currentBestTime = int.MaxValue;
            var currentBestBusId = 0;

            foreach (var b in buses)
            {
                var time = b;
                while (time<estimate)
                {
                    time += b;
                }

                if (time < currentBestTime)
                {
                    currentBestTime = time;
                    currentBestBusId = b;
                }
            }

            var result = currentBestBusId * (currentBestTime - estimate);

            Console.WriteLine(result);
        }

        [Obsolete("Use DP version instead")]
        internal static void Part2Old()
        {
            var input = Reader.ReadAsStringList("2020", "Day13");

            var buses = input
                .Last()
                .Split(',')
                .Select((v, i) => new KeyValuePair<int, string>(i, v))
                .Where(x => x.Value != "x")
                .Select(x => new KeyValuePair<int, int>(x.Key, int.Parse(x.Value)))
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            bool finished = false;
            var refBus = buses.First();
            long timestamp = refBus.Value;

            var second = buses.Skip(1).Take(1).Single();
            var third = buses.Skip(2).Take(1).Single();
            var fourth = buses.Skip(3).Take(1).Single();

            var differenceFound = false;
            var difference2Found = false;
            var difference3Found = false;
            int difference = 1;

            while (!finished)
            {
                var currentValid = true;

                foreach (var b in buses.Skip(1))
                {
                    if (!differenceFound && b.Key == second.Key && (timestamp + b.Key - refBus.Key) % b.Value == 0)
                    {
                        differenceFound = true;
                        difference = second.Value;
                    }

                    if (differenceFound && !difference2Found && b.Key == third.Key && (timestamp + b.Key - refBus.Key) % b.Value == 0)
                    {
                        difference2Found = true;
                        difference *= third.Value;
                    }

                    if (difference2Found && !difference3Found && b.Key == fourth.Key && (timestamp + b.Key - refBus.Key) % b.Value == 0)
                    {
                        difference3Found = true;
                        difference *= fourth.Value;
                    }

                    if (!currentValid) break;
                    if ((timestamp + b.Key - refBus.Key) % b.Value !=0 )
                    {
                        currentValid = false;
                    }
                }

                if (currentValid) finished = true;
                else timestamp += (refBus.Value * difference);
            }

            Console.WriteLine(timestamp-refBus.Key);
        }
        
        // DP
        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2020", "Day13");

            var buses = input
                .Last()
                .Split(',')
                .Select((v, i) => new KeyValuePair<int, string>(i, v))
                .Where(x => x.Value != "x")
                .Select(x => new KeyValuePair<int, int>(x.Key, int.Parse(x.Value)))
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            
            var refBus = buses.First();
            long timestamp = refBus.Value;

            long dp = 1;


            foreach (var b in buses.Skip(1))
            {
                bool finished = false;
                while (!finished)
                {
                    timestamp += (refBus.Value * dp);
                    if ((timestamp + b.Key - refBus.Key) % b.Value == 0)
                    {
                        finished = true;
                        dp *= b.Value;
                    }
                }
            }

            Console.WriteLine(timestamp - refBus.Key);
        }
    }
}
