using AdventOfCode2022.Dto.Day23;

namespace AdventOfCode2022._2022
{
    internal static class Day23
    {
        internal static void Part2()
        {
            var map = Reader.ReadAsCharMatrix("Day23");
            var positions = GetInitialPositioning(map);

            var result = ProceedMoving2(positions);

            Print(positions, true);

            Console.WriteLine("**************");
            Console.WriteLine($"Round without moves: {result}");
            Console.WriteLine("**************");

        }

        internal static void Part1()
        {
            var map = Reader.ReadAsCharMatrix("Day23");
            var positions = GetInitialPositioning(map);

            ProceedMoving(positions, 10);

            Print(positions, true);

            var boundings = GetBoundings(positions);

            var result = (boundings.maxR - boundings.minR + 1) * (boundings.maxC - boundings.minC + 1) - positions.Count;

            Console.WriteLine("**************");
            Console.WriteLine($"Empty fields: {result}");
            Console.WriteLine("**************");

        }

        static int ProceedMoving2(List<ElfPositioning> positions)
        {
            Direction? lastDirection = null;
            bool anythingToMove = true;
            int roundCounter = 0;

            // optimizeeeeeeeeeeeeeeeeeeeee
            // slow, but works :)
            while (anythingToMove)
            {
                var proposedMoves = new List<(ElfPositioning elf, int newRow, int newColumn)>();

                foreach (var elf in positions)
                {
                    var canMove = elf.CanMoveSomewhere(positions, lastDirection);

                    if (canMove != null)
                    {
                        proposedMoves.Add(canMove.Value);
                    }
                }

                var toBeMoved = proposedMoves.GroupBy(p => (p.newRow, p.newColumn)).Where(gp => gp.Count() == 1).Select(g => g.Single());

                if (!toBeMoved.Any()) 
                {
                    anythingToMove = false;
                }

                foreach (var g in toBeMoved)
                {
                    g.elf.Move(g.newRow, g.newColumn);
                }

                lastDirection = ElfPositioning.GetNextDirection(lastDirection);
                roundCounter++;

                //Print(positions, true);
            }

            return roundCounter;
        }

        static void ProceedMoving(List<ElfPositioning> positions, int rounds)
        {
            Direction? lastDirection = null;

            foreach (var i in Enumerable.Range(0, rounds))
            {
                var proposedMoves = new List<(ElfPositioning elf, int newRow, int newColumn)>();

                foreach (var elf in positions)
                {
                    var canMove = elf.CanMoveSomewhere(positions, lastDirection);

                    if (canMove != null)
                    {
                        proposedMoves.Add(canMove.Value);
                    }
                }

                var toBeMoved = proposedMoves.GroupBy(p => (p.newRow, p.newColumn)).Where(gp => gp.Count() == 1).Select(g => g.Single());

                foreach (var g in toBeMoved)
                {
                    g.elf.Move(g.newRow, g.newColumn);
                }

                lastDirection = ElfPositioning.GetNextDirection(lastDirection);

                //Print(positions, true);
            }
        }

        static List<ElfPositioning> GetInitialPositioning(char[,] map)
        {
            var elves = new List<ElfPositioning>();

            var lastElfId = 0;

            for (int i = 0; i< map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i,j] == '#')
                    {
                        elves.Add(new(++lastElfId, i, j));
                    }
                }
            }
            return elves;
        }

        public static (int maxR, int minR, int maxC, int minC) GetBoundings(IEnumerable<ElfPositioning> elves)
        {
            var result = elves.Aggregate(
                  new
                  {
                      MinR = int.MaxValue,
                      MaxR = int.MinValue,
                      MinC = int.MaxValue,
                      MaxC = int.MinValue,
                  },
                  (accumulator, e) => new
                  {
                      MinR = Math.Min(e.CurrentRow, accumulator.MinR),
                      MaxR = Math.Max(e.CurrentRow, accumulator.MaxR),
                      MinC = Math.Min(e.CurrentColumn, accumulator.MinC),
                      MaxC = Math.Max(e.CurrentColumn, accumulator.MaxC),
                  });

            return (result.MaxR, result.MinR, result.MaxC, result.MinC);
        }

        static void Print(List<ElfPositioning> elves, bool spaceBetweenRows)
        {
            var boundings = GetBoundings(elves);
            var betweenRows = spaceBetweenRows ? " " : string.Empty;

            Console.WriteLine("**************");
            Console.WriteLine("Map");
            Console.WriteLine("**************");

            for (int i = boundings.minR; i<= boundings.maxR; i++)
            {
                for (int j = boundings.minC; j <= boundings.maxC; j++)
                {
                    var toprint = elves.Any(e => e.CurrentRow == i && e.CurrentColumn== j) ? '#' : '.';

                    Console.Write(toprint);
                }

                Console.WriteLine();
            }
        }


    }
}
