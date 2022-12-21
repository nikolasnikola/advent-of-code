using AdventOfCode2022.Dto.Day16;
using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;

namespace AdventOfCode2022._2022
{
    internal static class Day16
    {
        private static Dictionary<(uint, uint), int> cachedPaths = new();

        internal static void Part2()
        {
            var valvesData = Reader.ReadAsStringList("Day16");

            var graph = new Graph();
            var valvesByName = GetValves(valvesData, graph).ToList().ToDictionary(k => k.Name, v => v);
            var valvesById = valvesByName.Values.ToDictionary(k => k.Id, v => v);

            foreach (var valve in valvesByName.Values)
            {
                foreach (var tunnel in valve.Connections)
                {
                    var connetctionId = valvesByName[tunnel].Id;
                    valve.ConnectionIds.Add(connetctionId);
                    graph.Connect(valve.Id, connetctionId, 1);
                }
            }

            var nonZeros = valvesById.Values.Where(v => v.Rate > 0).Select(v => v.Id).ToArray();
            var start = valvesById.Values.Single(valve => valve.Name == "AA").Id;
            int result = 0;

            Search(start, nonZeros, (26, 0), graph, valvesById, true, start, ref result);

            Console.WriteLine(result);
        }

        internal static void Part1()
        {
            var valvesData = Reader.ReadAsStringList("Day16");

            var graph = new Graph();
            var valvesByName = GetValves(valvesData, graph).ToList().ToDictionary(k => k.Name, v => v);
            var valvesById = valvesByName.Values.ToDictionary(k => k.Id, v => v);

            foreach (var valve in valvesByName.Values)
            {
                foreach (var tunnel in valve.Connections)
                {
                    var connetctionId = valvesByName[tunnel].Id;
                    valve.ConnectionIds.Add(connetctionId);
                    graph.Connect(valve.Id, connetctionId, 1);
                }
            }

            var nonZeros = valvesById.Values.Where(v => v.Rate > 0).Select(v => v.Id).ToArray();
            var start = valvesById.Values.Single(valve => valve.Name == "AA").Id;
            int result = 0;

            Search(start, nonZeros, (30, 0), graph, valvesById, false, start, ref result);

            Console.WriteLine(result);
        }

        static (int T, int Score) Move(uint from, uint to, (int T, int Score) previous, Graph graph, Dictionary<uint, Valve> valvesById)
        {
            if (!cachedPaths.TryGetValue((from, to), out var dist))
            {
                dist = graph.Dijkstra(from, to).Distance;
                cachedPaths.Add((from, to), dist);
            }

            var tEnd = previous.T - dist - 1;
            return (tEnd, previous.Score + (tEnd * valvesById[to].Rate));
        }

        static void Search(uint from, uint[] to, (int T, int Score) previous, Graph graph, Dictionary<uint, Valve> valvesById, bool elephant, uint start, ref int highScore)
        {
            foreach (var t in to)
            {
                var move = Move(from, t, previous, graph, valvesById);
                if (move.T >= 0)
                {
                    highScore = Math.Max(move.Score, highScore);

                    if (to.Length > 1)
                    {
                        Search(t, to.Where(j => j != t).ToArray(), move, graph, valvesById, elephant, start, ref highScore);
                    }
                }
                else if (elephant && previous.Score >= highScore / 2)
                {
                    Search(start, to, (26, previous.Score), graph, valvesById, false, start, ref highScore);
                }
            }
        }

        static IEnumerable<Valve> GetValves(string[] valvesData, Graph graph)
        {
            for (int i = 0; i < valvesData.Length; i++)
            {
                var vd = valvesData[i];

                var vdInfo = vd.Split("; ");
                var valveName = vdInfo[0].Substring(6, 2);
                var valveRate = int.Parse(vdInfo[0][23..]);

                var connectionsData = vdInfo[1].Split(", ");
                var connections = connectionsData.Select(cd => string.Concat(cd.Where(c => char.IsUpper(c)))).ToList();

                yield return new Valve { Name = valveName, Rate = valveRate, Connections = connections, Id = graph.AddNode() };
            }
        }
    }
}
