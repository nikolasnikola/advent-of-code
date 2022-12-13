using System.Collections.Immutable;

namespace AdventOfCode2022.Dto.Day7
{
    public class Dir
    {
        private readonly HashSet<Dir> _children = new();

        public Dir(Dir? parent, string name) { Parent = parent; Name = name; }

        public string Name { get; set; }

        public IEnumerable<Dir> Directories => _children.ToImmutableHashSet();

        public Dir? Parent { get; set; }

        public IList<F> Files { get; set; } = new List<F>();

        public int Size => Files.Select(f => f.Size).Sum() + _children.Select(d => d.Size).Sum();

        public void AddDir(Dir dir)
        {
            _children.Add(dir);
        }
    }
}
