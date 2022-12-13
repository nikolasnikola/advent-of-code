namespace AdventOfCode2022._2022
{
    internal static class Day2
    {
        internal static void Part2()
        {
            var games = Reader.ReadAsStringList("Day2");

            var rounds = games
                .Select(g => g.Split(' '))
                .Select(g => (Convert.ToChar(g[0]), Convert.ToChar(g[1])));

            var result = rounds
                .Select(r => GetRoundPointsV2(r.Item1, r.Item2))
                .Sum();

            Console.WriteLine(result);

        }

        internal static void Part1()
        {
            var games = Reader.ReadAsStringList("Day2");

            var rounds = games
                .Select(g => g.Split(' '))
                .Select(g => (Convert.ToChar(g[0]), Convert.ToChar(g[1])));

            var result = rounds
                .Select(r => GetRoundPoints(r.Item1, r.Item2))
                .Sum();

            Console.WriteLine(result);

        }

        static int GetRoundPointsV2(char oponnent, char result)
        {
            char me = GetMyTurn(oponnent, result);
            var myTurnPoints = me == 'X' ? 1 : me == 'Y' ? 2 : 3;

            return myTurnPoints + GetRoundOutcomePoints(oponnent, me);
        }

        static char GetMyTurn(char oponnent, char result) => oponnent switch
        {
            'A' => result == 'X' ? 'Z' : result == 'Y' ? 'X' : 'Y',
            'B' => result == 'X' ? 'X' : result == 'Y' ? 'Y' : 'Z',
            'C' => result == 'X' ? 'Y' : result == 'Y' ? 'Z' : 'X',
            _ => 'X',
        };

        static int GetRoundPoints(char oponnent, char me)
        {
            var myTurnPoints = me == 'X' ? 1 : me == 'Y' ? 2 : 3;

            return myTurnPoints + GetRoundOutcomePoints(oponnent, me);
        }

        static int GetRoundOutcomePoints(char oponnent, char me) => oponnent switch
        {
            'A' => me == 'X' ? 3 : me == 'Y' ? 6 : 0,
            'B' => me == 'Y' ? 3 : me == 'Z' ? 6 : 0,
            'C' => me == 'Z' ? 3 : me == 'X' ? 6 : 0,
            _ => 0,
        };
    }
}
