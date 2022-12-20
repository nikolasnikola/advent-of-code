using System.Diagnostics;
using AdventOfCode2022.Dto.Day17;

namespace AdventOfCode2022._2022
{
    internal static class Day17
    {
        internal static void Part2()
        {
            var instructions = Reader.ReadAsString("Day17").ToCharArray();
            var instructionLength = instructions.Length;
            var debug = false;

            List<Rock> rocks = new();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var maxPossibleHeight = CreateRockStack(rocks, 2022);
            var rows = maxPossibleHeight + 5;

            var map = CreateEmtptyMap(rows);

            var currentInstructionIndex = 0;

            var currentHeight = 0;

            List<((int,int,int), (int, int, int), (int, int, int), (int, int, int), (int, int, int), int, int, int) > referencePointsMatcher = new();

            int nonRepeatablItems = 0;

            var currentFlow = ((-1, -1, -1), (-1, -1, -1), (-1, -1, -1), (-1, -1, -1), (-1, -1, -1), 0, 0, 0);
            var currentReferenceRow = 0;
            var matcherFlowTotalHeight = 0;
            var heightBellowMatcher = 0;
            int totalRocksInMatcher = 0;
            List<int> heightsPerItemAfterMatcher = new();

            foreach (var rock in rocks)
            {
                rock.SetStartPosition(currentHeight, rows);
                if (debug && rock.Shape == RockType.Minus)
                {
                    var fallingPoints = rock.GetOtherPoints();
                    fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                    PrintMap(map, true, fallingPoints, currentHeight, $"Current height {currentHeight}. Setting start");
                }
                bool isInRest = false;

                while (!isInRest)
                {
                    bool moved = false;
                    if (instructions[currentInstructionIndex % instructionLength] == '>')
                    {
                        if (rock.CanMoveRight(map))
                        {
                            moved = true;
                            rock.MoveRight();
                            if (debug && rock.Shape == RockType.Minus)
                            {
                                var fallingPoints = rock.GetOtherPoints();
                                fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                                PrintMap(map, true, fallingPoints, currentHeight, "Moved right");
                            }
                        }
                    }
                    else
                    {
                        if (rock.CanMoveLeft(map))
                        {
                            moved = true;
                            rock.MoveLeft();
                            if (debug && rock.Shape == RockType.Minus)
                            {
                                var fallingPoints = rock.GetOtherPoints();
                                fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                                PrintMap(map, true, fallingPoints, currentHeight, "Moved left");
                            }
                        }
                    }
                    currentInstructionIndex++;

                    if (rock.CanMoveDown(map))
                    {
                        rock.MoveDown();
                        if (debug && rock.Shape == RockType.Minus)
                        {
                            var fallingPoints = rock.GetOtherPoints();
                            fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                            PrintMap(map, true, fallingPoints, currentHeight, $"Moved down. Moved before: {moved}");
                        }
                    }
                    else
                    {
                        rock.Rest(map);
                        isInRest = true;
                        int heightBeforeNew = currentHeight;
                        currentHeight = Math.Max(currentHeight, rock.GetTopPosition(rows));
                        if (debug && rock.Shape == RockType.Minus)
                        {
                            PrintMap(map, true, new List<(int, int)>(), currentHeight, $"Put to rest. Moved before: {moved}");
                        }

                        if (matcherFlowTotalHeight == 0)
                        {
                            if (rock.Shape == RockType.Minus)
                            {
                                currentFlow.Item1 = (rock.ReferencePointColumn, 0, currentHeight);
                                currentReferenceRow = rock.ReferencePointRow;
                                currentFlow.Item6 = rows - 1 - rock.ReferencePointRow;
                            }
                            else if (rock.Shape == RockType.Plus)
                            {
                                currentFlow.Item2 = (rock.ReferencePointColumn, currentReferenceRow - rock.ReferencePointRow, currentHeight);
                            }
                            else if (rock.Shape == RockType.L)
                            {
                                currentFlow.Item3 = (rock.ReferencePointColumn, currentReferenceRow - rock.ReferencePointRow, currentHeight);
                            }
                            else if (rock.Shape == RockType.I)
                            {
                                currentFlow.Item4 = (rock.ReferencePointColumn, currentReferenceRow - rock.ReferencePointRow, currentHeight);
                            }
                            else
                            {
                                currentFlow.Item5 = (rock.ReferencePointColumn, currentReferenceRow - rock.ReferencePointRow, currentHeight);
                                currentFlow.Item7 = currentHeight;
                                currentFlow.Item8 = instructionLength - currentInstructionIndex % instructionLength;

                                var matched = TryMatch(referencePointsMatcher, currentFlow);
                                if (matched != default)
                                {
                                    var foundedHeight = matched.Item6;
                                    var foundedIndex = referencePointsMatcher.IndexOf(matched);

                                    heightBellowMatcher = matched.Item6 - 1;
                                    matcherFlowTotalHeight = referencePointsMatcher.Last().Item7 - heightBellowMatcher;
                                    nonRepeatablItems = foundedIndex * 5;
                                    totalRocksInMatcher = (referencePointsMatcher.Count - foundedIndex) * 5;

                                    for (int i = foundedIndex; i < referencePointsMatcher.Count; i++)
                                    {
                                        heightsPerItemAfterMatcher.Add(referencePointsMatcher[i].Item1.Item3 - heightBellowMatcher);
                                        heightsPerItemAfterMatcher.Add(referencePointsMatcher[i].Item2.Item3 - heightBellowMatcher);
                                        heightsPerItemAfterMatcher.Add(referencePointsMatcher[i].Item3.Item3 - heightBellowMatcher);
                                        heightsPerItemAfterMatcher.Add(referencePointsMatcher[i].Item4.Item3 - heightBellowMatcher);
                                        heightsPerItemAfterMatcher.Add(referencePointsMatcher[i].Item5.Item3 - heightBellowMatcher);
                                    }
                                }
                                else
                                {
                                    referencePointsMatcher.Add(currentFlow);
                                }
                            }
                        }
                    }
                }
            }

            var uncounted = Convert.ToInt32((1000000000000 - nonRepeatablItems) % totalRocksInMatcher);

            var heightAboveMatcher = uncounted > 0 ? heightsPerItemAfterMatcher[uncounted - 1] : 0;
            var result = ((1000000000000 - nonRepeatablItems) / totalRocksInMatcher) * matcherFlowTotalHeight + heightBellowMatcher + heightAboveMatcher;

            sw.Stop();

            //PrintMap(map, true, new(), currentHeight);

            Console.WriteLine("\n**************");
            Console.WriteLine($"Matcher found after height: {heightBellowMatcher}");
            Console.WriteLine($"Total rocks in matcher: {totalRocksInMatcher} rocks");
            Console.WriteLine($"Total height after 1000000000000 rocks: { result }");
            Console.WriteLine($"Time elapsed: {sw.Elapsed}");
            Console.WriteLine("**************");
        }

        internal static void Part1()
        {
            var instructions = Reader.ReadAsString("Day17").ToCharArray();
            var instructionLength = instructions.Length;
            var debug = false;

            List<Rock> rocks = new();

            var maxPossibleHeight = CreateRockStack(rocks, 2022);
            var rows = maxPossibleHeight + 5;

            var map = CreateEmtptyMap(rows);

            var currentInstructionIndex = 0;

            var currentHeight = 0;

            foreach (var rock in rocks)
            {
                rock.SetStartPosition(currentHeight, rows);
                if (debug && rock.Shape == RockType.Minus)
                {
                    var fallingPoints = rock.GetOtherPoints();
                    fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                    PrintMap(map, true, fallingPoints, currentHeight, $"Current height {currentHeight}. Setting start");
                }
                bool isInRest = false;

                while (!isInRest)
                {
                    bool moved = false;
                    if (instructions[currentInstructionIndex % instructionLength] == '>')
                    {
                        if (rock.CanMoveRight(map))
                        {
                            moved = true;
                            rock.MoveRight();
                            if (debug && rock.Shape == RockType.Minus)
                            {
                                var fallingPoints = rock.GetOtherPoints();
                                fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                                PrintMap(map, true, fallingPoints, currentHeight, "Moved right");
                            }
                        }
                    }
                    else
                    {
                        if (rock.CanMoveLeft(map))
                        {
                            moved = true;
                            rock.MoveLeft();
                            if (debug && rock.Shape == RockType.Minus)
                            {
                                var fallingPoints = rock.GetOtherPoints();
                                fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                                PrintMap(map, true, fallingPoints, currentHeight, "Moved left");
                            }
                        }
                    }
                    currentInstructionIndex++;

                    if (rock.CanMoveDown(map))
                    {
                        rock.MoveDown();
                        if (debug && rock.Shape == RockType.Minus)
                        {
                            var fallingPoints = rock.GetOtherPoints();
                            fallingPoints.Add((rock.ReferencePointRow, rock.ReferencePointColumn));
                            PrintMap(map, true, fallingPoints, currentHeight, $"Moved down. Moved before: {moved}");
                        }
                    }
                    else
                    {
                        rock.Rest(map);
                        isInRest = true;
                        currentHeight = Math.Max(currentHeight, rock.GetTopPosition(rows));
                        if (debug && rock.Shape == RockType.Minus)
                        {
                            PrintMap(map, true, new List<(int, int)>(), currentHeight, $"Put to rest. Moved before: {moved}");
                        }
                    }
                }
            }

            PrintMap(map, true, new(), currentHeight);

            Console.WriteLine("\n**************");
            Console.WriteLine($"Total height: {currentHeight}");
            Console.WriteLine("**************");
        }

        private static ((int, int, int), (int, int, int), (int, int, int), (int, int, int), (int, int, int), int, int, int) TryMatch(List<((int, int, int), (int, int, int), (int, int, int), (int, int, int), (int, int, int), int, int, int)> referencePointsMatcher, ((int, int, int), (int, int, int), (int, int, int), (int, int, int), (int, int, int), int, int, int) currentFlow)
        {
            return referencePointsMatcher
                .SingleOrDefault(r => r.Item1.Item1 == currentFlow.Item1.Item1 && r.Item1.Item2 == currentFlow.Item1.Item2 &&
                                        r.Item2.Item1 == currentFlow.Item2.Item1 && r.Item2.Item2 == currentFlow.Item2.Item2 &&
                                        r.Item3.Item1 == currentFlow.Item3.Item1 && r.Item3.Item2 == currentFlow.Item3.Item2 &&
                                        r.Item4.Item1 == currentFlow.Item4.Item1 && r.Item4.Item2 == currentFlow.Item4.Item2 &&
                                        r.Item5.Item1 == currentFlow.Item5.Item1 && r.Item5.Item2 == currentFlow.Item5.Item2 &&
                                        r.Item8 == currentFlow.Item8);
        }

        static char[,] CreateEmtptyMap(int rows)
        {
            var cols = 9;
            var map = new char[rows, cols];

            for (int i = 0; i < rows * cols; i++) map[i / cols, i % cols] = '.';

            for (int i = 0; i < rows; i++)
            {
                if (i == rows - 1)
                {
                    map[i, 0] = '+';
                    map[i, cols - 1] = '+';
                }
                else
                {
                    map[i, 0] = '|';
                    map[i, cols - 1] = '|';
                }
            }

            for (int i = 1; i < cols - 1; i++)
            {
                map[rows - 1, i] = '-';
            }

            return map;
        }

        static void PrintMap(char[,] map, bool spaceBetweenRows, List<(int, int)> fallingRock, int currentHeight, string info = "")
        {
            Console.WriteLine("**************");
            Console.WriteLine("Map");
            Console.WriteLine(info);
            Console.WriteLine("**************");

            var betweenRows = spaceBetweenRows ? " " : string.Empty;
            var totalRows = map.GetLength(0);

            for (int i = totalRows - currentHeight - 10; i < totalRows; i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.ResetColor();

                    if (fallingRock.Contains((i, j)))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("o" + betweenRows);
                        continue;
                    }
                    else if (map[i, j] == '=')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (map[i, j] == '+')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (map[i, j] == '@')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (map[i, j] == 'M')
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (map[i, j] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    Console.Write(map[i, j] + betweenRows);
                }
                Console.ResetColor();
                Console.WriteLine();
            }


            //Thread.Sleep(10);
        }

        private static int CreateRockStack(List<Rock> rocks, int totalRocks)
        {
            int maxPossibleHeight = 0;

            foreach (var i in Enumerable.Range(0, totalRocks))
            {
                switch (i % 5)
                {
                    case 0:
                        var rockM = new Rock(RockType.Minus);
                        rocks.Add(rockM);
                        maxPossibleHeight += rockM.TotalHeight;
                        break;

                    case 1:
                        var rockP = new Rock(RockType.Plus);
                        rocks.Add(rockP);
                        maxPossibleHeight += rockP.TotalHeight;
                        break;

                    case 2:
                        var rockL = new Rock(RockType.L);
                        rocks.Add(rockL);
                        maxPossibleHeight += rockL.TotalHeight;
                        break;

                    case 3:
                        var rockI = new Rock(RockType.I);
                        rocks.Add(rockI);
                        maxPossibleHeight += rockI.TotalHeight;
                        break;

                    default:
                        var rockS = new Rock(RockType.Square);
                        rocks.Add(rockS);
                        maxPossibleHeight += rockS.TotalHeight;
                        break;
                }
            }

            return maxPossibleHeight;
        }
    }
}
