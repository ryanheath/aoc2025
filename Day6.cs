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
            Part2(input).Should().Be(3263827);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(5361735137219);
            Part2(input).Should().Be(11744693538946L);
        }

        long Part1(string[] lines) => SolveProblems(ParseInput(lines));
        long Part2(string[] lines) => SolveProblems(ParseInputRightToLeft(lines));

        static long SolveProblems((char[] ops, int[][] numbers) input)
        {
            var (ops, numbers) = input;
            var sum = 0L;

            for (var i = 0; i < ops.Length; i++)
            {
                sum += SolveProblem(ops[i], numbers[i]);
            }

            return sum;

            long SolveProblem(char op, int[] numbers)
            {
                Func<long, long, long> operation = op == '*' ? MulOp : PlusOp;
                var result = (long)numbers[0];

                for (var y = 1; y < numbers.Length; y++)
                {
                    result = operation(result, numbers[y]);
                }

                return result;
            }
            static long MulOp(long a, long b) => a * b;
            static long PlusOp(long a, long b) => a + b;
        }

        static (char[] ops, int[][] numbers) ParseInput(string[] lines)
        {
            char[] ops = [.. lines[^1].Where(c => c is '+' or '*')];
            int[][] numbers = [.. ops.Select(_ => new int[lines.Length - 1])];

            for (var j = 0; j < lines.Length - 1; j++)
            {
                var numsInLine = lines[j].ToInts(" ");
                for (var k = 0; k < numsInLine.Length; k++)
                {
                    numbers[k][j] = numsInLine[k];
                }
            }

            return (ops, numbers);
        }

        static (char[] operators, int[][] numbers) ParseInputRightToLeft(string[] lines)
        {
            var operatorsLine = lines[^1];
            char[] operators = [.. operatorsLine.Where(c => c is '+' or '*')];
            // make enough space for numbers
            int[][] numbers = new int[operators.Length][];

            // read numbers right to left
            var problemIndex = 0;
            for (var index = 0; index < operatorsLine.Length; index++)
            {
                var indexNextOperator = operatorsLine.IndexOfAny(['+', '*'], index + 1);
                if (indexNextOperator == -1) indexNextOperator = operatorsLine.Length + 1;
                var nrDigits = indexNextOperator - index - 1; // last space before operator
                
                numbers[problemIndex] = new int[nrDigits];
                for (var digitIndex = 0; digitIndex < nrDigits; digitIndex++)
                {
                    var number = 0;
                    for (var lineIndex = 0; lineIndex < lines.Length - 1; lineIndex++)
                    {
                        var c = lines[lineIndex][index + digitIndex];
                        if (c != ' ') number = number * 10 + (c - '0');
                    }

                    numbers[problemIndex][digitIndex] = number;
                }

                // skip to next operator
                index = indexNextOperator - 1;
                problemIndex++;
            }

            return (operators, numbers);
        }
    }
}