namespace AdventOfCode._2023
{
    internal static class Day16
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsCharMatrix("2023", "Day16");

            var memo = new List<(int, int, Heading)>();

            var beams = new List<Beam> { new Beam { CurrentPosition = (0, 0), Heading = Heading.Right, IsDone = false } };
            memo.Add((0, 0, Heading.Right));

            while (beams.Any(x => !x.IsDone))
            {
                var newBeams = new List<Beam>();
                foreach (var beam in beams)
                {
                    if (beam.IsDone) continue;

                    Move(beam, input, newBeams);

                    if (!beam.IsDone)
                    {
                        if (memo.Any(x => x.Item1 == beam.CurrentPosition.Item1 && x.Item2 == beam.CurrentPosition.Item2 && x.Item3 == beam.Heading))
                            beam.IsDone = true;
                        else memo.Add((beam.CurrentPosition.Item1, beam.CurrentPosition.Item2, beam.Heading));
                    }
                }

                beams.AddRange(newBeams);
                memo.AddRange(newBeams.Select(x => (x.CurrentPosition.Item1, x.CurrentPosition.Item2, x.Heading)));
            }

            Console.WriteLine(memo.Select(x => (x.Item1, x.Item2)).Distinct().Count());
        }

        // need to optimizeeeeeeeee
        internal static void Part2()
        {
            var input = Reader.ReadAsCharMatrix("2023", "Day16");

            var max = 0;

            var startPoints = new List<(int, int, Heading)>();
            startPoints.AddRange(Enumerable.Range(0, input.GetLength(1)).Select(x => (0, x, Heading.Bottom)));
            startPoints.AddRange(Enumerable.Range(0, input.GetLength(1)).Select(x => (input.GetLength(0)-1, x, Heading.Top)));
            startPoints.AddRange(Enumerable.Range(0, input.GetLength(0)).Select(x => (x, 0, Heading.Right)));
            startPoints.AddRange(Enumerable.Range(0, input.GetLength(0)).Select(x => (x, input.GetLength(1) - 1, Heading.Left)));

            foreach(var sp in startPoints)
            {
                var memo = new List<(int, int, Heading)>();

                var beams = new List<Beam> { new Beam { CurrentPosition = (sp.Item1, sp.Item2), Heading = sp.Item3, IsDone = false } };
                memo.Add((sp.Item1, sp.Item2, sp.Item3));

                while (beams.Any(x => !x.IsDone))
                {
                    var newBeams = new List<Beam>();
                    foreach (var beam in beams)
                    {
                        if (beam.IsDone) continue;

                        Move(beam, input, newBeams);

                        if (!beam.IsDone)
                        {
                            if (memo.Any(x => x.Item1 == beam.CurrentPosition.Item1 && x.Item2 == beam.CurrentPosition.Item2 && x.Item3 == beam.Heading))
                                beam.IsDone = true;
                            else memo.Add((beam.CurrentPosition.Item1, beam.CurrentPosition.Item2, beam.Heading));
                        }
                    }

                    beams.AddRange(newBeams);
                    memo.AddRange(newBeams.Select(x => (x.CurrentPosition.Item1, x.CurrentPosition.Item2, x.Heading)));
                }

                max = Math.Max(max, memo.Select(x => (x.Item1, x.Item2)).Distinct().Count());
                
            }

            Console.WriteLine(max);
        }

        private static void Move(Beam beam, char[,] field, List<Beam> newBeams)
        {
            var maxRow = field.GetLength(0)-1;
            var maxCol = field.GetLength(1)-1;
            var currentItem = field[beam.CurrentPosition.Item1, beam.CurrentPosition.Item2];

            if (beam.Heading == Heading.Left)
            {
                if (currentItem == '.')
                {
                    if (beam.CurrentPosition.Item2 > 0) beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2 - 1);
                    else beam.IsDone = true;
                }
                if (currentItem == '/')
                {
                    if (beam.CurrentPosition.Item1 < maxRow)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1 + 1, beam.CurrentPosition.Item2);
                        beam.Heading = Heading.Bottom;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '\\')
                {
                    if (beam.CurrentPosition.Item1 > 0)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1 - 1, beam.CurrentPosition.Item2);
                        beam.Heading = Heading.Top;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '-')
                {
                    if (beam.CurrentPosition.Item2 > 0) beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2 - 1);
                    else beam.IsDone = true;
                }
                if (currentItem == '|')
                {
                    var upRow = beam.CurrentPosition.Item1 - 1;
                    var bottomRow = beam.CurrentPosition.Item1 + 1;

                    if (upRow >= 0)
                    {
                        beam.CurrentPosition = (upRow, beam.CurrentPosition.Item2);
                        beam.Heading = Heading.Top;
                    }
                    else beam.IsDone = true;

                    if (bottomRow <= maxRow)
                    {
                        var newBeam = new Beam { CurrentPosition = (bottomRow, beam.CurrentPosition.Item2), Heading = Heading.Bottom, IsDone = false };
                        newBeams.Add(newBeam);
                    }
                }
            }

            else if (beam.Heading == Heading.Right)
            {
                if (currentItem == '.')
                {
                    if (beam.CurrentPosition.Item2 < maxCol) beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2 + 1);
                    else beam.IsDone = true;
                }
                if (currentItem == '/')
                {
                    if (beam.CurrentPosition.Item1 > 0)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1 - 1, beam.CurrentPosition.Item2);
                        beam.Heading = Heading.Top;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '\\')
                {
                    if (beam.CurrentPosition.Item1 < maxRow)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1 + 1, beam.CurrentPosition.Item2);
                        beam.Heading = Heading.Bottom;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '-')
                {
                    if (beam.CurrentPosition.Item2 < maxCol) beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2 + 1);
                    else beam.IsDone = true;
                }
                if (currentItem == '|')
                {
                    var upRow = beam.CurrentPosition.Item1 - 1;
                    var bottomRow = beam.CurrentPosition.Item1 + 1;

                    if (upRow >= 0)
                    {
                        beam.CurrentPosition = (upRow, beam.CurrentPosition.Item2);
                        beam.Heading = Heading.Top;
                    }
                    else beam.IsDone = true;

                    if (bottomRow <= maxRow)
                    {
                        var newBeam = new Beam { CurrentPosition = (bottomRow, beam.CurrentPosition.Item2), Heading = Heading.Bottom, IsDone = false };
                        newBeams.Add(newBeam);
                    }
                }
            }

            else if (beam.Heading == Heading.Top)
            {
                if (currentItem == '.')
                {
                    if (beam.CurrentPosition.Item1 > 0) beam.CurrentPosition = (beam.CurrentPosition.Item1-1, beam.CurrentPosition.Item2);
                    else beam.IsDone = true;
                }
                if (currentItem == '/')
                {
                    if (beam.CurrentPosition.Item2 < maxCol)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2+1);
                        beam.Heading = Heading.Right;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '\\')
                {
                    if (beam.CurrentPosition.Item2 > 0)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2-1);
                        beam.Heading = Heading.Left;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '|')
                {
                    if (beam.CurrentPosition.Item1 > 0) beam.CurrentPosition = (beam.CurrentPosition.Item1 - 1, beam.CurrentPosition.Item2);
                    else beam.IsDone = true;
                }
                if (currentItem == '-')
                {
                    var leftCol = beam.CurrentPosition.Item2 - 1;
                    var rightCol = beam.CurrentPosition.Item2 + 1;
                    if (leftCol >= 0)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1, leftCol);
                        beam.Heading = Heading.Left;
                    }
                    else beam.IsDone = true;

                    if (rightCol <= maxCol)
                    {
                        var newBeam = new Beam { CurrentPosition = (beam.CurrentPosition.Item1, rightCol), Heading = Heading.Right, IsDone = false };
                        newBeams.Add(newBeam);
                    }
                }
            }

            else if (beam.Heading == Heading.Bottom)
            {
                if (currentItem == '.')
                {
                    if (beam.CurrentPosition.Item1 < maxRow) beam.CurrentPosition = (beam.CurrentPosition.Item1 + 1, beam.CurrentPosition.Item2);
                    else beam.IsDone = true;
                }
                if (currentItem == '/')
                {
                    if (beam.CurrentPosition.Item2 > 0)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2 - 1);
                        beam.Heading = Heading.Left;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '\\')
                {
                    if (beam.CurrentPosition.Item2 < maxCol)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1, beam.CurrentPosition.Item2 + 1);
                        beam.Heading = Heading.Right;
                    }
                    else beam.IsDone = true;
                }
                if (currentItem == '|')
                {
                    if (beam.CurrentPosition.Item1 < maxRow) beam.CurrentPosition = (beam.CurrentPosition.Item1 + 1, beam.CurrentPosition.Item2);
                    else beam.IsDone = true;
                }
                if (currentItem == '-')
                {
                    var leftCol = beam.CurrentPosition.Item2 - 1;
                    var rightCol = beam.CurrentPosition.Item2 + 1;
                    if (leftCol >= 0)
                    {
                        beam.CurrentPosition = (beam.CurrentPosition.Item1, leftCol);
                        beam.Heading = Heading.Left;
                    }
                    else beam.IsDone = true;

                    if (rightCol <= maxCol)
                    {
                        var newBeam = new Beam { CurrentPosition = (beam.CurrentPosition.Item1, rightCol), Heading = Heading.Right, IsDone = false };
                        newBeams.Add(newBeam);
                    }
                }
            }
        }
    }

    internal class Beam
    {
        public (int, int) CurrentPosition { get; set; }
        public Heading Heading { get; set; }
        public bool IsDone { get; set; }
    }

    internal enum Heading
    {
        Right,
        Left,
        Bottom,
        Top
    }

}