static class MathExtensions
{
    public static T Gcd<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero) (b, a) = (a % b, b);

        return a;
    }

    public static T Lcm<T>(T a, T b) where T : INumber<T> => b / Gcd(a, b) * a;

    public static T Gcd<T>(this IEnumerable<T> numbers) where T : INumber<T> => numbers.Aggregate(Gcd);

    public static T Lcm<T>(this IEnumerable<T> numbers) where T : INumber<T>  => numbers.Aggregate(Lcm);

    public static int PositiveMod(int a, int b) => (a % b + b) % b;

    public static long SumOfN(long n) => n * (n + 1) / 2;
    public static long SumOfN(long start, long end) => SumOfN(end) - SumOfN(start - 1);

    public static int NumberOfDigits(long n) => n switch
    {
        < 10 => 1,
        < 100 => 2,
        < 1000 => 3,
        < 10000 => 4,
        < 100000 => 5,
        < 1000000 => 6,
        < 10000000 => 7,
        < 100000000 => 8,
        < 1000000000 => 9,
        < 10000000000 => 10,
        < 100000000000 => 11,
        _ => throw new ArgumentOutOfRangeException(nameof(n), "Number is too large"),
    };
}
