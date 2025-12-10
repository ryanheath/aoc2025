static partial class Aoc2025
{
    public static void Day10()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
                [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
                [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
                """.ToLines();
            Part1(input).Should().Be(7);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(422);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            return ParseManual(lines).Sum(FewestPresses);

            int FewestPresses((char[] lights, int[][] buttons, int[] joltage) config)
            {
                var (lights, buttons, joltage) = config;

                var mask = 0;
                for (var i = 0; i < lights.Length; i++)
                    if (lights[i] == '#') mask |= (1 << i);

                var queue = new Queue<(int state, int b, int count)>();
                var seen = new Dictionary<int, int>();

                // start of the buttons
                for (var b = 0; b < buttons.Length; b++)
                    queue.Enqueue((0, b, 0));

                while (queue.Count > 0)
                {
                    var (state, b, count) = queue.Dequeue();

                    // flip state with button b
                    var button = buttons[b];
                    foreach (var pos in button) state ^= (1 << pos);
                    if (state == mask) return count + 1;

                    // have we seen this with fewer presses?
                    if (seen.TryGetValue(state, out var existingCount) && existingCount <= count + 1)
                        continue;

                    seen[state] = count + 1;

                    // enqueue next button presses
                    for (var nb = 0; nb < buttons.Length; nb++)
                    {
                        // skip same button
                        if (nb == b) continue;
                        queue.Enqueue((state, nb, count + 1));
                    }
                }

                throw new UnreachableException();
            }
        }
        int Part2(string[] lines) => 0;

        static (char[] lights, int[][] buttons, int[] joltage)[] ParseManual(string[] lines)
        {
            return [.. lines.Select(ParseMachineConfig)];

            static (char[] lights, int[][] buttons, int[] joltage) ParseMachineConfig(string line)
            {
                var endLights = line.IndexOf(']');
                var lights = line[1 .. endLights].ToCharArray();
                var startJoltage = line.IndexOf('{');
                int[][] buttons = [.. line[(endLights + 3) .. (startJoltage - 2)]
                    .Split(") (", StringSplitOptions.RemoveEmptyEntries)
                    .Select(part => part.ToInts(","))];
                var joltage = line[(startJoltage + 1) .. (line.Length - 1)].ToInts(",");
                return (lights, buttons, joltage);
            }
        }
    }
}