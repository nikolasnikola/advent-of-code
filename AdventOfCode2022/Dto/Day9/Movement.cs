namespace AdventOfCode2022.Dto.Day9
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Movement
    {
        public Movement(params string[] items)
        {
            Direction = GetDirection(items[0]);
            Steps = int.Parse(items[1]);
        }

        public Direction Direction { get; set; }

        public int Steps { get; set; }

        private Direction GetDirection(string input) => input switch
        {
            "U" => Direction.Up,
            "D" => Direction.Down,
            "L" => Direction.Left,
            _ => Direction.Right,
        };
    }
}
