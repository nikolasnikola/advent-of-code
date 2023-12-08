using AdventOfCode.Models.Day9;

namespace AdventOfCode._2022
{
    internal static class Day9
    {
        internal static void Part2()
        {
            MoveRope(MakeSteps2);
        }

        internal static void Part1()
        {
            MoveRope(MakeSteps);
        }

        static void MoveRope(Func<Movement[], int, int, int> stepsFunction)
        {
            var moveSeries = Reader.ReadAsObjectArray<Movement>("2022", "Day9");

            var movementsCount = moveSeries.Aggregate(
                new
                {
                    TotalUp = 0,
                    TotalDown = 0,
                    TotalLeft = 0,
                    TotalRight = 0,
                },
                (accumulator, m) => new
                {
                    TotalUp = m!.Direction == Direction.Up ? accumulator.TotalUp + m.Steps : accumulator.TotalUp,
                    TotalDown = m.Direction == Direction.Down ? accumulator.TotalDown + m.Steps : accumulator.TotalDown,
                    TotalLeft = m.Direction == Direction.Left ? accumulator.TotalLeft + m.Steps : accumulator.TotalLeft,
                    TotalRight = m.Direction == Direction.Right ? accumulator.TotalRight + m.Steps : accumulator.TotalRight
                });

            int UpDownDifference = movementsCount.TotalUp - movementsCount.TotalDown;
            int RightLeftDifference = movementsCount.TotalLeft - movementsCount.TotalRight;

            int rows = Math.Max(movementsCount.TotalUp, movementsCount.TotalDown) + 1;
            int cols = Math.Max(movementsCount.TotalRight, movementsCount.TotalLeft) + 1;

            var startingRow = UpDownDifference >= 0 ? rows - 1 : 0;
            var startingColumn = RightLeftDifference >= 0 ? cols - 1 : 0;

            var result = stepsFunction(moveSeries!, startingRow, startingColumn);

            Console.WriteLine(result);
        }

        static int MakeSteps(Movement[] movements, int row, int column)
        {
            var H = (row, column);
            var T = (row, column);

            var THistory = new List<(int, int)>() { T };


            foreach (var step in movements)
            {
                var action = FindMovementAction(step);

                for (int i = 0; i < step.Steps; i++)
                {
                    H = action(H.row, H.column);

                    var tailAction = ShouldMoveTail(H, T);

                    if (tailAction != null)
                    {
                        T = tailAction(T.row, T.column);
                        THistory.Add(T);
                    }
                }
            }

            return THistory.Distinct().Count();
        }

        static int MakeSteps2(Movement[] movements, int row, int column)
        {
            var H = (row, column);
            var K1 = (row, column);
            var K2 = (row, column);
            var K3 = (row, column);
            var K4 = (row, column);
            var K5 = (row, column);
            var K6 = (row, column);
            var K7 = (row, column);
            var K8 = (row, column);
            var T = (row, column);

            var THistory = new List<(int, int)>() { T };


            foreach (var step in movements)
            {
                var action = FindMovementAction(step);

                for (int i = 0; i < step.Steps; i++)
                {
                    H = action(H.row, H.column);

                    K1 = MoveKnotIfNeeded(H, K1);
                    K2 = MoveKnotIfNeeded(K1, K2);
                    K3 = MoveKnotIfNeeded(K2, K3);
                    K4 = MoveKnotIfNeeded(K3, K4);
                    K5 = MoveKnotIfNeeded(K4, K5);
                    K6 = MoveKnotIfNeeded(K5, K6);
                    K7 = MoveKnotIfNeeded(K6, K7);
                    K8 = MoveKnotIfNeeded(K7, K8);

                    var tailAction = ShouldMoveTail(K8, T);

                    if (tailAction != null)
                    {
                        T = tailAction(T.row, T.column);
                        THistory.Add(T);
                    }
                }
            }

            return THistory.Distinct().Count();
        }

        static (int, int) MoveKnotIfNeeded((int, int) headKnot, (int, int) tailKnot)
        {
            var tailAction = ShouldMoveTail(headKnot, tailKnot);

            if (tailAction != null)
            {
                tailKnot = tailAction(tailKnot.Item1, tailKnot.Item2);
            }

            return tailKnot;
        }

        static Func<int, int, (int, int)> FindMovementAction(Movement movement) => movement.Direction switch
        {
            Direction.Up => MoveUp,
            Direction.Down => MoveDown,
            Direction.Left => MoveLeft,
            _ => MoveRight,
        };

        static Func<int, int, (int, int)>? ShouldMoveTail((int, int) headPosition, (int, int) tailPosition)
        {
            var shouldMove = Math.Abs(headPosition.Item1 - tailPosition.Item1) > 1
                || Math.Abs(headPosition.Item2 - tailPosition.Item2) > 1;

            if (!shouldMove) return null;

            bool isSameRow = headPosition.Item1 == tailPosition.Item1;
            bool isSameColumn = headPosition.Item2 == tailPosition.Item2;

            if (isSameRow && !isSameColumn)
            {
                return headPosition.Item2 > tailPosition.Item2 ? MoveRight : MoveLeft;
            }

            if (isSameColumn && !isSameRow)
            {
                return headPosition.Item1 > tailPosition.Item1 ? MoveDown : MoveUp;
            }

            if (!isSameRow && !isSameColumn)
            {
                var isHeadUpper = headPosition.Item1 < tailPosition.Item1;
                var isHeadLefter = headPosition.Item2 < tailPosition.Item2;

                return isHeadUpper && isHeadLefter
                    ? MoveUpLeft
                    : isHeadUpper && !isHeadLefter
                    ? MoveUpRight
                    : !isHeadUpper && isHeadLefter
                    ? MoveDownLeft
                    : MoveDownRight;
            }

            return null;
        }

        static (int, int) MoveUp(int currentRow, int currentColumn)
        {
            return (currentRow - 1, currentColumn);
        }

        static (int, int) MoveDown(int currentRow, int currentColumn)
        {
            return (currentRow + 1, currentColumn);
        }

        static (int, int) MoveLeft(int currentRow, int currentColumn)
        {
            return (currentRow, currentColumn - 1);
        }

        static (int, int) MoveRight(int currentRow, int currentColumn)
        {
            return (currentRow, currentColumn + 1);
        }

        static (int, int) MoveUpLeft(int currentRow, int currentColumn)
        {
            return (currentRow - 1, currentColumn - 1);
        }

        static (int, int) MoveUpRight(int currentRow, int currentColumn)
        {
            return (currentRow - 1, currentColumn + 1);
        }

        static (int, int) MoveDownLeft(int currentRow, int currentColumn)
        {
            return (currentRow + 1, currentColumn - 1);
        }

        static (int, int) MoveDownRight(int currentRow, int currentColumn)
        {
            return (currentRow + 1, currentColumn + 1);
        }
    }
}
