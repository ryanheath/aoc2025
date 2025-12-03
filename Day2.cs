using System.Net.WebSockets;

static partial class Aoc2025
{
    public static void Day2()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
                """.ToLines();
            Part1(input).Should().Be(1227775554L);
            Part2(input).Should().Be(0);//4174379265L);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(9188031749L);
            Part2(input).Should().Be(0);
        }

        long Part1(string[] lines) => ParseRanges(lines).Select(SumOfSillyIds).Sum();
        long Part2(string[] lines) => 0;

        static long SumOfSillyIds((long start, long end) range)
        {
            var (start, end) = range;

            var startShiftDigits = GetStartShiftDigits();
            var startShifted = start / startShiftDigits;

            // adjust startShifted upwards if needed
            if (ShiftedId(startShiftDigits, startShifted) < start)
            {
                startShifted++;
            }

            var endShiftValue = GetEndShiftDigits();
            var endShifted = end / endShiftValue;

            // adjust endShifted downwards if needed
            if (ShiftedId(endShiftValue, endShifted) > end)
            {
                endShifted--;
            }

            if (startShifted > endShifted)
            {
                // no valid silly IDs in range
                return 0;
            }

            return SumOfN(startShifted, endShifted) * (startShiftDigits + 1);

            int GetStartShiftDigits()
            {
                var startNrDigits = NumberOfDigits(start);

                if (startNrDigits % 2 != 0)
                {
                    // adjust start to be the smallest even digit number with more digits
                    start = (long)Math.Pow(10, startNrDigits);
                    return GetShiftDigits(startNrDigits + 1);
                }

                return GetShiftDigits(startNrDigits);
            }

            int GetEndShiftDigits()
            {
                var endNrDigits = NumberOfDigits(end);

                if (endNrDigits % 2 != 0)
                {
                    // adjust end to be the largest even digit number with fewer digits
                    endNrDigits -= 1;
                    end = (long)Math.Pow(10, endNrDigits) - 1;
                    return GetShiftDigits(endNrDigits);
                }

                return GetShiftDigits(endNrDigits);
            }

            static int GetShiftDigits(int nrDigits) => (int)Math.Pow(10, nrDigits >> 1);

            static long ShiftedId(int shiftValue, long shifted) => shifted * shiftValue + shifted;
        }

        IEnumerable<(long start, long end)> ParseRanges(string[] lines)
        {
            foreach(var ids in lines[0].Split(','))
            {
                yield return ids.To2Longs("-");
            }
        }
    }
}