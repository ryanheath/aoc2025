static partial class Aoc2025
{
    public static void Day3()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                987654321111111
                811111111111119
                234234234234278
                818181911112111
                """.ToLines();
            Part1(input).Should().Be(357);
            Part2(input).Should().Be(3121910778619L);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(17100);
            Part2(input).Should().Be(170418192256861L);
        }

        int Part1(string[] banks) => banks.Select(MaxJolt2Batteries).Sum();
        long Part2(string[] banks) => banks.Select(MaxJolt12Batteries).Sum();

        static int MaxJolt2Batteries(string bank) => (int)MaxJolt(bank, 2);
        static long MaxJolt12Batteries(string bank) => MaxJolt(bank, 12);

        static long MaxJolt(string bank, int nrOfBatteries)
        {
            var jolt = 0L;
            var index = 0;

            for (var i = 0; i < nrOfBatteries; i++)
            {
                var (atIndex, digit) = GetMaxDigitFrom(index, nrOfBatteries - i - 1); // leave enough digits for remaining batteries
                jolt = jolt * 10 + (digit - '0');
                index = atIndex + 1;
            }

            return jolt;

            (int index, char digit) GetMaxDigitFrom(int startIndex, int endIndex)
            {
                var maxDigit = '0';
                var index = -1;
                for (var i = startIndex; i < bank.Length - endIndex; i++)
                {
                    var c = bank[i];
                    if (c > maxDigit)
                    {
                        maxDigit = c;
                        index = i;
                    }
                }

                return (index, maxDigit);
            }
        }
    }
}