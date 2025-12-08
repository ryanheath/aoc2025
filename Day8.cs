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

        int Part1(string[] lines, int maxConnections)
        {
            var points = ParsePoints(lines);
            var distances = GetDistances(points);
            List<HashSet<int>> circuits = [];

            var connections = 0;

            foreach (var (i1, i2, _) in distances.OrderBy(x => x.distance))
            {
                if (connections == maxConnections)
                {
                    break;
                }

                var i1InCircuit = circuits.FirstOrDefault(c => c.Contains(i1));
                var i2InCircuit = circuits.FirstOrDefault(c => c.Contains(i2));

                if (i1InCircuit is not null && i2InCircuit is not null)
                {
                    if (i1InCircuit != i2InCircuit)
                    {
                        // merge i1 circuit into i2 circuit
                        i2InCircuit.UnionWith(i1InCircuit);
                        circuits.Remove(i1InCircuit);
                    }
                    connections++;
                    continue;
                }

                if (i1InCircuit is not null)
                {
                    i1InCircuit.Add(i2);
                    connections++;
                    continue;
                }

                if (i2InCircuit is not null)
                {
                    i2InCircuit.Add(i1);
                    connections++;
                    continue;
                }

                circuits.Add([i1, i2]);
                connections++;
            }

            return circuits
                .OrderByDescending(x => x.Count)
                .Take(3)
                .Aggregate(1, (a, b) => a * b.Count);
        }
        long Part2(string[] lines)
        {
            var points = ParsePoints(lines);
            var distances = GetDistances(points);
            List<HashSet<int>> circuits = [];

            foreach (var (i1, i2, _) in distances.OrderBy(x => x.distance))
            {
                var i1InCircuit = circuits.FirstOrDefault(c => c.Contains(i1));
                var i2InCircuit = circuits.FirstOrDefault(c => c.Contains(i2));

                if (i1InCircuit is not null && i2InCircuit is not null)
                {
                    if (i1InCircuit != i2InCircuit)
                    {
                        // merge i1 circuit into i2 circuit
                        i2InCircuit.UnionWith(i1InCircuit);
                        circuits.Remove(i1InCircuit);
                        if (circuits.Count == 1 && circuits[0].Count == points.Length)
                        {
                            return points[i1].X * (long)points[i2].X;
                        }
                    }
                    continue;
                }

                if (i1InCircuit is not null)
                {
                    i1InCircuit.Add(i2);
                    if (circuits.Count == 1 && circuits[0].Count == points.Length)
                    {
                        return points[i1].X * (long)points[i2].X;
                    }
                    continue;
                }

                if (i2InCircuit is not null)
                {
                    i2InCircuit.Add(i1);
                    if (circuits.Count == 1 && circuits[0].Count == points.Length)
                    {
                        return points[i1].X * (long)points[i2].X;
                    }
                    continue;
                }

                circuits.Add([i1, i2]);
            }
            throw new UnreachableException();
        }

        static List<(int i, int j, long distance)> GetDistances((int X, int Y, int Z)[] points)
        {
            List<(int i, int j, long distance)> distances = [];
            for (var i = 0; i < points.Length; i++)
                for (var j = i + 1; j < points.Length; j++)
                    distances.Add((i, j, DistanceSquared(points[i], points[j])));
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