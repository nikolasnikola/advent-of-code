using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2020
{
    internal enum Sides { North, West, East, South }

    internal class PositionWP
    {
        public PositionWP() { ShipWestEast = 0; ShipNorthSouth = 0; WaypointWestEast = 10; WaypointNorthSouth = 1; }

        internal int ShipWestEast { get; set; }
        internal int ShipNorthSouth { get; set; }
        internal int WaypointWestEast { get; set; }
        internal int WaypointNorthSouth { get; set; }

        internal void ExecuteCommand(char command, int value)
        {
            if (command == 'L' || command == 'R')
            {
                RotateWaypoint(command, value);
            }
            else if (command == 'F')
            {
                MoveForward(value);
            }
            else MoveWaypointOnSide(command, value);
        }

        private void MoveForward(int value)
        {
            ShipNorthSouth += WaypointNorthSouth * value;
            ShipWestEast += WaypointWestEast * value;
        }

        private void MoveWaypointOnSide(char side, int value)
        {
            switch (side)
            {
                case 'W': WaypointWestEast -= value; break;
                case 'E': WaypointWestEast += value; break;
                case 'N': WaypointNorthSouth += value; break;
                default: WaypointNorthSouth -= value; break;
            };
        }

        private void RotateWaypoint(char direction, int value)
        {
            var currentNS = WaypointNorthSouth;
            var currentWE = WaypointWestEast;

            switch (value)
            {
                case 90:
                    WaypointWestEast = direction == 'R' ? currentNS : -currentNS;
                    WaypointNorthSouth = direction == 'R' ? -currentWE : currentWE;
                    break;
                case 180:
                    WaypointWestEast = -currentWE;
                    WaypointNorthSouth = -currentNS;
                    break;
                default:
                    WaypointWestEast = direction == 'R' ? -currentNS : currentNS;
                    WaypointNorthSouth = direction == 'R' ? currentWE : -currentWE;
                    break;
            };
        }
    }

    internal class Position
    {
        public Position() { WestEast = 0; NorthSouth = 0; Facing = Sides.East; }

        internal int WestEast { get; set; }
        internal int NorthSouth { get; set; }
        internal Sides Facing { get; set; }

        internal void ExecuteCommand(char command, int value)
        {
            if (command == 'L' || command == 'R')
            {
                Facing = GetNewSide(command, value);
            }
            else if (command == 'F')
            {
                MoveForward(value);
            }
            else MoveOnSide(command, value);
        }

        private void MoveForward(int value)
        {
            switch (Facing)
            {
                case Sides.West: WestEast -= value; break;
                case Sides.East: WestEast += value; break;
                case Sides.North: NorthSouth += value; break;
                default: NorthSouth -= value; break;
            };
        }

        private void MoveOnSide(char side, int value)
        {
            switch (side)
            {
                case 'W': WestEast -= value; break;
                case 'E': WestEast += value; break;
                case 'N': NorthSouth += value; break;
                default: NorthSouth -= value; break;
            };
        }

        private Sides GetNewSide(char direction, int value)
        {
            return value switch
            {
                90 => Facing switch
                {
                    Sides.North => direction == 'R' ? Sides.East : Sides.West,
                    Sides.East => direction == 'R' ? Sides.South : Sides.North,
                    Sides.South => direction == 'R' ? Sides.West : Sides.East,
                    _ => direction == 'R' ? Sides.North : Sides.South
                },
                180 => Facing switch
                {
                    Sides.North => Sides.South,
                    Sides.East => Sides.West,
                    Sides.South => Sides.North,
                    _ => Sides.East
                },
                _ => Facing switch
                {
                    Sides.North => direction == 'R' ? Sides.West : Sides.East,
                    Sides.East => direction == 'R' ? Sides.North : Sides.South,
                    Sides.South => direction == 'R' ? Sides.East : Sides.West,
                    _ => direction == 'R' ? Sides.South : Sides.North
                },
            };
        }
    }

    internal static class Day12
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2020", "Day12");
            var commands = GetCommands(input);

            var position = new Position();

            foreach (var command in commands)
            {
                position.ExecuteCommand(command.Item1, command.Item2);
            }

            Console.WriteLine(Math.Abs(position.WestEast) + Math.Abs(position.NorthSouth));
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsStringList("2020", "Day12");
            var commands = GetCommands(input);

            var position = new PositionWP();

            foreach (var command in commands)
            {
                position.ExecuteCommand(command.Item1, command.Item2);
            }

            Console.WriteLine(Math.Abs(position.ShipWestEast) + Math.Abs(position.ShipNorthSouth));
        }

        static (char, int)[] GetCommands(string[] input)
        {
            return input.Select(x => (x.First(), int.Parse(x[1..]))).ToArray();
        }
    }
}
