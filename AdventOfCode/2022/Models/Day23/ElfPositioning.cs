namespace AdventOfCode.Models.Day23
{
    public enum Direction
    {
        North,
        South,
        West,
        East
    }

    internal class ElfPositioning
    {
        public ElfPositioning(int elfId, int currentRow, int currentColumn) 
        {
            ElfId = elfId;
            CurrentRow = currentRow;
            CurrentColumn = currentColumn;
        }

        public int ElfId { get; set; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public bool CanMoveNorth(List<ElfPositioning> allPositions, (int r, int c) n, (int r, int c) nw, (int r, int c) ne)
        {
            return !allPositions.Any(p => p.CurrentRow == n.r && p.CurrentColumn == n.c) &&
                !allPositions.Any(p => p.CurrentRow == ne.r && p.CurrentColumn == ne.c) &&
                !allPositions.Any(p => p.CurrentRow == nw.r && p.CurrentColumn == nw.c);
        }

        public bool CanMoveSouth(List<ElfPositioning> allPositions, (int r, int c) s, (int r, int c) sw, (int r, int c) se)
        {
            return !allPositions.Any(p => p.CurrentRow == s.r && p.CurrentColumn == s.c) &&
                !allPositions.Any(p => p.CurrentRow == se.r && p.CurrentColumn == se.c) &&
                !allPositions.Any(p => p.CurrentRow == sw.r && p.CurrentColumn == sw.c);
        }

        public bool CanMoveEast(List<ElfPositioning> allPositions, (int r, int c) e, (int r, int c) ne, (int r, int c) se)
        {
            return !allPositions.Any(p => p.CurrentRow == e.r && p.CurrentColumn == e.c) &&
                !allPositions.Any(p => p.CurrentRow == ne.r && p.CurrentColumn == ne.c) &&
                !allPositions.Any(p => p.CurrentRow == se.r && p.CurrentColumn == se.c);
        }

        public bool CanMoveWest(List<ElfPositioning> allPositions, (int r, int c) w, (int r, int c) nw, (int r, int c) sw)
        {
            return !allPositions.Any(p => p.CurrentRow == w.r && p.CurrentColumn == w.c) &&
                !allPositions.Any(p => p.CurrentRow == nw.r && p.CurrentColumn == nw.c) &&
                !allPositions.Any(p => p.CurrentRow == sw.r && p.CurrentColumn == sw.c);
        }

        public (ElfPositioning, int newRow, int newColumn)? CanMoveSomewhere(List<ElfPositioning> allPositions, Direction? lastDirection)
        {
            (int r, int c) n = (CurrentRow - 1, CurrentColumn);
            (int r, int c) nw = (CurrentRow - 1, CurrentColumn - 1);
            (int r, int c) ne = (CurrentRow - 1, CurrentColumn + 1);
            (int r, int c) e = (CurrentRow, CurrentColumn + 1);
            (int r, int c) w = (CurrentRow, CurrentColumn - 1);
            (int r, int c) s = (CurrentRow + 1, CurrentColumn);
            (int r, int c) sw = (CurrentRow + 1, CurrentColumn - 1);
            (int r, int c) se = (CurrentRow + 1, CurrentColumn + 1);

            if (IsAlone(allPositions, n, nw, ne, e, w, s, sw, se)) return null;

            var directionToCheck = GetNextDirection(lastDirection);

            if (directionToCheck == Direction.North)
            {
                if (CanMoveNorth(allPositions, n, nw, ne)) return (this, n.r, n.c); 
                if (CanMoveSouth(allPositions, s, sw, se)) return (this, s.r, s.c);
                if (CanMoveWest(allPositions, w, nw, sw)) return (this, w.r, w.c);
                if (CanMoveEast(allPositions, e, ne, se)) return (this, e.r, e.c);

                return null;
            }

            else if (directionToCheck == Direction.South)
            {
                if (CanMoveSouth(allPositions, s, sw, se)) return (this, s.r, s.c);
                if (CanMoveWest(allPositions, w, nw, sw)) return (this, w.r, w.c);
                if (CanMoveEast(allPositions, e, ne, se)) return (this, e.r, e.c);
                if (CanMoveNorth(allPositions, n, nw, ne)) return (this, n.r, n.c);

                return null;
            }

            else if (directionToCheck == Direction.West)
            {
                if (CanMoveWest(allPositions, w, nw, sw)) return (this, w.r, w.c);
                if (CanMoveEast(allPositions, e, ne, se)) return (this, e.r, e.c);
                if (CanMoveNorth(allPositions, n, nw, ne)) return (this, n.r, n.c);
                if (CanMoveSouth(allPositions, s, sw, se)) return (this, s.r, s.c);

                return null;
            }

            else
            {
                if (CanMoveEast(allPositions, e, ne, se)) return (this, e.r, e.c);
                if (CanMoveNorth(allPositions, n, nw, ne)) return (this, n.r, n.c);
                if (CanMoveSouth(allPositions, s, sw, se)) return (this, s.r, s.c);
                if (CanMoveWest(allPositions, w, nw, sw)) return (this, w.r, w.c);

                return null;
            }
        }

        private bool IsAlone(
            List<ElfPositioning> allPositions,
            (int r, int c) n,
            (int r, int c) nw,
            (int r, int c) ne,
            (int r, int c) e,
            (int r, int c) w,
            (int r, int c) s,
            (int r, int c) sw,
            (int r, int c) se)
        {
            return !allPositions.Any(p => (p.CurrentRow == n.r && p.CurrentColumn == n.c) ||
                                (p.CurrentRow == nw.r && p.CurrentColumn == nw.c) ||
                                (p.CurrentRow == ne.r && p.CurrentColumn == ne.c) ||
                                (p.CurrentRow == e.r && p.CurrentColumn == e.c) ||
                                (p.CurrentRow == w.r && p.CurrentColumn == w.c) ||
                                (p.CurrentRow == s.r && p.CurrentColumn == s.c) ||
                                (p.CurrentRow == sw.r && p.CurrentColumn == sw.c) ||
                                (p.CurrentRow == se.r && p.CurrentColumn == se.c));
        }

        public void Move(int r, int c)
        {
            CurrentRow = r;
            CurrentColumn = c;
        }

        public void MoveNorth()
        {
            CurrentRow -= 1;
        }

        public void MoveSouth()
        {
            CurrentRow += 1;
        }

        public void MoveWest()
        {
            CurrentColumn -= 1;
        }

        public void MoveEast()
        {
            CurrentColumn += 1;
        }

        public static Direction GetNextDirection(Direction? lastDireciton) => lastDireciton switch
        {
            Direction.North => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.East,
            _ => Direction.North
        };
    }
}
