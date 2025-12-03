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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(17100);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] banks) => banks.Select(MaxJolt).Sum();

        int Part2(string[] banks) => 0;

        static int MaxJolt(string bank)
        {
            var (index, firstDigit) = GetMaxDigitFrom(0, bank.Length - 1); // exclude last digit
            var (_, nextDigit) = GetMaxDigitFrom(index + 1, bank.Length);

            return (firstDigit - '0') * 10 + (nextDigit - '0');

            (int index, char digit) GetMaxDigitFrom(int startIndex, int endIndex)
            {
                var maxDigit = '0';
                var index = -1;
                for (var i = startIndex; i < endIndex; i++)
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