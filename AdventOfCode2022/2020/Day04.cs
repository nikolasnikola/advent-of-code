using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal static class Day04
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsString("2020", "Day04");
            var passports = input.Split("\r\n\r\n");
            var counter = 0;

            var requiredFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            

            foreach (var passport in passports)
            {
                var passportData = passport.Split(null).Where(s => !string.IsNullOrWhiteSpace(s));
                var passportDataValidator = requiredFields.ToDictionary(f => f, _ => 0);

                foreach (var pi in passportData)
                {
                    var splitted = pi.Split(':');
                    if (passportDataValidator.ContainsKey(splitted[0])) passportDataValidator[splitted[0]]++;
                }

                if (passportDataValidator.All(p => p.Value == 1))
                {
                    Console.WriteLine($"Passport VALID");
                    counter++;
                }
                else
                {
                    Console.WriteLine($"Passport NOT VALID");
                }
            }

            Console.WriteLine($"Total: " + counter);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsString("2020", "Day04");
            var passports = input.Split("\r\n\r\n");
            var counter = 0;
            var requiredFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            foreach (var passport in passports)
            {
                var passportData = passport.Split(null).Where(s => !string.IsNullOrWhiteSpace(s));
                var passportDataValidator = requiredFields.ToDictionary(f => f, _ => 0);
                var isValid = true;

                foreach (var pi in passportData)
                {
                    var splitted = pi.Split(':');
                    if (passportDataValidator.ContainsKey(splitted[0])) passportDataValidator[splitted[0]]++;
                    if (!DataValidator(splitted[0], splitted[1]))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid && passportDataValidator.All(p => p.Value == 1)) counter++;
            }

            Console.WriteLine($"Total: " + counter);
        }

        private static bool DataValidator(string key, string value)
        {
            if (key == "byr")
            {
                if (int.TryParse(value, out var year))
                {
                    return year >= 1920 && year <= 2002;
                }
                return false;
            }
            if (key == "iyr")
            {
                if (int.TryParse(value, out var year))
                {
                    return year >= 2010 && year <= 2020;
                }
                return false;
            }
            if (key == "eyr")
            {
                if (int.TryParse(value, out var year))
                {
                    return year >= 2020 && year <= 2030;
                }
                return false;
            }
            if (key == "hgt")
            {
                if (int.TryParse(value.Substring(0, value.Length - 2), out var height))
                {
                    if (value.EndsWith("cm")) return height >= 150 && height <= 193;
                    if (value.EndsWith("in")) return height >= 59 && height <= 76;
                }
                return false;
            }
            if (key == "hcl")
            {
                return Regex.IsMatch(value, @"^#[0-9a-f]{6}$");
            }
            if (key == "ecl")
            {
                return value == "amb" || value == "blu" || value == "brn" || value == "gry" || value == "grn" || value == "hzl" || value == "oth";
            }
            if (key == "pid")
            {
                return Regex.IsMatch(value, @"^[0-9]{9}$");
            }

            if (key == "cid") return true;

            return false;
        }
    }
}
