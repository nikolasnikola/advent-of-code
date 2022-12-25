using System.Data;
using AdventOfCode2022.Dto.Day24;

namespace AdventOfCode2022._2022
{
    public class State
    {
        public State() { }
        public State((int, int) newPosition, State oldState, string process)
        {
            foreach (var item in oldState.History)
            {
                History.Add(item);
            }

            History.Add(newPosition);

            foreach (var item in oldState.Process)
            {
                Process.Add(item);
            }

            Process.Add(process);
        }

        public List<string> Process = new();
        public List<(int, int)> History = new();
        public int CurrentPositionWaits = 0;

        public State WaitState()
        {
            var ns = new State();
            ns.History = new();
            foreach (var item in History)
            {
                ns.History!.Add(item);
            }

            foreach (var item in Process)
            {
                ns.Process.Add(item);
            }

            ns.CurrentPositionWaits = CurrentPositionWaits + 1;
            ns.Process.Add("Wait");

            return ns;
        }
    }

    internal static class Day24
    {
        private static (int r, int c) StartingPosition = (0, 0);
        private static (int r, int c) EndPosition = (0, 0);
        private static bool EndReached = false;

        internal static void Part2()
        {
            var map = Reader.ReadAsCharMatrix("Day24");
            var maxR = map.GetLength(0) - 2;
            var maxC = map.GetLength(1) - 2;

            var blizzards = GetInitialPositioning(map);

            var oldStartPoint = StartingPosition;
            var oldEndPoint = EndPosition;

            HashSet<State> states = new()
            {
                new State(StartingPosition, new State(), "Initial")
            };

            var currentMinuteTotal = 0;
            var currentMinutePt1 = 0;
            var currentMinutePt2 = 0;
            var currentMinutePt3 = 0;

            while (!EndReached)
            {
                currentMinuteTotal++;
                currentMinutePt1++;

                Console.Write($"\nCalculating minute {currentMinuteTotal}... 00%");
                states = ProcessMinute(blizzards, states, new HashSet<State>(), maxR, maxC);

                states = states.DistinctBy(s => s.History.Last()).ToHashSet();

                Console.Write($" => {states.Count} states total");
            }

            Console.WriteLine("\n\n************** Reached FROM START TO END");

            HashSet<State> states2 = new()
            {
                new State(oldEndPoint, new State(), "Initial")
            };

            
            StartingPosition = oldEndPoint;
            EndPosition = oldStartPoint;
            EndReached = false;

            while (!EndReached)
            {
                currentMinuteTotal++;
                currentMinutePt2++;

                Console.Write($"\nCalculating minute {currentMinuteTotal}... 00%");
                states2 = ProcessMinute(blizzards, states2, new HashSet<State>(), maxR, maxC);

                states2 = states2.DistinctBy(s => s.History.Last()).ToHashSet();

                Console.Write($" => {states2.Count} states total");
            }

            Console.WriteLine("\n\n************** Reached FROM END TO START");

            HashSet<State> states3 = new()
            {
                new State(oldStartPoint, new State(), "Initial")
            };

            StartingPosition = oldStartPoint;
            EndPosition = oldEndPoint;
            EndReached = false;

            while (!EndReached)
            {
                currentMinuteTotal++;
                currentMinutePt3++;

                Console.Write($"\nCalculating minute {currentMinuteTotal}... 00%");
                states3 = ProcessMinute(blizzards, states3, new HashSet<State>(), maxR, maxC);

                states3 = states3.DistinctBy(s => s.History.Last()).ToHashSet();

                Console.Write($" => {states3.Count} states total");
            }

            Console.WriteLine("\n\n**************");
            Console.WriteLine($"Less minutes PT1: {currentMinutePt1}; Less minutes PT2: {currentMinutePt2}; Less minutes PT3: {currentMinutePt3}. Total: {currentMinuteTotal}");
            Console.WriteLine("**************");
        }

        internal static void Part1()
        {
            var map = Reader.ReadAsCharMatrix("Day24");
            var maxR = map.GetLength(0) - 2;
            var maxC = map.GetLength(1) - 2;

            var blizzards = GetInitialPositioning(map);

            HashSet<State> states = new()
            {
                new State(StartingPosition, new State(), "Initial")
            };

            var currentMinute = 0;

            while (!EndReached)
            {
                currentMinute++;

                Console.Write($"\nCalculating minute {currentMinute}... 00%");
                states = ProcessMinute(blizzards, states, new HashSet<State>(), maxR, maxC);

                states = states.DistinctBy(s => s.History.Last()).ToHashSet();

                Console.Write($" => {states.Count} states total");
            }

            Console.WriteLine("\n\n**************");
            Console.WriteLine($"Less minutes: {currentMinute}");
            Console.WriteLine("**************");
        }

        static HashSet<State> ProcessMinute(List<Blizzard> blizzards, HashSet<State> states, HashSet<State> nextStates, int maxR, int maxC)
        {
            foreach (var blizzard in blizzards)
            {
                blizzard.Move(maxR, maxC);
            }

            foreach (var (state, i) in states.Select((s, i) => (s, i)))
            {
                Console.CursorLeft -= 3;
                Console.CursorVisible = false;
                Console.Write($"{(100 * (i + 1) / states.Count).ToString("D2")}%");

                (int r, int c) lastPosition = state.History.Last();

                if (!blizzards.Any(b => b.CurrentRow == lastPosition.r && b.CurrentColumn == lastPosition.c) /*&& state.CurrentPositionWaits <= 5*/)
                {
                    nextStates.Add(state.WaitState());
                }

                var canMoveDown = CanMoveDown(lastPosition, blizzards, maxR);

                if (canMoveDown.Item1)
                {
                    nextStates.Add(new State((lastPosition.r + 1, lastPosition.c), state, "Down"));

                    if (canMoveDown.Item2)
                    {
                        EndReached = true;
                        break;
                    }
                }

                var canMoveUp = CanMoveUp(lastPosition, blizzards);
                if (canMoveUp.Item1)
                {
                    nextStates.Add(new State((lastPosition.r - 1, lastPosition.c), state, "Up"));

                    if (canMoveUp.Item2)
                    {
                        EndReached = true;
                        break;
                    }
                }

                if (CanMoveLeft(lastPosition, blizzards, maxR)) nextStates.Add(new State((lastPosition.r, lastPosition.c - 1), state, "Left"));
                if (CanMoveRight(lastPosition, blizzards, maxR, maxC)) nextStates.Add(new State((lastPosition.r, lastPosition.c + 1), state, "Right"));
                //if (CanMoveUp(lastPosition, blizzards)) nextStates.Add(new State((lastPosition.r - 1, lastPosition.c), state, "Up"));

            }

            return nextStates;
        }

        static (bool, bool) CanMoveDown((int r, int c) position, List<Blizzard> blizzards, int maxR)
        {
            var newR = position.r + 1;

            if ((newR, position.c) == EndPosition) return (true, true);

            if ((newR, position.c) == StartingPosition) return (true, false);

            if (newR > maxR) return (false, false);

            if (blizzards.Any(b => b.CurrentRow == newR && b.CurrentColumn == position.c)) return (false, false);

            return (true, false);
        }

        static (bool, bool) CanMoveUp((int r, int c) position, List<Blizzard> blizzards)
        {
            var newR = position.r - 1;

            if ((newR, position.c) == EndPosition) return (true, true);

            if ((newR, position.c) == StartingPosition) return (true, false);

            if (newR < 1) return (false, false);

            if (blizzards.Any(b => b.CurrentRow == newR && b.CurrentColumn == position.c)) return (false, false);

            return (true, false);
        }

        static bool CanMoveRight((int r, int c) position, List<Blizzard> blizzards, int maxR, int maxC)
        {
            var newC = position.c + 1;

            if (position.r == 0) return false;
            if (position.r == maxR+1) return false;
            if (newC > maxC) return false;

            if (blizzards.Any(b => b.CurrentRow == position.r && b.CurrentColumn == newC)) return false;

            return true;
        }

        static bool CanMoveLeft((int r, int c) position, List<Blizzard> blizzards, int maxR)
        {
            var newC = position.c - 1;

            if (position.r == 0) return false;
            if (position.r == maxR + 1) return false;
            if (newC < 1) return false;

            if (blizzards.Any(b => b.CurrentRow == position.r && b.CurrentColumn == newC)) return false;

            return true;
        }

        static List<Blizzard> GetInitialPositioning(char[,] map)
        {
            var blizzards = new List<Blizzard>();

            var lastBlizzardId = 0;

            var rows = map.GetLength(0);
            var cols = map.GetLength(1);

            for (int i = 0; i < rows; i++)
            {

                for (int j = 0; j < cols; j++)
                {
                    if (i == 0 && map[i, j] == '.')
                    {
                        StartingPosition = (i, j);
                        continue;
                    }

                    if (i == rows - 1 && map[i, j] == '.')
                    {
                        EndPosition = (i, j);
                        continue;
                    }

                    if (map[i, j] == '>')
                    {
                        blizzards.Add(new Blizzard(++lastBlizzardId, i, j, Direction.Right));
                    }
                    if (map[i, j] == '^')
                    {
                        blizzards.Add(new Blizzard(++lastBlizzardId, i, j, Direction.Up));
                    }
                    if (map[i, j] == '<')
                    {
                        blizzards.Add(new Blizzard(++lastBlizzardId, i, j, Direction.Left));
                    }
                    if (map[i, j] == 'v')
                    {
                        blizzards.Add(new Blizzard(++lastBlizzardId, i, j, Direction.Down));
                    }
                }
            }
            return blizzards;
        }
    }
}
