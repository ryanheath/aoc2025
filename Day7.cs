static partial class Aoc2025
{
    public static void Day7()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                .......S.......
                ...............
                .......^.......
                ...............
                ......^.^......
                ...............
                .....^.^.^.....
                ...............
                ....^.^...^....
                ...............
                ...^.^...^.^...
                ...............
                ..^...^.....^..
                ...............
                .^.^.^.^.^...^.
                ...............
                """.ToLines();
            Part1(input).Should().Be(21);
            Part2(input).Should().Be(40);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1690);
            Part2(input).Should().Be(221371496188107L);
        }

        int Part1(string[] grid)
        {
            var splitted = 0;
            var h = grid.Length;
            var w = grid[0].Length;
            var start = (0, grid[0].IndexOf('S'));

            var beamsQueue = new Queue<(int Y, int X)>();
            beamsQueue.Enqueue(start);

            while (beamsQueue.Count > 0)
            {
                var (y, x) = beamsQueue.Dequeue();
                var by = y + 1;
                if (by >= h)
                {
                    // exited the grid
                    continue;
                }

                switch (grid[by][x])
                {
                    case '.': EnqueueBeam((by, x)); break;
                    case '^':
                        splitted++;
                        var lx = x - 1; if (lx >= 0 && grid[by][lx] == '.') EnqueueBeam((by, lx));
                        var rx = x + 1; if (rx  < w && grid[by][rx] == '.') EnqueueBeam((by, rx));
                        break;
                }
            }

            return splitted;

            void EnqueueBeam((int, int) p) { if (!beamsQueue.Contains(p)) beamsQueue.Enqueue(p); }
        }
        long Part2(string[] grid)
        {
            var countExits = 0L;
            var h = grid.Length;
            var w = grid[0].Length;
            var start = (0, grid[0].IndexOf('S'));

            var beamsQueue = new Queue<(int Y, int X)>();
            beamsQueue.Enqueue(start);
            var beams = new Dictionary<(int Y, int X), long>() { [start] = 1 };

            while (beamsQueue.Count > 0)
            {
                var beam = beamsQueue.Dequeue();
                var count = beams[beam];
                beams.Remove(beam); // keep dictionary small
                var (y, x) = beam;
                var by = y + 1;
                if (by >= h)
                {
                    // exited the grid
                    countExits += count;
                    continue;
                }

                switch (grid[by][x])
                {
                    case '.': EnqueueBeam((by, x), count); break;
                    case '^':
                        var lx = x - 1; if (lx >= 0 && grid[by][lx] == '.') EnqueueBeam((by, lx), count);
                        var rx = x + 1; if (rx  < w && grid[by][rx] == '.') EnqueueBeam((by, rx), count);
                        break;
                }
            }

            return countExits;

            void EnqueueBeam((int, int) p, long count)
            {
                if (!beams.ContainsKey(p)) beams[p] = count; else beams[p] += count;
                if (!beamsQueue.Contains(p)) beamsQueue.Enqueue(p);
            }
        }
    }
}