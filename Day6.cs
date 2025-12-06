static partial class Aoc2025
{
    public static void Day6()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                123 328  51 64
                 45 64  387 23
                  6 98  215 314
                *   +   *   +
                """.ToLines();
            Part1(input).Should().Be(4277556);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(5361735137219);
            Part2(input).Should().Be(0);
        }

        long Part1(string[] lines)
        {
            var (numbers, operators) = ParseInput(lines);
            var sum = 0L;

            for (var x = 0; x < numbers[0].Length; x++)
                sum += SolveProblem(x, operators[x] == '+' ? PlusOp : MulOp);

            return sum;

            long SolveProblem(int x, Func<long, long, long> operation)
            {
                long result = numbers[0][x];

                for (var y = 1; y < numbers.Length; y++)
                    result = operation(result, numbers[y][x]);

                return result;
            }
            static long PlusOp(long a, long b) => a + b;
            static long MulOp(long a, long b) => a * b;
        }
        int Part2(string[] lines) => 0;

        (int[][] numbers, char[] operators) ParseInput(string[] lines)
        {
            int[][] numbers = [.. lines[..^1].Select(line => line.ToInts(" "))];
            char[] operators = [.. lines[^1].Where(c => c is '+' or '*')];
            return (numbers, operators);
        }
    }
}