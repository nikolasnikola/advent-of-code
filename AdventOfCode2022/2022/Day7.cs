using AdventOfCode.Models.Day7;

namespace AdventOfCode._2022
{
    internal static class Day7
    {
        internal static void Part2()
        {
            var commands = Reader.ReadAsStringList("2022", "Day7");
            var dir = GetTree(commands);

            var flatDirectories = GetWithSubItems(dir).ToList();

            var requiredSpaceToDelete = 30000000 - (70000000 - dir.Size);

            var sizes = flatDirectories.Select(d => d.Size).ToList();

            var result = sizes.Where(d => d >= requiredSpaceToDelete).Min();

            Console.WriteLine(result);
        }

        internal static void Part1()
        {
            var commands = Reader.ReadAsStringList("2022", "Day7");
            var dir = GetTree(commands);

            var flatDirectories = GetWithSubItems(dir);

            var result = flatDirectories.Where(d => d.Size <= 100000).Select(d => d.Size).Sum();

            Console.WriteLine(result);
        }

        static IEnumerable<Dir> GetWithSubItems(Dir root)
        {
            var retList = new List<Dir> { root };

            foreach (var sub in root.Directories)
            {
                retList.AddRange(GetWithSubItems(sub));
            }

            return retList;
        }


        static Dir GetTree(string[] commands)
        {
            Dir root = new(null, "/");
            Dir currentDirectory = root;

            foreach (var command in commands.Skip(1))
            {
                if (command.StartsWith("$ cd"))
                {
                    currentDirectory = ChangeDirectory(root, currentDirectory, command);
                }
                else if (command == "$ ls")
                {
                    continue;
                }
                else if (command.StartsWith("dir"))
                {
                    var dirName = command[4..];
                    currentDirectory.AddDir(new Dir(currentDirectory, dirName));
                }
                else
                {
                    var file = command.Split(" ");
                    var size = int.Parse(file[0]);
                    var name = file[1];

                    currentDirectory.Files.Add(new F() { Name = name, Size = size });
                }
            }

            return root;
        }

        static Dir ChangeDirectory(Dir root, Dir currentDirectory, string command)
        {
            if (command.StartsWith("$ cd .."))
            {
                currentDirectory = currentDirectory.Parent!;
            }
            else if (command.StartsWith("$ cd /"))
            {
                currentDirectory = root;
            }
            else
            {
                var dirName = command[5..];
                currentDirectory = currentDirectory.Directories.Single(d => d.Name == dirName);
            }

            return currentDirectory;
        }
    }
}
