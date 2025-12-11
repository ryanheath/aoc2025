static partial class Aoc2025
{
    public static void Day11()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                aaa: you hhh
                you: bbb ccc
                bbb: ddd eee
                ccc: ddd eee fff
                ddd: ggg
                eee: out
                fff: out
                ggg: out
                hhh: ccc fff iii
                iii: out
                """.ToLines();
            Part1(input).Should().Be(5);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(599);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            var edges = ParseInput(lines);

            Queue<(string, string)> toVisit = new();
            var youDevs = edges["you"];
            foreach (var dev in youDevs)
                toVisit.Enqueue(("you", dev));

            var paths = 0;

            while (toVisit.Count > 0)
            {
                var (_, to) = toVisit.Dequeue();

                if (to == "out") { paths++; continue; }

                foreach (var next in edges[to])
                    toVisit.Enqueue((to, next));
            }

            return paths;
        }
        int Part2(string[] lines) => 0;

        static Dictionary<string, string[]> ParseInput(string[] lines)
        {
            var dict = new Dictionary<string, string[]>();
            foreach (var line in lines)
            {
                var parts = line.Split(": ");
                var key = parts[0];
                var values = parts[1].Split(' ');
                dict[key] = values;
            }
            return dict;
        }
    }
}