namespace AdventOfCode2022.Dto.Day22
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }

    public enum Side
    {
        Top,
        Bottom,
        Front,
        Back,
        Left,
        Right,
    }

    public class Map
    {
        public Map(char[,] graph)
        {
            Graph = graph;

            var startPosition = GetStartPosition();

            CurrentRow = startPosition.Item1;
            CurrentColumn = startPosition.Item2;
            CurrentDirection = Direction.Right;
            CurrentSide = Side.Top;

            TopSideFirstRow = 0;
            TopSideFirstColumn = 50;
            RightSideFirstRow = 0;
            RightSideFirstColumn = 100;
            FrontSideFirstRow = 50;
            FrontSideFirstColumn = 50;
            LeftSideFirstRow = 100;
            LeftSideFirstColumn = 0;
            BottomSideFirstRow = 100;
            BottomSideFirstColumn = 50;
            BackSideFirstRow = 150;
            BackSideFirstColumn = 0;
        }

        public char[,] Graph { get; private set; }

        public int CurrentRow { get; set; }

        public int CurrentColumn { get; set; }

        public Direction CurrentDirection { get; set; }

        public Side CurrentSide { get; set; }

        private int TopSideFirstRow { get; set; }
        private int TopSideFirstColumn { get; set; }
        private int LeftSideFirstRow { get; set; }
        private int LeftSideFirstColumn { get; set; }
        private int RightSideFirstRow { get; set; }
        private int RightSideFirstColumn { get; set; }
        private int BottomSideFirstRow { get; set; }
        private int BottomSideFirstColumn { get; set; }
        private int FrontSideFirstRow { get; set; }
        private int FrontSideFirstColumn { get; set; }
        private int BackSideFirstRow { get; set; }
        private int BackSideFirstColumn { get; set; }

        private int TopSideLastRow => TopSideFirstRow + 49;
        private int TopSideLastColumn => TopSideFirstColumn + 49;
        private int LeftSideLastRow => LeftSideFirstRow + 49;
        private int LeftSideLastColumn => LeftSideFirstColumn + 49;
        private int RightSideLastRow => RightSideFirstRow + 49;
        private int RightSideLastColumn => RightSideFirstColumn + 49;
        private int BottomSideLastRow => BottomSideFirstRow + 49;
        private int BottomSideLastColumn => BottomSideFirstColumn + 49;
        private int FrontSideLastRow => FrontSideFirstRow + 49;
        private int FrontSideLastColumn => FrontSideFirstColumn + 49;
        private int BackSideLastRow => BackSideFirstRow + 49;
        private int BackSideLastColumn => BackSideFirstColumn + 49;


        public int GetScore()
        {
            return (CurrentRow + 1) * 1000 + (CurrentColumn + 1) * 4 + GetDirectionScore();
        }

        public bool GoForwardIfPossible()
        {
            var nextPosition = GetNextPosition();

            if (nextPosition == null) return false;

            CurrentRow = nextPosition.Value.Item1;
            CurrentColumn = nextPosition.Value.Item2;

            return true;
        }

        public bool GoForwardWithSidesIfPossible()
        {
            var nextPosition = GetNextPositionWidhSides();

            if (nextPosition == null) return false;

            CurrentRow = nextPosition.Value.Item1;
            CurrentColumn = nextPosition.Value.Item2;
            CurrentDirection = nextPosition.Value.Item3;
            CurrentSide = nextPosition.Value.Item4;

            return true;
        }

        private int GetDirectionScore() => CurrentDirection == Direction.Right ? 0
                    : CurrentDirection == Direction.Down ? 1
                    : CurrentDirection == Direction.Left ? 2
                    : 3;

        private (int, int) GetStartPosition()
        {
            for (int i = 0; i < Graph.GetLength(0); i++)
            {
                for (int j = 0; j < Graph.GetLength(1); j++)
                {
                    if (Graph[i, j] == '.')
                    {
                        return (i, j);
                    }
                }
            }

            return (0, 0);
        }

        private (int, int)? GetNextPosition()
        {
            if (CurrentDirection == Direction.Left)
            {
                var newColumn = CurrentColumn - 1;
                if (newColumn < 0 || Graph[CurrentRow, newColumn] == '\0') newColumn = Array.FindLastIndex(MatrixHelper.GetRow(Graph, CurrentRow).ToArray(), c => c != '\0');

                if (Graph[CurrentRow, newColumn] == '.') { return (CurrentRow, newColumn); }

                return null;
            }
            else if (CurrentDirection == Direction.Right)
            {
                var newColumn = CurrentColumn + 1;
                if (newColumn >= Graph.GetLength(1) || Graph[CurrentRow, newColumn] == '\0') newColumn = Array.FindIndex(MatrixHelper.GetRow(Graph, CurrentRow), c => c != '\0');

                if (Graph[CurrentRow, newColumn] == '.') { return (CurrentRow, newColumn); }

                return null;
            }
            else if (CurrentDirection == Direction.Up)
            {
                var newRow = CurrentRow - 1;
                if (newRow < 0 || Graph[newRow, CurrentColumn] == '\0') newRow = Array.FindLastIndex(MatrixHelper.GetColumn(Graph, CurrentColumn).ToArray(), c => c != '\0');

                if (Graph[newRow, CurrentColumn] == '.') { return (newRow, CurrentColumn); }

                return null;
            }
            else
            {
                var newRow = CurrentRow + 1;
                if (newRow >= Graph.GetLength(0) || Graph[newRow, CurrentColumn] == '\0') newRow = Array.FindIndex(MatrixHelper.GetColumn(Graph, CurrentColumn), c => c != '\0');

                if (Graph[newRow, CurrentColumn] == '.') { return (newRow, CurrentColumn); }

                return null;
            }
        }

        private (int, int, Direction, Side)? GetNextPositionWidhSides()
        {
            var (minR, maxR, minC, maxC) = GetCurrentSideLimitations();

            if (CurrentDirection == Direction.Left)
            {
                var newColumn = CurrentColumn - 1;
                var newRow = CurrentRow;
                var newDirection = CurrentDirection;
                var newSide = CurrentSide;

                if (newColumn < minC)
                {
                    if (CurrentSide == Side.Top)
                    {
                        newRow = LeftSideLastRow - CurrentRow + minR;
                        newColumn = LeftSideFirstColumn;
                        newDirection = Direction.Right;
                        newSide = Side.Left;
                    }
                    else if (CurrentSide == Side.Back)
                    {
                        newRow = TopSideFirstRow;
                        newColumn = TopSideFirstColumn + CurrentRow - minR;
                        newDirection = Direction.Down;
                        newSide = Side.Top;
                    }
                    else if (CurrentSide == Side.Front)
                    {
                        newRow = LeftSideFirstRow;
                        newColumn = LeftSideFirstColumn + CurrentRow - minR;
                        newDirection = Direction.Down;
                        newSide = Side.Left;
                    }
                    else if (CurrentSide == Side.Right)
                    {
                        newRow = CurrentRow;
                        newColumn = TopSideLastColumn;
                        newDirection = Direction.Left;
                        newSide = Side.Top;
                    }
                    else if (CurrentSide == Side.Bottom)
                    {
                        newRow = CurrentRow;
                        newColumn = LeftSideLastColumn;
                        newDirection = Direction.Left;
                        newSide = Side.Left;
                    }
                    else
                    {
                        newRow = maxR - CurrentRow + TopSideFirstRow;
                        newColumn = TopSideFirstColumn;
                        newDirection = Direction.Right;
                        newSide = Side.Top;
                    }
                }

                if (Graph[newRow, newColumn] == '.') { return (newRow, newColumn, newDirection, newSide); }

                return null;
            }
            else if (CurrentDirection == Direction.Right)
            {
                var newColumn = CurrentColumn + 1;
                var newRow = CurrentRow;
                var newDirection = CurrentDirection;
                var newSide = CurrentSide;

                if (newColumn > maxC)
                {
                    if (CurrentSide == Side.Top)
                    {
                        newRow = CurrentRow;
                        newColumn = RightSideFirstColumn;
                        newDirection = Direction.Right;
                        newSide = Side.Right;
                    }
                    else if (CurrentSide == Side.Back)
                    {
                        newRow = BottomSideLastRow;
                        newColumn = CurrentRow - minR + BottomSideFirstColumn;
                        newDirection = Direction.Up;
                        newSide = Side.Bottom;
                    }
                    else if (CurrentSide == Side.Front)
                    {
                        newRow = RightSideLastRow;
                        newColumn = CurrentRow - minR + RightSideFirstColumn;
                        newDirection = Direction.Up;
                        newSide = Side.Right;
                    }
                    else if (CurrentSide == Side.Right)
                    {
                        newRow = BottomSideLastRow - CurrentRow + minR;
                        newColumn = BottomSideLastColumn;
                        newDirection = Direction.Left;
                        newSide = Side.Bottom;
                    }
                    else if (CurrentSide == Side.Bottom)
                    {
                        newRow = maxR - CurrentRow + RightSideFirstRow;
                        newColumn = RightSideLastColumn;
                        newDirection = Direction.Left;
                        newSide = Side.Right;
                    }
                    else //left
                    {
                        newRow = CurrentRow;
                        newColumn = BottomSideFirstColumn;
                        newDirection = Direction.Right;
                        newSide = Side.Bottom;
                    }
                }

                if (Graph[newRow, newColumn] == '.') { return (newRow, newColumn, newDirection, newSide); }

                return null;
            }
            else if (CurrentDirection == Direction.Up)
            {
                var newRow = CurrentRow - 1;
                var newColumn = CurrentColumn;
                var newDirection = CurrentDirection;
                var newSide = CurrentSide;

                if (newRow < minR)
                {
                    if (CurrentSide == Side.Top)
                    {
                        newRow = CurrentColumn + BackSideLastRow - maxC;
                        newColumn = BackSideFirstColumn;
                        newDirection = Direction.Right;
                        newSide = Side.Back;
                    }
                    else if (CurrentSide == Side.Back)
                    {
                        newRow = LeftSideLastRow;
                        newColumn = CurrentColumn;
                        newDirection = Direction.Up;
                        newSide = Side.Left;
                    }
                    else if (CurrentSide == Side.Front)
                    {
                        newRow = TopSideLastRow;
                        newColumn = CurrentColumn;
                        newDirection = Direction.Up;
                        newSide = Side.Top;
                    }
                    else if (CurrentSide == Side.Right)
                    {
                        newRow = BackSideLastRow;
                        newColumn = CurrentColumn - maxC + BackSideLastColumn;
                        newDirection = Direction.Up;
                        newSide = Side.Back;
                    }
                    else if (CurrentSide == Side.Bottom)
                    {
                        newRow = FrontSideLastRow;
                        newColumn = CurrentColumn;
                        newDirection = Direction.Up;
                        newSide = Side.Front;
                    }
                    else //left
                    {
                        newRow = CurrentColumn + FrontSideLastRow - maxC;
                        newColumn = FrontSideFirstColumn;
                        newDirection = Direction.Right;
                        newSide = Side.Front;
                    }
                }

                if (Graph[newRow, newColumn] == '.') { return (newRow, newColumn, newDirection, newSide); }

                return null;
            }
            else //down
            {
                var newRow = CurrentRow + 1;
                var newColumn = CurrentColumn;
                var newDirection = CurrentDirection;
                var newSide = CurrentSide;

                if (newRow > maxR)
                {
                    if (CurrentSide == Side.Top)
                    {
                        newRow = FrontSideFirstRow;
                        newColumn = CurrentColumn;
                        newDirection = Direction.Down;
                        newSide = Side.Front;
                    }
                    else if (CurrentSide == Side.Back)
                    {
                        newRow = RightSideFirstRow;
                        newColumn = CurrentColumn + RightSideLastColumn - maxC;
                        newDirection = Direction.Down;
                        newSide = Side.Right;
                    }
                    else if (CurrentSide == Side.Front)
                    {
                        newRow = BottomSideFirstRow;
                        newColumn = CurrentColumn;
                        newDirection = Direction.Down;
                        newSide = Side.Bottom;
                    }
                    else if (CurrentSide == Side.Right)
                    {
                        newRow = CurrentColumn - maxC + FrontSideLastRow;
                        newColumn = FrontSideLastColumn;
                        newDirection = Direction.Left;
                        newSide = Side.Front;
                    }
                    else if (CurrentSide == Side.Bottom)
                    {
                        newRow = CurrentColumn + BackSideLastRow - maxC;
                        newColumn = BackSideLastColumn;
                        newDirection = Direction.Left;
                        newSide = Side.Back;
                    }
                    else //left
                    {
                        newRow = BackSideFirstRow;
                        newColumn = CurrentColumn;
                        newDirection = Direction.Down;
                        newSide = Side.Back;
                    }
                }

                if (Graph[newRow, newColumn] == '.') { return (newRow, newColumn, newDirection, newSide); }

                return null;
            }
        }

        private (int minR, int maxR, int minC, int maxC) GetCurrentSideLimitations() => CurrentSide switch
        {
            Side.Top => (TopSideFirstRow, TopSideLastRow, TopSideFirstColumn, TopSideLastColumn),
            Side.Bottom => (BottomSideFirstRow, BottomSideLastRow, BottomSideFirstColumn, BottomSideLastColumn),
            Side.Back => (BackSideFirstRow, BackSideLastRow, BackSideFirstColumn, BackSideLastColumn),
            Side.Right => (RightSideFirstRow, RightSideLastRow, RightSideFirstColumn, RightSideLastColumn),
            Side.Front => (FrontSideFirstRow, FrontSideLastRow, FrontSideFirstColumn, FrontSideLastColumn),
            _ => (LeftSideFirstRow, LeftSideLastRow, LeftSideFirstColumn, LeftSideLastColumn)
        };

        public void ChangeDirection(char dir)
        {
            if (dir == 'R')
            {
                CurrentDirection = CurrentDirection == Direction.Left
                    ? Direction.Up
                    : CurrentDirection == Direction.Up
                    ? Direction.Right
                    : CurrentDirection == Direction.Right
                    ? Direction.Down
                    : Direction.Left;
            }
            else
            {
                CurrentDirection = CurrentDirection == Direction.Left
                    ? Direction.Down
                    : CurrentDirection == Direction.Down
                    ? Direction.Right
                    : CurrentDirection == Direction.Right
                    ? Direction.Up
                    : Direction.Left;
            }
        }
    }
}
