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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1690);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            var grid = ParseInput(lines);
            var h = grid.Length;
            var w = grid[0].Length;

            var splitted = 0;

            for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++)
                if (grid[y][x] is '|' or 'S')
                {
                    var by = y + 1;
                    if (by >= h) continue;

                    switch (grid[by][x])
                    {
                        case '.':
                            grid[by][x] = '|';
                            break;
                        case '^':
                            splitted++;
                            var lx = x - 1; if (lx >= 0 && grid[by][lx] == '.') grid[by][lx] = '|';
                            var rx = x + 1; if (rx  < w && grid[by][rx] == '.') grid[by][rx] = '|';
                            break;
                    }
                }

            return splitted;
        }
        int Part2(string[] lines) => 0;

        char[][] ParseInput(string[] lines) => [.. lines.Select(x => x.ToCharArray())];
    }
}