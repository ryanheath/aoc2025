static partial class Aoc2025
{
    public static void Day4()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                ..@@.@@@@.
                @@@.@.@.@@
                @@@@@.@.@@
                @.@@@@..@.
                @@.@@@@.@@
                .@@@@@@@.@
                .@.@.@.@@@
                @.@@@.@@@@
                .@@@@@@@@.
                @.@.@@@.@.
                """.ToLines();
            Part1(input).Should().Be(13);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1480);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] grid)
        {
            var h = grid.Length;
            var w = grid[0].Length;
            var accessibleCount = 0;

            for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++)
                if (grid[y][x] == '@' && CountSurrounding(y, x) < 4)
                {
                    accessibleCount++;
                }

            return accessibleCount;

            int CountSurrounding(int y, int x)
            {
                var sy = y - 1 < 0 ? 0 : y - 1;
                var ey = y + 1 >= h ? h - 1 : y + 1;
                var sx = x - 1 < 0 ? 0 : x - 1;
                var ex = x + 1 >= w ? w - 1 : x + 1;

                var rolls = 0;

                for (var iy = sy; iy <= ey; iy++)
                for (var ix = sx; ix <= ex; ix++)
                    if (iy == y && ix == x)
                        continue;
                    else if (grid[iy][ix] == '@')
                        rolls++;

                return rolls;
            }
        }
        int Part2(string[] grid) => 0;
    }
}