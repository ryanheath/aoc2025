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
            input = 
                """
                svr: aaa bbb
                aaa: fft
                fft: ccc
                bbb: tty
                tty: ccc
                ccc: ddd eee
                ddd: hub
                hub: fff
                eee: dac
                dac: fff
                fff: ggg hhh
                ggg: out
                hhh: out
                """.ToLines();
            Part2(input).Should().Be(2);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(599);
            Part2(input).Should().Be(393474305030400L);
        }

        int Part1(string[] lines)
        {
            var edges = ParseInput(lines);

            Queue<string> toVisit = new();
            foreach (var dev in edges["you"])
                toVisit.Enqueue(dev);

            var paths = 0;

            while (toVisit.Count > 0)
            {
                var to = toVisit.Dequeue();

                if (to == "out") { paths++; continue; }

                foreach (var next in edges[to])
                    toVisit.Enqueue(next);
            }

            return paths;
        }
        long Part2(string[] lines)
        {
            var edges = ParseInput(lines);

            var nodes = new HashSet<string>(edges.Keys);
            foreach (var kv in edges)
                foreach (var to in kv.Value)
                    nodes.Add(to);

            var visits = new Dictionary<string, int>();
            foreach (var n in nodes) visits[n] = 0;
            foreach (var kv in edges)
                foreach (var to in kv.Value)
                    visits[to] += 1;

            // Kahn's algorithm for topo order
            var q = new Queue<string>();
            q.Enqueue("svr");
            
            var topoOrder = new List<string>();
            while (q.Count > 0)
            {
                var node = q.Dequeue();
                topoOrder.Add(node);

                if (node == "out") continue;

                foreach (var next in edges[node])
                {
                    var v = visits[next] -= 1;
                    if (v == 0) q.Enqueue(next);
                }
            }

            // paths[node][state] where state: 0-none,1-fft,2-dac,3-both
            var paths = new Dictionary<string, long[]>();
            foreach (var n in nodes) paths[n] = new long[4];

            paths["out"][3] = 1; // end with both

            // Process in reverse topo order (from sinks up)
            foreach (var node in topoOrder.AsEnumerable().Reverse())
            {
                var fft = node == "fft";
                var dac = node == "dac";

                if (node == "out") continue;
                foreach (var next in edges[node])
                {
                    for (var s = 0; s < 4; s++)
                    {
                        var ns = s;
                        if (fft) ns |= 1;
                        if (dac) ns |= 2;
                        paths[node][s] += paths[next][ns];
                    }
                }
            }

            return paths["svr"][0]; // start with none
        }

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