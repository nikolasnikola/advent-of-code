using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal static class Day10
    {
        internal static void Part1()
        {
            var input = Reader.ReadAsIntegerArray("2020", "Day10");

            Array.Sort(input);

            var ones = 0;
            var threes = 1;

            if (input[0] == 1) ones++;
            if (input[0] == 3) threes++;

            for (int i = 1; i < input.Length; i++)
            {
                var difference = input[i] - input[i-1];
                if (difference == 1) ones++;
                if (difference == 3) threes++;
            }
           
            Console.WriteLine(ones*threes);
        }

        internal static void Part2()
        {
            var input = Reader.ReadAsIntegerArray("2020", "Day10");
            Array.Sort(input);

            var list = new List<int> { 0 };
            list.AddRange(input);
            list.Add(input.Last() + 3);

            var arr = list.ToArray();
            var result = CountWays(arr);
            Console.WriteLine(result);
        }


        static long CountWays(int[] arr)
        {
            int n = arr.Length;

            long[] dp = new long[n];

            dp[0] = 1;
            long result = 0;

            for (int i = 1; i < n; i++)
            {
                dp[i] = 0;
                for (int j = 0; j < i; j++)
                {
                    if (arr[i] - arr[j] <= 3)
                    {
                        dp[i] += dp[j];
                    }
                }
                result = dp[i];
            }

            return result;
        }
    }


}
