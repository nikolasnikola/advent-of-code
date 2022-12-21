namespace AdventOfCode2022.Dto.Day16
{
    public class Valve
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public int Rate { get; set; }

        public List<string> Connections { get; set; }

        public List<uint> ConnectionIds { get; set; } = new List<uint>();
    }
}
