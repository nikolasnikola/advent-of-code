using System.Text.RegularExpressions;
using AdventOfCode.Models.Day19;

namespace AdventOfCode._2022
{
    public class Day19State : IEquatable<Day19State>
    {
        public Day19State(int ore, int clay, int obsidian, int geode, int oreRobot, int clayRobot, int obsidianRobot, int geodeRobot)
        {
            Ore = ore;
            Clay = clay;
            Obsidian = obsidian;
            Geode = geode;
            OreRobot = oreRobot;
            ClayRobot = clayRobot;
            ObsidianRobot = obsidianRobot;
            GeodeRobot = geodeRobot;
        }

        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geode { get; set; }

        public int OreRobot { get; set; }
        public int ClayRobot { get; set; }
        public int ObsidianRobot { get; set; }
        public int GeodeRobot { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ore, Clay, Obsidian, Geode, OreRobot, ClayRobot, ObsidianRobot, GeodeRobot);
        }

        public Day19State GenerateNewState()
        {
            return new Day19State(Ore, Clay, Obsidian, Geode, OreRobot, ClayRobot, ObsidianRobot, GeodeRobot);
        }

        public void BuildOreRobot(Blueprint blueprint)
        {
            Ore -= blueprint.OreRobotOreCost;
            OreRobot++;
        }

        public void BuildClayRobot(Blueprint blueprint)
        {
            Ore -= blueprint.ClayRobotOreCost;
            ClayRobot++;
        }

        public void BuildObsidianBot(Blueprint blueprint)
        {
            Ore -= blueprint.ObsidianRobotOreCost;
            Clay -= blueprint.ObsidianRobotClayCost;
            ObsidianRobot++;
        }

        public void BuildGeodeBot(Blueprint blueprint)
        {
            Ore -= blueprint.GeodeRobotOreCost;
            Obsidian -= blueprint.GeodeRobotObisidianCost;
            GeodeRobot++;
        }

        public bool Equals(Day19State? other)
        {
            return other != null && Ore == other.Ore && Clay == other.Clay && Obsidian == other.Obsidian && Geode == other.Geode
                && OreRobot == other.OreRobot && ClayRobot == other.ClayRobot && ObsidianRobot == other.ObsidianRobot && GeodeRobot == other.GeodeRobot;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Day19State);
        }

        public static bool operator ==(Day19State? left, Day19State? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Day19State? left, Day19State? right)
        {
            return !Equals(left, right);
        }
    }

    internal static class Day19
    {
        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2022", "Day19");

            var blueprints = GetBlueprints(input).Take(3);

            int result = 1;

            Parallel.ForEach(blueprints, blueprint =>
            {
                var bestResult = ProcessBlueprint(blueprint, 32);
                result *= bestResult;
                Console.WriteLine($"Blueprint {blueprint.Id} most geodes: {bestResult}");
            });

            Console.WriteLine("\n\n**************");
            Console.WriteLine($"Total result: {result}");
            Console.WriteLine("**************");
        }

        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2022", "Day19");

            var blueprints = GetBlueprints(input);

            int quality = 0;

            Parallel.ForEach(blueprints, blueprint =>
            {
                var bestResult = ProcessBlueprint(blueprint);
                quality += blueprint.Id * bestResult;
                Console.WriteLine($"Blueprint {blueprint.Id} most geodes: {bestResult}");
            });

            Console.WriteLine("\n\n**************");
            Console.WriteLine($"Best quality: {quality}");
            Console.WriteLine("**************");
        }

        static int ProcessBlueprint(Blueprint blueprint, int minutes = 24)
        {
            HashSet<Day19State> states = new()
            {
                new Day19State(0, 0, 0, 0, 1, 0, 0, 0)
            };

            foreach (var i in Enumerable.Range(1, minutes))
            {
                //Console.Write($"\nCalculating minute {i}... 00%");

                states = ProcessMinute(states, new HashSet<Day19State>(), blueprint);

                states = states.Distinct().Where(s => blueprint.CurrentGeodeBest - s.Geode < 2 && blueprint.CurrentGeodeRobotBest - s.GeodeRobot < 2).ToHashSet();

                var currentBest = states.MaxBy(s => s.Geode);

                //Console.Write($" => {states.Count} states total");
            }

            var maxState = states.MaxBy(s => s.Geode);
            var bestState = maxState!.Geode;

            return bestState;
        }

        static HashSet<Day19State> ProcessMinute(HashSet<Day19State> states, HashSet<Day19State> nextStates, Blueprint blueprint)
        {
            foreach (var (state, i) in states.Select((s, i) => (s, i)))
            {
                //Console.CursorLeft -= 3;
                //Console.CursorVisible = false;
                //Console.Write($"{(100 * (i + 1) / states.Count).ToString("D2")}%");

                if (state.Ore >= blueprint.GeodeRobotOreCost && state.Obsidian >= blueprint.GeodeRobotObisidianCost)
                {
                    var newState = state.GenerateNewState();

                    newState.Ore += state.OreRobot;
                    newState.Clay += state.ClayRobot;
                    newState.Obsidian += state.ObsidianRobot;
                    newState.Geode += state.GeodeRobot;

                    newState.BuildGeodeBot(blueprint);

                    var newStateRobots = newState.ClayRobot + newState.ObsidianRobot + newState.GeodeRobot;
                    if (blueprint.CurrentBest - newStateRobots < 10) nextStates.Add(newState);       
                }

                if (state.Ore >= blueprint.ObsidianRobotOreCost && state.Clay >= blueprint.ObsidianRobotClayCost && state.ObsidianRobot < blueprint.GeodeRobotObisidianCost)
                {
                    var newState = state.GenerateNewState();

                    newState.Ore += state.OreRobot;
                    newState.Clay += state.ClayRobot;
                    newState.Obsidian += state.ObsidianRobot;
                    newState.Geode += state.GeodeRobot;

                    newState.BuildObsidianBot(blueprint);

                    var newStateRobots = newState.ClayRobot + newState.ObsidianRobot + newState.GeodeRobot;
                    if (blueprint.CurrentBest - newStateRobots < 10) nextStates.Add(newState);
                }

                if (state.Ore >= blueprint.ClayRobotOreCost && state.ClayRobot < blueprint.ObsidianRobotClayCost)
                {
                    var newState = state.GenerateNewState();

                    newState.Ore += state.OreRobot;
                    newState.Clay += state.ClayRobot;
                    newState.Obsidian += state.ObsidianRobot;
                    newState.Geode += state.GeodeRobot;

                    newState.BuildClayRobot(blueprint);

                    var newStateRobots = newState.ClayRobot + newState.ObsidianRobot + newState.GeodeRobot;
                    if (blueprint.CurrentBest - newStateRobots < 10) nextStates.Add(newState);
                }

                if (state.Ore >= blueprint.OreRobotOreCost && state.OreRobot < blueprint.GetMaxOres())
                {
                    var newState = state.GenerateNewState();

                    newState.Ore += state.OreRobot;
                    newState.Clay += state.ClayRobot;
                    newState.Obsidian += state.ObsidianRobot;
                    newState.Geode += state.GeodeRobot;

                    newState.BuildOreRobot(blueprint);

                    var newStateRobots = newState.ClayRobot + newState.ObsidianRobot + newState.GeodeRobot;
                    if (blueprint.CurrentBest - newStateRobots < 10)
                        nextStates.Add(newState);
                }

                var currentStateRobots = state.ClayRobot + state.ObsidianRobot + state.GeodeRobot;

                if (blueprint.CurrentBest - currentStateRobots < 10)
                {
                    var newState = state.GenerateNewState();
                    newState.Ore += state.OreRobot;
                    newState.Clay += state.ClayRobot;
                    newState.Obsidian += state.ObsidianRobot;
                    newState.Geode += state.GeodeRobot;

                    nextStates.Add(newState);
                }

                blueprint.CurrentBest = Math.Max(blueprint.CurrentBest, currentStateRobots);
                blueprint.CurrentGeodeBest = Math.Max(blueprint.CurrentGeodeBest, state.Geode);
                blueprint.CurrentGeodeRobotBest = Math.Max(blueprint.CurrentGeodeRobotBest, state.GeodeRobot);
            }

            return nextStates;
        }

        static List<Blueprint> GetBlueprints(string[] blueprints)
        {
            return blueprints.Select(bp =>
                            Regex.Match(bp, @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian."))
                .Select(match =>
                new Blueprint()
                {
                    Id = int.Parse(match.Groups[1].Value),
                    OreRobotOreCost = int.Parse(match.Groups[2].Value),
                    ClayRobotOreCost = int.Parse(match.Groups[3].Value),
                    ObsidianRobotOreCost = int.Parse(match.Groups[4].Value),
                    ObsidianRobotClayCost = int.Parse(match.Groups[5].Value),
                    GeodeRobotOreCost = int.Parse(match.Groups[6].Value),
                    GeodeRobotObisidianCost = int.Parse(match.Groups[7].Value)
                })
                .ToList();
        }
    }
}
