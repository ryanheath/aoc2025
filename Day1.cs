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
            Part1(50, input).Should().Be(3);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(50, input).Should().Be(1071);
            Part2(input).Should().Be(0);
        }

        int Part1(int startingPoint, string[] lines)
        {
            var countZeroes = 0;

            foreach (var rotation in ParseRotations(lines))
            {
                startingPoint += rotation;
                startingPoint = PositiveMod(startingPoint, 100);
                if (startingPoint == 0) { countZeroes++; }
            }

            return countZeroes;
        }
        int Part2(string[] lines) => 0;

        static IEnumerable<int> ParseRotations(string[] lines) =>
            lines.Select(line => (line[0] == 'L' ? -1 : 1) * line[1..].ToInt());
    }
}