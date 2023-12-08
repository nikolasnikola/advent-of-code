using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Models.Day19;

namespace AdventOfCode._2020
{
    internal static class Day02
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsStringList("2020", "Day02");
           
            var passwords = GetPasswordAndPolicies(input);

            Console.WriteLine(passwords.Count(p => p.IsValid()));
        }

        internal static void Part2()
        {

            var input = Reader.ReadAsStringList("2020", "Day02");

            var passwords = GetPasswordAndPolicies(input);

            Console.WriteLine(passwords.Count(p => p.IsValid2()));
        }

        static List<PasswordPolicy> GetPasswordAndPolicies(string[] input)
        {
            return input.Select(p =>
                            Regex.Match(p, @"(\d+)-(\d+) (\S+): (\S+)"))
                .Select(match =>
                new PasswordPolicy()
                {
                    Min = int.Parse(match.Groups[1].Value),
                    Max = int.Parse(match.Groups[2].Value),
                    Letter = char.Parse(match.Groups[3].Value),
                    Password = match.Groups[4].Value,
                })
                .ToList();
        }
    }

    internal class PasswordPolicy
    {
        internal int Min { get; set; }
        internal int Max { get; set; }
        internal char Letter { get; set; }
        internal string Password { get; set; }

        internal bool IsValid()
        {
            var numberOfLetters = Password.Where(c => c == Letter).Count();
            return numberOfLetters >= Min && numberOfLetters <= Max;
        }

        internal bool IsValid2()
        {
            bool position1valid = Password[Min-1] == Letter;
            bool position2valid = Password[Max-1] == Letter;

            return position1valid ^ position2valid;
        }
    }
}
