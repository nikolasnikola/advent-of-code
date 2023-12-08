using System.Data.Common;

namespace AdventOfCode._2023
{
    internal static class Day05
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2023", "Day05");

            var data = input.Split("\r\n\r\n");
            var plants = data[0]
                .Split(':')[1]
                .Trim()
                .Split(' ')
                .Select(x => new SeedPlant() { Seed = long.Parse(x)})
                .ToList();

            var seedToSoil = GetMaps(data[1]);
            var soilToFertilizer = GetMaps(data[2]);
            var fertilizerToWater = GetMaps(data[3]);
            var waterToLight = GetMaps(data[4]);
            var lightToTemperature = GetMaps(data[5]);
            var temperatureToHumidity = GetMaps(data[6]);
            var humidityToLocation = GetMaps(data[7]);

            foreach (var plant in plants)
            {
                plant.Soil = GetNextItem(plant.Seed, seedToSoil);
                plant.Fertilizer = GetNextItem(plant.Soil, soilToFertilizer);
                plant.Water = GetNextItem(plant.Fertilizer, fertilizerToWater);
                plant.Light = GetNextItem(plant.Water, waterToLight);
                plant.Temperature = GetNextItem(plant.Light, lightToTemperature);
                plant.Humidity = GetNextItem(plant.Temperature, temperatureToHumidity);
                plant.Location = GetNextItem(plant.Humidity, humidityToLocation);
            }

            var result = plants.Min(p => p.Location);
            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var skipStep = 50000; // for optimization
            var input = Reader.ReadAsString("2023", "Day05");

            var data = input.Split("\r\n\r\n");
            var plantsData = data[0]
                .Split(':')[1]
                .Trim()
                .Split(' ');

            var seedToSoil = GetMaps(data[1]);
            var soilToFertilizer = GetMaps(data[2]);
            var fertilizerToWater = GetMaps(data[3]);
            var waterToLight = GetMaps(data[4]);
            var lightToTemperature = GetMaps(data[5]);
            var temperatureToHumidity = GetMaps(data[6]);
            var humidityToLocation = GetMaps(data[7]);

            var currentMinLocation = long.MaxValue;
            SeedPlant minPlant = null!;

            // get best value with skipping ever {slipStep} items
            for (int i = 0; i < plantsData.Length; i += 2)
            {
                var first = long.Parse(plantsData[i]);
                var last = long.Parse(plantsData[i]) + long.Parse(plantsData[i + 1]);

                for (long j = first;  j < last; j+= skipStep)
                {
                    var plant = new SeedPlant { Seed = j, Range = (first, last - 1) };
                    FillPlant(seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation, plant);
                    
                    if (plant.Location < currentMinLocation)
                    {
                        currentMinLocation = plant.Location;
                        minPlant = plant;
                    }
                }
            }

            // when best seed is found with skipped items, final result must be in range from {bestSeed-skipStep} to {bestSeed+skipStep}.
            for (long i = Math.Max(minPlant.Range.Item1, minPlant.Seed - skipStep); i < Math.Min(minPlant.Range.Item2, minPlant.Seed + skipStep); i++)
            {
                var plant = new SeedPlant() { Seed = i };
                FillPlant(seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation, plant);

                currentMinLocation = Math.Min(currentMinLocation, plant.Location);
            }

            Console.WriteLine(currentMinLocation);
        }

        private static void FillPlant(
            IEnumerable<(long, long, long)> seedToSoil,
            IEnumerable<(long, long, long)> soilToFertilizer,
            IEnumerable<(long, long, long)> fertilizerToWater,
            IEnumerable<(long, long, long)> waterToLight,
            IEnumerable<(long, long, long)> lightToTemperature,
            IEnumerable<(long, long, long)> temperatureToHumidity,
            IEnumerable<(long, long, long)> humidityToLocation,
            SeedPlant plant)
        {
            plant.Soil = GetNextItem(plant.Seed, seedToSoil);
            plant.Fertilizer = GetNextItem(plant.Soil, soilToFertilizer);
            plant.Water = GetNextItem(plant.Fertilizer, fertilizerToWater);
            plant.Light = GetNextItem(plant.Water, waterToLight);
            plant.Temperature = GetNextItem(plant.Light, lightToTemperature);
            plant.Humidity = GetNextItem(plant.Temperature, temperatureToHumidity);
            plant.Location = GetNextItem(plant.Humidity, humidityToLocation);
        }

        private static IEnumerable<(long, long, long)> GetMaps(string mapData)
        {
            var splitted = mapData.Split("\r\n");
            foreach (var item in splitted.Skip(1))
            {
                var items = item.Split(" ");
                yield return (long.Parse(items[0]), long.Parse(items[1]), long.Parse(items[2]));
            }
        }

        private static long GetNextItem(long currentItem, IEnumerable<(long, long, long)> maps)
        {
            foreach(var map in maps)
            {
                if (currentItem >= map.Item2 && currentItem < map.Item2 + map.Item3) return currentItem + map.Item1 - map.Item2;
            }
            return currentItem;
        }
    }

    internal class SeedPlant
    {
        public (long, long) Range { get; set; }
        public long Seed { get; set; }
        public long Soil { get; set; }
        public long Fertilizer { get; set; }
        public long Water { get; set; }
        public long Light { get; set; }
        public long Temperature { get; set; }
        public long Humidity { get; set; }
        public long Location { get; set; }
    }
}
