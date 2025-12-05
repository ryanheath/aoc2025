static partial class Aoc2025
{
    public static void Day5()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                3-5
                10-14
                16-20
                12-18

                1
                5
                8
                11
                17
                32
                """.ToLines();
            Part1(input).Should().Be(3);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(896);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            var (ranges, ingredients) = ParseInput(lines);
            var freshIngredients = 0;

            foreach (var ingredient in ingredients)
                if (IsFresh(ingredient)) freshIngredients++;

            return freshIngredients;

            bool IsFresh(long ingredient)
            {
                foreach (var (start, end) in ranges)
                    if (ingredient >= start && ingredient <= end)
                        return true;
                return false;
            }
        }

        int Part2(string[] lines) => 0;

        static (List<(long start, long end)> ranges, List<long> ingredients) ParseInput(string[] lines)
        {
            var i = 0;
            var ranges = new List<(long start, long end)>();
            for (; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line == "") break;

                ranges.Add(line.To2Longs("-"));
            }

            var ingredients = new List<long>();
            for (i++; i < lines.Length; i++)
                ingredients.Add(lines[i].ToLong());

            return (ranges, ingredients);
        }
    }
}