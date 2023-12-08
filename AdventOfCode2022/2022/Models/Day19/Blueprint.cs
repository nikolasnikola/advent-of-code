namespace AdventOfCode.Models.Day19
{
    public class Blueprint
    {
        public int Id { get; set; }
        public int OreRobotOreCost { get; set; }
        public int ClayRobotOreCost { get; set; }
        public int ObsidianRobotOreCost { get; set; }
        public int ObsidianRobotClayCost { get; set; }
        public int GeodeRobotOreCost { get; set; }
        public int GeodeRobotObisidianCost { get; set; }

        public int CurrentBest { get; set; }
        public int CurrentGeodeBest { get; set; }
        public int CurrentGeodeRobotBest { get; set; }

        public int GetMaxOres() => Math.Max(OreRobotOreCost, Math.Max(ClayRobotOreCost, Math.Max(ObsidianRobotOreCost, GeodeRobotOreCost)));
    }
}
