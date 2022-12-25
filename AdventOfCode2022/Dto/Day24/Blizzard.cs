namespace AdventOfCode2022.Dto.Day24
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Blizzard
    {
        public Blizzard(int id, int currentRow, int currentColumn, Direction direction)
        {
            Id = id;
            CurrentRow = currentRow;
            CurrentColumn = currentColumn;
            Direction = direction;
        }

        public int Id { get; set; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }
        public Direction Direction { get; set; }

        public void Move(int maxR, int maxC)
        {
            if (Direction == Direction.Down)
            {
                int newRow = CurrentRow + 1;
                CurrentRow = newRow > maxR ? 1 : newRow;
            }
            else if (Direction == Direction.Up)
            {
                int newRow = CurrentRow - 1;
                CurrentRow = newRow < 1 ? maxR : newRow;
            }
            else if (Direction == Direction.Right)
            {
                int newColumn = CurrentColumn + 1;
                CurrentColumn = newColumn > maxC ? 1 : newColumn;
            }
            else
            {
                int newColumn = CurrentColumn - 1;
                CurrentColumn = newColumn < 1 ? maxC : newColumn;
            }
        }
    }
}
