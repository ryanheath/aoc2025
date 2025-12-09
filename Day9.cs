static partial class Aoc2025
{
    public static void Day9()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                7,1
                11,1
                11,7
                9,7
                9,5
                2,5
                2,3
                7,3
                """.ToLines();
            Part1(input).Should().Be(50);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(4774877510);
            Part2(input).Should().Be(0);
        }

        long Part1(string[] lines)
        {
            var points = ParsePoints(lines);
            var areas = GetAreas(points);

            areas.TryPeek(out var _, out var biggestArea);

            return -biggestArea;
        }
        int Part2(string[] lines) => 0;

        static PriorityQueue<(int x, int y), long> GetAreas((int x, int y)[] points)
        {
            var areas = new PriorityQueue<(int x, int y), long>();

            for (var i = 0; i < points.Length; i++)
            {
                var p1 = points[i];
                for (var j = i + 1; j < points.Length; j++)
                {
                    var p2 = points[j];
                    var area = Area(p1, p2);
                    areas.Enqueue((i, j), -area);
                }
            }

            return areas;
        }
        static long Area((int x, int y) p1, (int x, int y) p2) => (Math.Abs(p1.x - (long)p2.x) + 1) * (Math.Abs(p1.y - (long)p2.y) + 1);
        static (int x, int y)[] ParsePoints(string[] lines) => [.. lines.Select(line => line.To2Ints(","))];
    }
}