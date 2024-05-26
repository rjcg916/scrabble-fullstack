namespace Scrabble.Domain.Model
{
    public enum R
    {
        _1 = 1, _2, _3, _4, _5, _6, _7, _8,
        _9, _10, _11, _12, _13, _14, _15
    }

    public enum C
    {
        A = 1, B, C, D, E, F, G, H,
        I, J, K, L, M, N, O
    }

    public class Coord(R row, C col)
    {
        public R Row { get; } = row;
        public C Col { get; } = col;

        public override string ToString()
        {
            return $"{col}{(int)row}";
        }
    }

    public class Span
    {
        Coord Start { get; }
        Coord End { get; }
    }

    public class Endpoints
    {
        int Start { get; }
        int End { get; }

        Endpoints(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }
    }
}