namespace AdventOfCode.Models.Day17
{
    public enum RockType
    {
        Minus,
        Plus,
        L,
        I,
        Square
    }

    public class Rock
    {
        public Rock(RockType rockType)
        {
            switch (rockType)
            {
                case RockType.Minus:
                    ConstructMinus();
                    break;
                case RockType.Plus:
                    ConstructPlus();
                    break;
                case RockType.L:
                    ConstructL();
                    break;
                case RockType.I:
                    ConstructI();
                    break;
                default:
                    ConstructSquare();
                    break;
            }
        }

        public RockType Shape { get; set; }
        public int ReferencePointRow { get; set; }

        public int ReferencePointColumn { get; set; }

        //public int StartPositionReferencePointLeft { get; set; }

        //public int StartPositionReferencePointBottom { get; set; }

        //public IList<int> ShapeReferencePointSameRow { get; private set; }
        //public IList<int> ShapeReferencePointSameColumn { get; private set; }

        public IList<(int, int)> ShapeOtherPointsFromReference { get; private set; }

        public int TotalHeight { get; set; }

        public IList<PointToCheck> PointsToCheckToMoveLeft { get; set; }

        public IList<PointToCheck> PointsToCheckToMoveRight { get; set; }

        public IList<PointToCheck> PointsToCheckToMoveDown { get; set; }

        public void MoveLeft()
        {
            ReferencePointColumn -= 1;
        }

        public void MoveRight()
        {
            ReferencePointColumn += 1;
        }

        public void MoveDown()
        {
            ReferencePointRow += 1;
        }

        public void Rest(char[,] map)
        {
            var symbol = GetSymbol;
            List<(int r, int c)> otherPoints = GetOtherPoints();

            map[ReferencePointRow, ReferencePointColumn] = symbol;

            foreach (var (r, c) in otherPoints)
            {
                map[r, c] = symbol;
            }
        }

        public bool CanMoveLeft(char[,] map)
        {
            foreach (var pt in PointsToCheckToMoveLeft)
            {
                var currentRown = ReferencePointRow + pt.RowAwayFromReference;
                var currentColumn = ReferencePointColumn + pt.ColumnAwayFromReference;

                if (map[currentRown, currentColumn - 1] != '.')
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanMoveRight(char[,] map)
        {
            foreach (var pt in PointsToCheckToMoveRight)
            {
                var currentRown = ReferencePointRow + pt.RowAwayFromReference;
                var currentColumn = ReferencePointColumn + pt.ColumnAwayFromReference;

                if (map[currentRown, currentColumn + 1] != '.')
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanMoveDown(char[,] map)
        {
            foreach (var pt in PointsToCheckToMoveDown)
            {
                var currentRow = ReferencePointRow + pt.RowAwayFromReference;
                var currentColumn = ReferencePointColumn + pt.ColumnAwayFromReference;

                if (map[currentRow + 1, currentColumn] != '.')
                {
                    return false;
                }
            }

            return true;
        }

        public List<(int r, int c)> GetOtherPoints()
        {
            List<(int r, int c)> otherPoints = new();

            foreach (var (r, c) in ShapeOtherPointsFromReference)
            {
                otherPoints.Add((ReferencePointRow + r, ReferencePointColumn + c));
            }

            return otherPoints;
        }

        public int GetTopPosition(int totalRows)
        {
            return totalRows - 1 - ReferencePointRow + Math.Abs(ShapeOtherPointsFromReference.Select(s => s.Item1).Min());
        }

        public void SetStartPosition(int currentHeight, int totalRows)
        {
            switch (Shape)
            {
                case RockType.Minus:
                    ReferencePointRow = totalRows - 1 - 4 - currentHeight;
                    ReferencePointColumn = 3;
                    break;
                case RockType.Plus:
                    ReferencePointRow = totalRows - 1 - 5 - currentHeight;
                    ReferencePointColumn = 4;
                    break;
                case RockType.L:
                    ReferencePointRow = totalRows - 1 - 4 - currentHeight;
                    ReferencePointColumn = 5;
                    break;
                case RockType.I:
                    ReferencePointRow = totalRows - 1 - 4 - currentHeight;
                    ReferencePointColumn = 3;
                    break;
                default:
                    ReferencePointRow = totalRows - 1 - 4 - currentHeight;
                    ReferencePointColumn = 3;
                    break;
            }
        }

        private void ConstructMinus()
        {
            TotalHeight = 1;
            Shape = RockType.Minus;

            ShapeOtherPointsFromReference = new List<(int, int)> { (0, 1), (0, 2), (0, 3) };

            PointsToCheckToMoveDown = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
                new PointToCheck(0, 1),
                new PointToCheck(0, 2),
                new PointToCheck(0, 3),
            };

            PointsToCheckToMoveLeft = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
            };

            PointsToCheckToMoveRight = new List<PointToCheck>
            {
                new PointToCheck(0, 3),
            };
        }

        private void ConstructPlus()
        {

            TotalHeight = 3;
            Shape = RockType.Plus;

            ShapeOtherPointsFromReference = new List<(int, int)> { (0, -1), (0, 1), (1, 0), (-1, 0) };

            PointsToCheckToMoveDown = new List<PointToCheck>
            {
                new PointToCheck(1, 0),
                new PointToCheck(0, -1),
                new PointToCheck(0, 1),
            };

            PointsToCheckToMoveLeft = new List<PointToCheck>
            {
                new PointToCheck(0, -1),
                new PointToCheck(-1, 0),
                new PointToCheck(1, 0),
            };

            PointsToCheckToMoveRight = new List<PointToCheck>
            {
                new PointToCheck(0, 1),
                new PointToCheck(-1, 0),
                new PointToCheck(1, 0),
            };
        }

        private void ConstructL()
        {
            TotalHeight = 3;
            Shape = RockType.L;

            ShapeOtherPointsFromReference = new List<(int, int)> { (0, -1), (0, -2), (-1, 0), (-2, 0) };

            PointsToCheckToMoveDown = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
                new PointToCheck(0, -1),
                new PointToCheck(0, -2),
            };

            PointsToCheckToMoveLeft = new List<PointToCheck>
            {
                new PointToCheck(-1, 0),
                new PointToCheck(-2, 0),
                new PointToCheck(0, -2),
            };

            PointsToCheckToMoveRight = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
                new PointToCheck(-1, 0),
                new PointToCheck(-2, 0),
            };
        }

        private void ConstructI()
        {

            TotalHeight = 4;
            Shape = RockType.I;

            ShapeOtherPointsFromReference = new List<(int, int)> { (-1, 0), (-2, 0), (-3, 0) };

            PointsToCheckToMoveDown = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
            };

            PointsToCheckToMoveLeft = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
                new PointToCheck(-1, 0),
                new PointToCheck(-2, 0),
                new PointToCheck(-3, 0),
            };

            PointsToCheckToMoveRight = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
                new PointToCheck(-1, 0),
                new PointToCheck(-2, 0),
                new PointToCheck(-3, 0),
            };
        }

        private void ConstructSquare()
        {
            TotalHeight = 2;
            Shape = RockType.Square;

            ShapeOtherPointsFromReference = new List<(int, int)> { (0, 1), (-1, 0), (-1, 1) };

            PointsToCheckToMoveDown = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
                new PointToCheck(0, 1),
            };

            PointsToCheckToMoveLeft = new List<PointToCheck>
            {
                new PointToCheck(0, 0),
                new PointToCheck(-1, 0),
            };

            PointsToCheckToMoveRight = new List<PointToCheck>
            {
                new PointToCheck(0, 1),
                new PointToCheck(-1, 1),
            };
        }

        private char GetSymbol => Shape switch
        {
            RockType.Minus => '=',
            RockType.Plus => '+',
            RockType.L => '@',
            RockType.I => 'M',
            _ => '#'
        };
    }

    public class PointToCheck
    {
        public PointToCheck(int rowDiff, int columnDiff)
        {
            RowAwayFromReference = rowDiff;
            ColumnAwayFromReference = columnDiff;
        }

        public int RowAwayFromReference { get; set; }
        public int ColumnAwayFromReference { get; set; }
    }
}
