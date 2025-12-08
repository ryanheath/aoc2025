using System.Security.Policy;

static partial class Aoc2025
{
    public static void Day8()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                162,817,812
                57,618,57
                906,360,560
                592,479,940
                352,342,300
                466,668,158
                542,29,236
                431,825,988
                739,650,466
                52,470,668
                216,146,977
                819,987,18
                117,168,530
                805,96,715
                346,949,466
                970,615,88
                941,993,340
                862,61,35
                984,92,344
                425,690,689
                """.ToLines();
            Part1(input, 10).Should().Be(40);
            Part2(input).Should().Be(25272);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input, 1000).Should().Be(46398);
            Part2(input).Should().Be(8141888143L);
        }

        long Part1(string[] lines, int maxConnections)
        {
            var connections = 0;
            return CombinePoints(lines, (points, circuits, _, _) =>
            {
                connections++;
                if (connections >= maxConnections)
                {
                    int[] top3 = [.. circuits.OrderByDescending(x => x.Count).Select(x => x.Count).Take(3)];
                    return (true, top3[0] * top3[1] * top3[2]);
                }
                return (false, 0);
            });
        }
        long Part2(string[] lines)
        {
            return CombinePoints(lines, (points, circuits, i1, i2) =>
            {
                if (circuits.Count == 1 && circuits[0].Count == points.Length)
                {
                    return (true, points[i1].X * (long)points[i2].X);
                }
                return (false, 0L);
            });
        }

        long CombinePoints(string[] lines, Func<(int X, int Y, int Z)[], List<HashSet<int>>, int, int, (bool, long)> breakCondition)
        {
            var points = ParsePoints(lines);
            var distances = GetDistances(points);
            List<HashSet<int>> circuits = [];

            while (distances.Count > 0)
            {
                var (i1, i2) = distances.Dequeue();

                var circuitI1 = circuits.FirstOrDefault(c => c.Contains(i1));
                var circuitI2 = circuits.FirstOrDefault(c => c.Contains(i2));

                if (circuitI1 is not null && circuitI2 is not null)
                {
                    if (circuitI1 != circuitI2)
                    {
                        // merge i1 circuit into i2 circuit
                        circuitI2.UnionWith(circuitI1);
                        circuits.Remove(circuitI1);
                    }
                }
                else if (circuitI1 is not null) { circuitI1.Add(i2); } 
                else if (circuitI2 is not null) { circuitI2.Add(i1); }
                else { circuits.Add([i1, i2]);}

                var (shouldBreak, result) = breakCondition(points, circuits, i1, i2);
                if (shouldBreak)
                {
                    return result;
                }
            }
            throw new UnreachableException();
        }

        static PriorityQueue<(int i, int j), long> GetDistances((int X, int Y, int Z)[] points)
        {
            PriorityQueue<(int i, int j), long> distances = new();
            for (var i = 0; i < points.Length; i++)
                for (var j = i + 1; j < points.Length; j++)
                    distances.Enqueue((i, j), DistanceSquared(points[i], points[j]));
            return distances;
        }

        static long DistanceSquared((int X, int Y, int Z) p1, (int X, int Y, int Z) p2)
        {
            long dx = p1.X - p2.X;
            long dy = p1.Y - p2.Y;
            long dz = p1.Z - p2.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        static (int X, int Y, int Z)[] ParsePoints(string[] lines) => [.. lines.Select(ParsePoint)];
        static (int X, int Y, int Z) ParsePoint(string line)
        {
            var point = line.ToInts(",");
            return (point[0], point[1], point[2]);
        }
    }
}