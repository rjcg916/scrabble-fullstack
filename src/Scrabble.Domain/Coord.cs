namespace Scrabble.Domain
{
    public enum R : ushort
    {
        _1 = 0, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15
    }

    public enum C : ushort
    {
        A = 0, B, C, D, E, F, G, H, I, J, K, L, M, N, O
    }

    public class Coord(R row, C col)
    {
        public R Row { get; init; } = row;
        public C Col { get; init; } = col;

        public override string ToString()
        {
            return $"{Col}{(ushort)Row}";
        }
    }
}