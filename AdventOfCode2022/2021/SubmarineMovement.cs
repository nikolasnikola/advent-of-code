namespace AdventOfCode
{
    public class SubmarineMovement
    {
        public SubmarineMovement(params string[] items)
        {
            Direction = items[0];
            Units = int.Parse(items[1]);
        }

        public string Direction { get; set; }
        public int Units { get; set; }
    }
}
