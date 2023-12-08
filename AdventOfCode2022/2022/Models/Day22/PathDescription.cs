namespace AdventOfCode.Models.Day22
{
    public class PathDescription
    {
        public PathDescription(char direction) 
        { 
            Direction= direction;
            IsDirectionChange = true;
        }

        public PathDescription(int steps)
        {
            Steps= steps;
            IsDirectionChange = false;
        }

        public char? Direction { get; set; }

        public int? Steps { get; set; }

        public bool IsDirectionChange { get; set; }
    }
}
