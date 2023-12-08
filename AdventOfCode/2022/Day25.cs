namespace AdventOfCode._2022
{
    internal static class Day25
    {
        internal static void Part1()
        {
            var snafuNumbers = Reader.ReadAsStringList("2022", "Day25");

            var digitalNumbers = snafuNumbers.Select(sn => SnafuToDecimal(sn)).ToList();

            var decimalSum = digitalNumbers.Sum();
            var snafuSum = DecimalToSnafu(decimalSum);

            Console.WriteLine($"Decimal sum: {decimalSum}. Snafu sum: {snafuSum}");
        }

        static string DecimalToSnafu(double decimalNumber)
        {
            double counter = decimalNumber;
            string snafuNumber = string.Empty;

            while(counter != 0)
            {
                var remainder = counter % 5;

                switch (remainder)
                {
                    case 0:
                        snafuNumber += '0';
                        counter = Math.Floor(counter/5);
                        break;
                    case 1:
                        snafuNumber += '1';
                        counter = Math.Floor(counter / 5);
                        break;
                    case 2:
                        snafuNumber += '2';
                        counter = Math.Floor(counter / 5);
                        break;
                    case 3:
                        snafuNumber += '=';
                        counter = Math.Floor(counter / 5);
                        ++counter;
                        break;
                    default:
                        snafuNumber += '-';
                        counter = Math.Floor(counter / 5);
                        ++counter;
                        break;
                }
            }

            return new string(snafuNumber.Reverse().ToArray());
        }

        static double SnafuToDecimal(string snafuNumber)
        {
            double sum = 0;
            var reversed = snafuNumber.Reverse().ToArray();

            for (int i = 0; i< snafuNumber.Length; i++)
            {
                sum += ConvertSnafuDigitToDecimal(reversed[i], Math.Pow(5, i));
            }

            return sum;
        }

        static double ConvertSnafuDigitToDecimal(char snafu, double basePosition)
        {
            return GetSnafuValue(snafu) * basePosition;
        }

        static int GetSnafuValue(char snafu) => snafu switch
        {
            '2' => 2,
            '1' => 1,
            '0' => 0,
            '-' => -1,
            _ => -2
        };

        static string GetSnafuDigit(int decimalValue) => decimalValue switch
        {
            2 => "2",
            1 => "1",
            0 => "0",
            3 => "=",
            _ => "-"
        };
    }
}
