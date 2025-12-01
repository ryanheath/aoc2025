static partial class Aoc2025
{
    public static void Day1()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                L68
                L30
                R48
                L5
                R60
                L55
                L1
                L99
                R14
                L82
                """.ToLines();
            Part1(input).Should().Be(3);
            Part2(input).Should().Be(6);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1071);
            Part2(input).Should().Be(6700);
        }

        int Part1(string[] lines)
        {
            var startingPoint = 50;
            var countZeroes = 0;

            foreach (var rotation in ParseRotations(lines))
            {
                startingPoint = PositiveMod(startingPoint + rotation, 100);
                if (startingPoint == 0) { countZeroes++; }
            }

            return countZeroes;
        }

        int Part2(string[] lines)
        {
            var startingPoint = 50;
            var countZeroClicks = 0;

            foreach (var rotation in ParseRotations(lines))
            {
                var zeroClicks = Math.Abs(rotation) / 100;
                var remainder = rotation % 100;
                var tempPoint = startingPoint + remainder;
                if (tempPoint <= 0)
                {
                    if (startingPoint != 0)
                    {
                        zeroClicks++;
                    }
                    if (tempPoint < 0)
                    {
                        tempPoint += 100;
                    }
                }
                else if (tempPoint >= 100)
                {
                    zeroClicks++;
                    tempPoint -= 100;
                }

                startingPoint = tempPoint;
                countZeroClicks += zeroClicks;
            }

            return countZeroClicks;
        }

        static IEnumerable<int> ParseRotations(string[] lines) =>
            lines.Select(line => (line[0] == 'L' ? -1 : 1) * line[1..].ToInt());
    }
}