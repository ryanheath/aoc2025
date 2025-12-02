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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(9188031749L);
            Part2(input).Should().Be(0);
        }

        long Part1(string[] lines) => ParseRanges(lines).Select(SumOfSillyIds).Sum();
        int Part2(string[] lines) => 0;

        static long SumOfSillyIds((long start, long end) range)
        {
            var (start, end) = range;

            // will refactor this mess later

            var startShiftValue = GetShiftValue(start);
            if (startShiftValue < 0)
            {
                startShiftValue = -startShiftValue;
                start = (long)Math.Pow(10, startShiftValue);
                startShiftValue = (int)Math.Pow(10, (startShiftValue + 1) >> 1);
            }
            var startShifted = start / startShiftValue;
            var startIdShifted = ShiftedId(startShiftValue, startShifted);
            var endShiftValue = GetShiftValue(end);
            if (endShiftValue < 0)
            {
                endShiftValue = -endShiftValue - 1;
                end = (long)Math.Pow(10, endShiftValue) - 1;
                endShiftValue = (int)Math.Pow(10, endShiftValue >> 1);
            }
            var endShifted = end / endShiftValue;
            var endIdShifted = ShiftedId(endShiftValue, endShifted);

            if (startIdShifted < start)
            {
                startShifted++;
            }

            if (endIdShifted > end)
            {
                endShifted--;
            }

            if (startShifted > endShifted)
            {
                // no valid silly IDs in range
                return 0;
            }

            var sum = SumOfN(startShifted, endShifted);

            sum += sum * (startShiftValue);

            return sum;

            static int GetShiftValue(long value)
            {
                var nrDigits = (int)Math.Log10(value) + 1;
                return nrDigits % 2 != 0 ? -nrDigits : (int)Math.Pow(10, nrDigits >> 1);
            }
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