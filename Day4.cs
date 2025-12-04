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
            Part2(input).Should().Be(43);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1480);
            Part2(input).Should().Be(8899);
        }

        int Part1(string[] lines)
        {
            var grid = ParseGrid(lines);
            var h = grid.Length;
            var w = grid[0].Length;
            var accessibleCount = 0;

            for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++)
                if (IsAccessible(grid, y, x)) accessibleCount++;

            return accessibleCount;
        }
        int Part2(string[] lines)
        {
            var grid = ParseGrid(lines);
            var h = grid.Length;
            var w = grid[0].Length;
            var removed = 0;
            var toBeRemoved = new List<(int y, int x)>();

            do
            {
                for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                    if (IsAccessible(grid, y, x)) toBeRemoved.Add((y, x));

                if (toBeRemoved.Count == 0) break;

                removed += toBeRemoved.Count;
                foreach (var (y, x) in toBeRemoved)
                    grid[y][x] = '.';
                toBeRemoved.Clear();

            } while (true);

            return removed;
        }

        static bool IsAccessible(char[][] grid, int y, int x) => grid[y][x] == '@' && CountSurrounding(grid, y, x) < 5;

        static char[][] ParseGrid(string[] lines) => [.. lines.Select(line => line.ToCharArray())];
        static int CountSurrounding(char[][] grid, int y, int x)
        {
            var sy = y - 1 < 0 ? 0 : y - 1;
            var ey = y + 1 >= grid.Length ? grid.Length - 1 : y + 1;
            var sx = x - 1 < 0 ? 0 : x - 1;
            var ex = x + 1 >= grid[0].Length ? grid[0].Length - 1 : x + 1;

            var rolls = 0;

            for (var iy = sy; iy <= ey; iy++)
            for (var ix = sx; ix <= ex; ix++)
                if (grid[iy][ix] == '@')
                    rolls++;

            return rolls;
        }
    }
}