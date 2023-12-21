namespace AdventOfCode._2023
{
    internal static class Day20
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2023", "Day20");

            var modules = new List<IModule>();
            PopulateModules(input, modules);

            var firstPulse = new PulseResult() { Pulse = Pulse.Low, ToModule = "broadcaster", FromModule = "button"};
            var allPulses = new List<PulseResult>() { firstPulse };
            var buttonCounter = 1;
            var dictionary = modules.ToDictionary(x => x.Name, x => x);
            var currentIteration = 0;
            bool iterationFinished = false;

            while (!(iterationFinished && (modules.Where(m => m is FlipFlopModule ffm).All(m => !(m as FlipFlopModule).IsOn) || buttonCounter == 1000)))
            {
                if (iterationFinished) 
                { 
                    allPulses.Add(new PulseResult() { Pulse = Pulse.Low, ToModule = "broadcaster", FromModule = "button" }); 
                    buttonCounter++;
                }

                var pulse = allPulses[currentIteration++];

                if (dictionary.ContainsKey(pulse.ToModule))
                {
                    var currentModule = dictionary[pulse.ToModule];

                    var processResult = currentModule.ProceedPulse(pulse.Pulse, pulse.FromModule);
                    if (processResult != null && processResult.Count > 0)
                    {
                        allPulses.AddRange(processResult);
                    }
                }

                iterationFinished = allPulses.Count == currentIteration;
            }

            var totalRequiredCycles = 1000 / buttonCounter;
            var remainder = 1000 % buttonCounter;

            var lowPulsesInCycle = allPulses.Count(x => x.Pulse == Pulse.Low);
            var highPulses = allPulses.Count - lowPulsesInCycle;

            var result = totalRequiredCycles * lowPulsesInCycle * totalRequiredCycles * highPulses + remainder;

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2023", "Day20");

            var modules = new List<IModule>();
            PopulateModules(input, modules);

            // manual input analyze
            var parentTargetModule = (ConjuctionModule) modules.Single(x => x.ConnectedTo.Contains("rx")); // there is only single conjuction module with rx output real input
            var parentsOfParent = parentTargetModule.InputModuleStates.Keys; // 4 conjuction module with parent module output

            var firstPulse = new PulseResult() { Pulse = Pulse.Low, ToModule = "broadcaster", FromModule = "button" };
            var allPulses = new List<PulseResult>() { firstPulse };
            long buttonCounter = 1;
            var dictionary = modules.ToDictionary(x => x.Name, x => x);
            var currentIteration = 0;
            bool iterationFinished = false;

            var parentOFParentStates = parentsOfParent.ToDictionary(x => x, x => (false, (long)0));

            // look when each parentOfParent sends high pulse to their output
            while (parentOFParentStates.Values.Any(x => !x.Item1))
            {
                if (iterationFinished)
                {
                    allPulses.Add(new PulseResult() { Pulse = Pulse.Low, ToModule = "broadcaster", FromModule = "button" });
                    buttonCounter++;
                }

                var pulse = allPulses[currentIteration++];

                if (parentOFParentStates.ContainsKey(pulse.FromModule) 
                    && ((ConjuctionModule)dictionary[pulse.FromModule]).InputModuleStates.Any(x => x.Value == Pulse.Low))
                {
                    parentOFParentStates[pulse.FromModule] = (true, buttonCounter);
                }

                if (dictionary.ContainsKey(pulse.ToModule))
                {
                    var currentModule = dictionary[pulse.ToModule];

                    var processResult = currentModule.ProceedPulse(pulse.Pulse, pulse.FromModule);
                    if (processResult != null && processResult.Count > 0)
                    {
                        allPulses.AddRange(processResult);
                    }
                }

                iterationFinished = allPulses.Count == currentIteration;
            }

            long result = 1;

            foreach (var counter in parentOFParentStates.Values.Select(x => x.Item2))
            {
                result *= counter;
            }

            Console.WriteLine(result);
        }

        private static void PopulateModules(string[] input, List<IModule> modules)
        {
            var conjuctionModules = new List<ConjuctionModule>();

            foreach (var item in input)
            {
                var splitted = item.Split(" -> ");
                var moduleName = splitted[0];
                var connectedTo = splitted[1].Split(',', StringSplitOptions.TrimEntries);

                if (moduleName == "broadcaster")
                {
                    var module = new BroadcastModule() { Name = moduleName, ConnectedTo = connectedTo.ToList() };
                    modules.Add(module);
                }
                else if (moduleName.StartsWith('%'))
                {
                    var module = new FlipFlopModule() { Name = moduleName[1..], ConnectedTo = connectedTo.ToList() };
                    modules.Add(module);
                }
                else
                {
                    var module = new ConjuctionModule() { Name = moduleName[1..], ConnectedTo = connectedTo.ToList() };
                    modules.Add(module);
                    conjuctionModules.Add(module);
                }
            }

            foreach (var conModule in conjuctionModules)
            {
                var inputs = modules.Where(x => x.ConnectedTo.Contains(conModule.Name)).Select(x => x.Name);
                conModule.InitializeInputStates(inputs);
            }
        }
    }
}

public interface IModule
{
    public string Name { get; set; }
    public IList<string> ConnectedTo { get; set; }
    IList<PulseResult> ProceedPulse(Pulse pulse, string inputModule);
}
public abstract class Module
{
    public string Name { get; set; }
    public IList<string> ConnectedTo { get; set; }
}

public class FlipFlopModule : Module, IModule
{
    public FlipFlopModule()
    {
        IsOn = false;
    }

    public bool IsOn { get; set; }

    public IList<PulseResult> ProceedPulse(Pulse pulse, string inputModule)
    {
        if (pulse == Pulse.High)
        {
            return null;
        }

        IsOn = !IsOn;
        Pulse pulseToSend = IsOn ? Pulse.High : Pulse.Low;

        return ConnectedTo.Select(connection => new PulseResult { Pulse = pulseToSend, ToModule = connection, FromModule = Name }).ToList();
    }
}

public class ConjuctionModule : Module, IModule
{
    public ConjuctionModule()
    {
        InputModuleStates = new Dictionary<string, Pulse>();
    }

    public Dictionary<string, Pulse> InputModuleStates { get; set; }

    public void InitializeInputStates(IEnumerable<string> inputModules)
    {
        foreach (var module in inputModules)
        {
            InputModuleStates.Add(module, Pulse.Low);
        }
    }

    public IList<PulseResult> ProceedPulse(Pulse pulse, string inputModule)
    {
        InputModuleStates[inputModule] = pulse;

        var pulseToSend = InputModuleStates.Values.All(x => x == Pulse.High) ? Pulse.Low : Pulse.High;

        return ConnectedTo.Select(connection => new PulseResult { Pulse = pulseToSend, ToModule = connection, FromModule = Name }).ToList();
    }
}

public class BroadcastModule : Module, IModule
{
    public IList<PulseResult> ProceedPulse(Pulse pulse, string inputModule)
    {
        return ConnectedTo.Select(connection => new PulseResult { Pulse = pulse, ToModule = connection, FromModule = Name }).ToList();
    }
}

public class PulseResult
{
    public string ToModule { get; set; }
    public Pulse Pulse { get; set; }
    public string FromModule { get; set; }
}

public enum Pulse
{
    Low,
    High,
}