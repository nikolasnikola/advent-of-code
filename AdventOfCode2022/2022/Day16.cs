using System.Linq;
using AdventOfCode2022.Dto.Day16;

namespace AdventOfCode2022._2022
{
    public struct State
    {
        public State(List<(string, long)> opened, Valve valve, long totalRate)
        {
            Opened = opened;
            Valve = valve;
            TotalRate = totalRate;
        }

        public List<(string, long)> Opened;
        public Valve Valve;
        public long TotalRate;
    }

    public class OpenedValve
    {
        public OpenedValve(string name, int rate, int minute, string openedBy)
        {
            Name = name;
            Rate = rate;
            Minute = minute;
            OpenedBy = openedBy;
        }

        public string Name { get; set; }
        public int Rate { get; set; }
        public int Minute { get; set; }
        public string OpenedBy { get; set; }
    }

    public class State2
    {
        public State2(List<OpenedValve> opened, Valve valveI, Valve valveE, long totalRate)
        {
            Opened = opened;
            ValveI = valveI;
            ValveE = valveE;
            TotalRate = totalRate;
        }

        public List<OpenedValve> Opened;
        public Valve ValveI;
        public Valve ValveE;
        public long TotalRate;
    }

    internal static class Day16
    {
        private static List<Valve> openableValves = new();
        private static Valve startValve;

        internal static void Part2()
        {
            var valvesData = Reader.ReadAsStringList("Day16");

            var valves = GetValves(valvesData).ToList();
            startValve = valves.Single(v => v.Name == "AA");
            openableValves = valves.Where(v => v.Rate > 0).ToList();

            var checkPointsOptimization = new Dictionary<int, long>
            {
                {5, 20 },
                {10, 200 },
                {15, 700 },
                {20, 1100 },
                {25, 1500 }
            };

            HashSet<State2> states = new()
            {
                new State2(new List<OpenedValve>(), startValve, startValve, 0)
            };
            foreach (var (minute, updatedStates) in Enumerable.Range(1, 26).Select(m => (m, new HashSet<State2>())))
            {
                Console.Write($"\nCalculating minute {minute}... 00%");
                states = ProcessMinute2(valves, states, updatedStates, minute);
                Console.Write($" => {states.Count} states total");

                if (minute == 9)
                {
                    states = states.OrderByDescending(x => x.TotalRate).Take(6000000).ToHashSet();
                }

                if (minute == 11)
                {
                    var bestValves = openableValves.OrderByDescending(v => v.Rate).Take(2);

                    states = states
                        .Where(s => s.Opened.Select(o => o.Rate).Contains(bestValves.First().Rate) && s.Opened.Select(o => o.Rate).Contains(bestValves.Last().Rate))
                        .ToHashSet();
                }

                if (minute >= 12)
                {
                    var average = states.Select(s => s.TotalRate).Average();
                    states = states.Where(s => s.TotalRate >= average).ToHashSet();
                }

                //if (minute >= 16)
                //{
                //    var secondBestFlow = states.Select(s => s.TotalRate).Distinct().OrderByDescending(x => x).Take(2).Last();
                //    states = states.Where(s => s.TotalRate >= secondBestFlow).ToHashSet();
                //}

                if (checkPointsOptimization.Keys.Contains(minute))
                {
                    states = states.Where(s => s.TotalRate >= checkPointsOptimization[minute]).ToHashSet();
                }
            }

            var result = states.OrderByDescending(s => s.TotalRate).First();

            Console.WriteLine("\n********");
            Console.WriteLine($"Best flow: {result.TotalRate}");
            Console.WriteLine($"\nOpened valves");
            foreach (var opened in result.Opened)
            {
                Console.WriteLine($"Valve: {opened.Name}, Rate {opened.Rate}, Minute {opened.Minute}, opened by {opened.OpenedBy}");
            }
            Console.WriteLine("********");
        }

        internal static void Part1()
        {
            var valvesData = Reader.ReadAsStringList("Day16Test");

            var valves = GetValves(valvesData).ToList();
            HashSet<State> states = new()
            {
                new State(new List<(string, long)>(), valves.Single(v => v.Name == "AA"), 0)
            };
            foreach (var (minute, updatedStates) in Enumerable.Range(0, 30).Select(m => (m, new HashSet<State>())))
            {
                Console.WriteLine($"Calculating minute {minute}...");
                states = ProcessMinute(valves, states, updatedStates);
            }

            var result = states.Select(s => s.TotalRate).Max();

            Console.WriteLine("\n********");
            Console.WriteLine($"Best flow: {result}");
            Console.WriteLine("********");
        }

        private static HashSet<State2> ProcessMinute2(List<Valve> valves, HashSet<State2> states, HashSet<State2> updatedStates, int minute)
        {
            var currentMax = states.Max(s => s.Opened.Select(o => o.Rate).Sum());
            foreach (var (s, rate, i) in states.Select((s,i) => (s, s.Opened.Select(o => o.Rate).Sum() + s.TotalRate, i)))
            {
                Console.CursorLeft -= 3;
                Console.CursorVisible = false;
                Console.Write($"{(100*(i+1)/states.Count).ToString("D2")}%");
                SetNewStates2(valves, updatedStates, s, rate, minute, currentMax);
            }


            return updatedStates;
        }

        private static HashSet<State> ProcessMinute(List<Valve> valves, HashSet<State> states, HashSet<State> updatedStates)
        {
            foreach (var (s, rate) in states.Select(s => (s, s.Opened.Select(o => o.Item2).Sum() + s.TotalRate)))
            {
                SetNewStates(valves, updatedStates, s, rate);
            }

            return updatedStates;
        }

        private static void SetNewStates2(List<Valve> valves, HashSet<State2> updatedStates, State2 state, long rate, int minute, long currentMaxRate)
        {
            if (openableValves.Count == state.Opened.Count)
            {
                var newState = new State2(state.Opened, startValve, startValve, rate);

                if (CanBeatCurrentMax(newState, currentMaxRate, minute)) updatedStates.Add(newState);
            }

            var currentStatesCount = updatedStates.Count;

            var IHaveSomethingToOpenHere = state.ValveI.Rate > 0 && !state.Opened.Select(o => o.Name).Contains(state.ValveI.Name);
            

            if (IHaveSomethingToOpenHere)
            {
                var newOpenI = new List<OpenedValve>();
                newOpenI.AddRange(state.Opened);
                newOpenI.Add(new OpenedValve(state.ValveI.Name, state.ValveI.Rate, minute, "ME"));

                foreach (var connectionE in state.ValveE.Connections)
                {
                    var nsI = new State2(newOpenI, state.ValveI, valves.Single(v => v.Name == connectionE), rate);

                    if (CanBeatCurrentMax(nsI, currentMaxRate, minute)) updatedStates.Add(nsI);
                }
            }

            var elephantHasSomethingToOpenHere = state.ValveE.Rate > 0 && !state.Opened.Select(o => o.Name).Contains(state.ValveE.Name);

            if (elephantHasSomethingToOpenHere)
            {
                var newOpenE = new List<OpenedValve>();
                newOpenE.AddRange(state.Opened);
                newOpenE.Add(new OpenedValve(state.ValveE.Name, state.ValveE.Rate, minute, "ELEPHANT"));

                foreach (var connectionI in state.ValveI.Connections)
                {
                    var nsE = new State2(newOpenE, valves.Single(v => v.Name == connectionI), state.ValveE, rate);
                    if (CanBeatCurrentMax(nsE, currentMaxRate, minute)) updatedStates.Add(nsE);
                }
            }

            if (IHaveSomethingToOpenHere && elephantHasSomethingToOpenHere && state.ValveE.Name != state.ValveI.Name)
            {
                var newOpen = new List<OpenedValve>();
                newOpen.AddRange(state.Opened);
                newOpen.Add(new OpenedValve(state.ValveI.Name, state.ValveI.Rate, minute, "ME"));
                newOpen.Add(new OpenedValve(state.ValveE.Name, state.ValveE.Rate, minute, "ELEPHANT"));

                var nsB = new State2(newOpen, state.ValveI, state.ValveE, rate);
                if (CanBeatCurrentMax(nsB, currentMaxRate, minute)) updatedStates.Add(nsB);
            }

            if (updatedStates.Count == currentStatesCount)
            {
                foreach (var cI in state.ValveI.Connections)
                {
                    foreach (var cE in state.ValveE.Connections)
                    {
                        var nsW = new State2(state.Opened, valves.Single(v => v.Name == cI), valves.Single(v => v.Name == cE), rate);
                        if (CanBeatCurrentMax(nsW, currentMaxRate, minute)) updatedStates.Add(nsW);
                    }
                }
            }
        }

        static bool CanBeatCurrentMax(State2 state, long currentMax, int minute)
        {
            var minutesLeft = 26 - minute;

            var currentMaxWorstCase = (minutesLeft+1) * currentMax;

            var notOpened = openableValves.Select(v => v.Rate).Except(state.Opened.Select(o => o.Rate)).OrderByDescending(x => x).ToList();

            long result = state.TotalRate;

            var newOpened = new List<int>();

            for (int i = 0; i< minutesLeft; i++)
            {
                var newRate = result + state.Opened.Select(o => o.Rate).Sum() + newOpened.Sum();

                if(notOpened.Any())
                {
                    var bestNext = notOpened[0];
                    newOpened.Add(bestNext);
                    notOpened.Remove(bestNext);

                    if (notOpened.Any())
                    {
                        var bestNext2 = notOpened[0];
                        newOpened.Add(bestNext2);
                        notOpened.Remove(bestNext2);
                    }
                }

                result = newRate;
            }

            return result >= currentMaxWorstCase;
        }

        private static void SetNewStates(List<Valve> valves, HashSet<State> updatedStates, State state, long rate)
        {
            if (state.Valve.Rate > 0 && !state.Opened.Select(o => o.Item1).Contains(state.Valve.Name))
            {
                var newOpen = new List<(string, long)>();
                newOpen.AddRange(state.Opened);

                newOpen.Add((state.Valve.Name, state.Valve.Rate));
                updatedStates.Add(new State(newOpen, state.Valve, rate));
            }

            foreach (var c in state.Valve.Connections)
            {
                updatedStates.Add(new State(state.Opened, valves.Single(v => v.Name == c), rate));
            }
        }

        static IEnumerable<Valve> GetValves(string[] valvesData)
        {
            for (int i = 0; i < valvesData.Length; i++)
            {
                var vd = valvesData[i];

                var vdInfo = vd.Split("; ");
                var valveName = vdInfo[0].Substring(6, 2);
                var valveRate = int.Parse(vdInfo[0][23..]);

                var connectionsData = vdInfo[1].Split(", ");
                var connections = connectionsData.Select(cd => string.Concat(cd.Where(c => char.IsUpper(c)))).ToList();

                yield return new Valve { Name = valveName, Rate = valveRate, Connections = connections, Id = i };
            }
        }

    }
}
